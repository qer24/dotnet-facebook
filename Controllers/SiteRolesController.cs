using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Roles;

namespace dotnet_facebook.Controllers
{
    public class SiteRolesController : Controller
    {
        private readonly TestContext _context;

        public SiteRolesController(TestContext context)
        {
            _context = context;
        }

        // GET: SiteRoles
        public async Task<IActionResult> Index()
        {
            return View(await _context.SiteRoles.ToListAsync());
        }

        // GET: SiteRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var siteRole = await _context.SiteRoles
                .FirstOrDefaultAsync(m => m.SiteRoleId == id);
            if (siteRole == null)
            {
                return NotFound();
            }

            return View(siteRole);
        }

        // GET: SiteRoles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SiteRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SiteRoleId,SiteRoleName,IsDefault,AdministrativePerms")] SiteRole siteRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(siteRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(siteRole);
        }

        // GET: SiteRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var siteRole = await _context.SiteRoles.FindAsync(id);
            if (siteRole == null)
            {
                return NotFound();
            }
            return View(siteRole);
        }

        // POST: SiteRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SiteRoleId,SiteRoleName,IsDefault,AdministrativePerms")] SiteRole siteRole)
        {
            if (id != siteRole.SiteRoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(siteRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SiteRoleExists(siteRole.SiteRoleId))
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
            return View(siteRole);
        }

        // GET: SiteRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var siteRole = await _context.SiteRoles
                .FirstOrDefaultAsync(m => m.SiteRoleId == id);
            if (siteRole == null)
            {
                return NotFound();
            }

            return View(siteRole);
        }

        // POST: SiteRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var siteRole = await _context.SiteRoles.FindAsync(id);
            if (siteRole != null)
            {
                _context.SiteRoles.Remove(siteRole);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SiteRoleExists(int id)
        {
            return _context.SiteRoles.Any(e => e.SiteRoleId == id);
        }
    }
}
