using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Groups;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
            ViewBag.IsMember = userRole == GroupRole.Member;
            return View(group);
        }
        public async Task<IActionResult> AddUser(int? id)
        {
            if (id == null)
            {
                return GroupNotFound();
            }

            var @group = await _context.Groups
                .Include(g => g.Users)
                .ThenInclude(gu => gu.User)
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (@group == null)
            {
                return GroupNotFound();
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
                            return GroupNotFound();
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

            return GroupNotFound();
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
        [HttpGet("GroupNotFound")]
        public IActionResult GroupNotFound()
        {
            return View();
        }
    } 
}
