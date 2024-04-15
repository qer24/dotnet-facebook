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
            GenerateViewBag();

            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId,GroupName,GroupDescription,GroupPictureFileName")] Group @group)
        {
            var selecteUserIdString = Request.Form["selectedUserId"];
            
            if (string.IsNullOrWhiteSpace(selecteUserIdString))
            {
                ModelState.AddModelError("selectedUserId", "Please select a user.");
            }
            else
            {
                var selectedUserId = Convert.ToInt32(selecteUserIdString);
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
            GenerateViewBag();

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

            GenerateViewBag();

            return View(@group);
        }

        // POST: Groups/AddUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(int? id)
        {
            Console.WriteLine("Post");

            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups.FindAsync(id);
            var @group2 = await _context.Groups
                .Include(g => g.Users)
                .ThenInclude(gu => gu.User)
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (@group == null)
            {
                return NotFound();
            }

            GenerateViewBag();

            return View(@group);
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

        private void GenerateViewBag()
        {
            ViewBag.Users = _context.Users.Select(u => new SelectListItem
            {
                Value = u.UserId.ToString(),
                Text = u.Nickname
            }).ToList();
        }
    }
}
