using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Groups;
using Microsoft.IdentityModel.Tokens;
using dotnet_facebook.Models.DatabaseObjects.Users;

namespace dotnet_facebook.Controllers
{
    public class GroupsController : Controller
    {
        private readonly TestContext _context;

        public GroupsController(TestContext context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            return View(await _context.Groups.ToListAsync());
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(g => g.Users)
                .ThenInclude(gu => gu.User)
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            // Populate ViewBag.Users with a list of users to select from
            GenerateUsersBag();

            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId,GroupName,GroupDescription,GroupPictureFileName")] Group @group)
        {
            var selectedUserIdString = Request.Form["selectedUserId"];
            
            if (string.IsNullOrWhiteSpace(selectedUserIdString))
            {
                ModelState.AddModelError("selectedUserId", "Please select a user.");
            }
            else
            {
                var selectedUserId = Convert.ToInt32(selectedUserIdString);
                var user = _context.Users.Find(selectedUserId);
                group.Users =
                [
                    new GroupUser
                    {
                        User = user,
                        Group = group,
                        GroupRole = GroupRole.Admin
                    },
                ];
            }
            group.GroupCreationDate = DateTime.Now;

            if (_context.Groups.Any(g => g.GroupName == group.GroupName))
            {
                ModelState.AddModelError("GroupName", "Group Name already exists!");
            }

            if (ModelState.IsValid)
            {
                _context.Add(@group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate ViewBag.Users to maintain the data on validation failure
            GenerateUsersBag();

            return View(@group);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }
            return View(@group);
        }

        // GET: Groups/Manage
        public async Task<IActionResult> Manage(int? id, string? userIdError = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(g => g.Users)
                .ThenInclude(gu => gu.User)
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (@group == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(userIdError))
            {
                ModelState.AddModelError("selectedUserId", userIdError);
            }

            GenerateUsersBag();
            GenerateRolesBag();

            return View(@group);
        }

        // POST: Groups/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(g => g.Users)
                .ThenInclude(gu => gu.User)
                .FirstOrDefaultAsync(m => m.GroupId == id);
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

        // POST: Groups/AddUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(g => g.Users)
                .ThenInclude(gu => gu.User)
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (@group == null)
            {
                return NotFound();
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
                            return NotFound();
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

            return RedirectToAction(nameof(Manage), new { id = @group.GroupId, userIdError = userStringError });
        }

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
                return RedirectToAction(nameof(Manage), new { id = @group.GroupId, userIdError = "Cannot remove the last admin from the group." });
            }

            var groupUser = @group.Users.FirstOrDefault(gu => gu.User.UserId == userId);
            if (groupUser != null)
            {
                @group.Users.Remove(groupUser);

                _context.Update(@group);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Manage), new { id = @group.GroupId });
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupId,GroupName,GroupDescription,GroupCreationDate,GroupPictureFileName")] Group @group)
        {
            if (id != @group.GroupId)
            {
                return NotFound();
            }

            if (_context.Groups.Any(g => g.GroupName == @group.GroupName))
            {
                ModelState.AddModelError("GroupName", "Group Name already exists!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.GroupId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@group);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @group = await _context.Groups.FindAsync(id);
            if (@group != null)
            {
                _context.Groups.Remove(@group);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
    }
}
