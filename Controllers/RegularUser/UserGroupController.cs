using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Groups;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dotnet_facebook.Controllers.RegularUser
{
    [Route("UserGroup")]
    public class UserGroupController : Controller
    {
        private readonly TestContext _context;
        private readonly GroupService _groupService;
        private readonly UserService _userservice;

        public UserGroupController(TestContext context, GroupService groupService, UserService userservice)
        {
            _context = context;
            _groupService = groupService;
            _userservice = userservice;
        }

        // GET: UserGroup
        [HttpGet("{id?}")]
        [OutputCache(NoStore = true, Duration = 0)]
        public async Task<IActionResult> Index(int? id)
            {
            if (id == null)
            {
                return RedirectToAction("GroupNotFound");
            }

            var group = await _groupService.GetGroupByIdAsync(id);
            if (group == null)
            {
                return RedirectToAction("GroupNotFound");
            }
            var userRole = group.Users.FirstOrDefault(u => u.User.Nickname == User.Identity.Name)?.GroupRole;

            ViewBag.localUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.IsAdmin = userRole == GroupRole.Admin;
            ViewBag.IsMember = userRole == GroupRole.Member || userRole == GroupRole.Moderator || userRole == GroupRole.Admin;
            ViewBag.IsModerator = userRole == GroupRole.Moderator || userRole == GroupRole.Admin;
            ViewBag.Roles = Enum.GetValues(typeof(GroupRole)).Cast<GroupRole>().Select(r => new SelectListItem
            {
                Value = r.ToString(),
                Text = r.ToString()
            }).ToList();
            return View(group);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(int? GroupId)
        {
            if (GroupId == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(g => g.Users)
                .ThenInclude(gu => gu.User)
                .FirstOrDefaultAsync(m => m.GroupId == GroupId);
            if (@group == null)
            {
                return NotFound();
            }

            var selectedUserRoleString = Request.Form["selectedUserRole"];
            var selectedUserIdStrings = Request.Form["changedRoleUserId"];

            // select first non empty array element from selected user id string
            var selectedUserIdString = selectedUserIdStrings.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));

            if (string.IsNullOrWhiteSpace(selectedUserIdString))
            {
                ModelState.AddModelError("selectedUserId", "Please select a user.");
            }
            else
            {
                var selectedUserId = Convert.ToInt32(selectedUserIdString);
                var groupUser = @group.Users.FirstOrDefault(gu => gu.User.UserId == selectedUserId);

                if (groupUser == null)
                {
                    ModelState.AddModelError("selectedUserId", "User not found in group.");
                }
                // make sure user is not the last admin in the group
                else if (groupUser.GroupRole == GroupRole.Admin && @group.Users.Count(gu => gu.GroupRole == GroupRole.Admin) == 1)
                {
                    ModelState.AddModelError("selectedUserId", "Cannot remove the last admin from the group.");
                }
                else
                {
                    var selectedUserRole = Enum.Parse<GroupRole>(selectedUserRoleString);
                    groupUser.GroupRole = selectedUserRole;

                    _context.Update(@group);
                    await _context.SaveChangesAsync();
                }
            }

            GenerateUsersBag();
            GenerateRolesBag();

            return View(@group);
        }
        [HttpGet("AddUser")]
        public async Task<IActionResult> AddUser(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("GroupNotFound");
            }

            var @group = await _context.Groups
                .Include(g => g.Users)
                .ThenInclude(gu => gu.User)
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (@group == null)
            {
                return RedirectToAction("GroupNotFound");
            }

            var userStringError = "";

            // if user is already in the group, throw error
            var selectedUserIdString = Request.Form["selectedUserId"];
            if (string.IsNullOrWhiteSpace(selectedUserIdString))
            {
                userStringError = "Please select a user.";
            }
            else
            {
                var userAlreadyInGroup = @group.Users.Any(gu => gu.User.UserId == Convert.ToInt32(selectedUserIdString));

                if (userAlreadyInGroup)
                {
                    userStringError = "User is already in the group.";
                }
                else
                {
                    var user = _context.Users.Find(Convert.ToInt32(selectedUserIdString));
                    @group.Users.Add(new GroupUser
                    {
                        User = user,
                        Group = @group,
                        GroupRole = GroupRole.Member
                    });

                    try
                    {
                        _context.Update(@group);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!GroupExists(@group.GroupId))
                        {
                            return RedirectToAction("GroupNotFound");
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            GenerateUsersBag();
            GenerateRolesBag();

            return RedirectToAction("GroupNotFound");
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.GroupId == id);
        }



        private void GenerateUsersBag()
        {
            ViewBag.Users = _context.Users.Select(u => new SelectListItem
            {
                Value = u.UserId.ToString(),
                Text = u.Nickname
            }).ToList();
        }

        private void GenerateRolesBag()
        {
            ViewBag.Roles = Enum.GetValues(typeof(GroupRole)).Cast<GroupRole>().Select(r => new SelectListItem
            {
                Value = r.ToString(),
                Text = r.ToString()
            }).ToList();
        }

        [HttpPost("UpdateBio")]
        public async Task<IActionResult> UpdateBio(int GroupId, string GroupDescription)
        {
            var group = await _groupService.GetGroupByIdAsync(GroupId);
            if (group == null)
            {
                return RedirectToAction("GroupNotFound");
            }

            // if the group bio is null, set it to an empty string
            GroupDescription ??= "";

            group.GroupDescription = GroupDescription;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id = GroupId });
        }
        [HttpPost("UpdateGroupPicture")]
        [EnableCors("AllowAllOrigins")]
        public async Task<IActionResult> UpdateGroupPicture(IFormFile file, int GroupId)
        {
            if (file != null && file.Length > 0)
            {
                var group = await _groupService.GetGroupByIdAsync(GroupId);

                if (group == null)
                {
                    return Json(new { success = false, message = "Group not found" });
                }

                var fileExtension = Path.GetExtension(file.FileName);
                // file name is the user id + the file extension
                var fileName = $"grouppfp_{group.GroupId}";

                var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/uploadedFiles", fileName + fileExtension);

                // first, delete any existing file with the same name (any extension)
                var existingFiles = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/uploadedFiles"), $"{fileName}.*");
                foreach (var existingFile in existingFiles)
                {
                    System.IO.File.Delete(existingFile);
                }

                using var stream = new FileStream(path, FileMode.Create);

                await file.CopyToAsync(stream);

                group.GroupPictureFileName = fileName + fileExtension;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "File uploaded successfully" });
            }

            return Json(new { success = false, message = "No file uploaded" });
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int GroupId)
        {
            var @group = await _context.Groups.FindAsync(GroupId);
            if (@group != null)
            {
                _context.Groups.Remove(@group);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("GroupNotFound");
        }
        [HttpPost("RemoveUser")]
        public async Task<IActionResult> RemoveUser(int? userId, int? groupId)
        {
            if (userId == null || groupId == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(g => g.Users)
                .ThenInclude(gu => gu.User)
                .FirstOrDefaultAsync(m => m.GroupId == groupId);
            if (@group == null)
            {
                return NotFound();
            }

            var @user = await _context.Users.FindAsync(userId);
            if (@user == null)
            {
                return NotFound();
            }

            // Can't remove last admin
            if (@group.Users.Count(gu => gu.GroupRole == GroupRole.Admin) == 1 && @group.Users.Any(gu => gu.User.UserId == userId && gu.GroupRole == GroupRole.Admin))
            {
                return RedirectToAction("GroupNotFound");
            }

            var groupUser = @group.Users.FirstOrDefault(gu => gu.User.UserId == userId);
            if (groupUser != null)
            {
                @group.Users.Remove(groupUser);

                _context.Update(@group);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", new { id = @group.GroupId });
        }
        [HttpGet("GroupList")]
        public async Task<IActionResult> GroupList()
        {
            var groups = await _userservice.GetGroupsForLocalUserAsync(User);
            return View(groups); // Or return Json(groups) if you prefer a JSON response
        }

        [HttpGet("GroupNotFound")]
        public IActionResult GroupNotFound()
        {
            return View();
        }
    } 
}
