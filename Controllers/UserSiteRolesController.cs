using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Roles;
using dotnet_facebook.Controllers.Services;
using Microsoft.AspNetCore.Authorization;

namespace dotnet_facebook.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserSiteRolesController : Controller
    {
        private readonly TestContext _context;

        public UserSiteRolesController(TestContext context)
        {
            _context = context;

            AssignUserRoles();
        }

        private void AssignUserRoles()
        {
            var usersWithoutRoles = _context.Users.Where(u => !_context.UserSiteRoles.Any(ur => ur.User.UserId == u.UserId)).ToList();
            if (!usersWithoutRoles.Any()) return;

            foreach (var user in usersWithoutRoles)
            {
                UserService.AddDefaultRoles(_context, user);
            }

            _context.SaveChanges();
        }

        // GET: UserSiteRoles
        public async Task<IActionResult> Index()
        {
            var userSiteRoles = await _context.UserSiteRoles
                .Include(u => u.User)
                .Include(u => u.Role)
                .ToListAsync();
            return View(userSiteRoles);
        }

        // GET: UserSiteRoles/Create
        public IActionResult Create()
        {
            GenerateViewBag();

            return View();
        }

        // POST: UserSiteRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserSiteRoleId")] UserSiteRole userSiteRole)
        {
            var userId = Request.Form["selectedUserId"];
            var roleId = Request.Form["selectedRoleId"];

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(roleId))
            {
                ModelState.AddModelError("selectedRoleId", "Please select a user and a role.");
            }
            else
            {
                userSiteRole.User = _context.Users.Find(int.Parse(userId));
                userSiteRole.Role = _context.SiteRoles.Find(int.Parse(roleId));

                // Find if the user already has the role
                var existingUserSiteRole = _context.UserSiteRoles
                    .FirstOrDefault(u => u.User.UserId == userSiteRole.User.UserId && u.Role.SiteRoleId == userSiteRole.Role.SiteRoleId);

                if (existingUserSiteRole != null)
                {
                    ModelState.AddModelError("selectedRoleId", "User already has this role.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(userSiteRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            GenerateViewBag();

            return View(userSiteRole);
        }

        private void GenerateViewBag()
        {
            ViewBag.Users = _context.Users.Select(u => new SelectListItem
            {
                Value = u.UserId.ToString(),
                Text = u.Nickname
            }).ToList();

            ViewBag.Roles = _context.SiteRoles.Select(r => new SelectListItem
            {
                Value = r.SiteRoleId.ToString(),
                Text = r.SiteRoleName
            }).ToList();
        }

        // GET: UserSiteRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userSiteRole = await _context.UserSiteRoles
                .Include(u => u.User)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserSiteRoleId == id);
            if (userSiteRole == null)
            {
                return NotFound();
            }

            return View(userSiteRole);
        }

        // POST: UserSiteRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userSiteRole = await _context.UserSiteRoles.FindAsync(id);
            if (userSiteRole != null)
            {
                _context.UserSiteRoles.Remove(userSiteRole);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserSiteRoleExists(int id)
        {
            return _context.UserSiteRoles.Any(e => e.UserSiteRoleId == id);
        }
    }
}
