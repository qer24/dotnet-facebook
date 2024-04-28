﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using dotnet_facebook.Models.DatabaseObjects.Roles;
using dotnet_facebook.Utils;
using Microsoft.AspNetCore.Identity;

namespace dotnet_facebook.Controllers
{
    public class UsersController : Controller
    {
        private readonly TestContext _context;

        public UsersController(TestContext context)
        {
            _context = context;

            var passwordsToChange = _context.Users.Where(u => u.HashedPassword == null);
            if (!passwordsToChange.Any()) return;

            foreach (var user in passwordsToChange)
            {
                user.HashedPassword = PasswordHash.Create(user.Password);
                user.Password = "";
            }

            _context.SaveChanges();
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Nickname,Password")] User user)
        {
            user.UserProfile = new UserProfile()
            {
                User = user,
                UserBio = "Hey, I'm a user!"
            };

            user.AccountCreationDate = DateTime.Now;

            if (_context.Users.Any(u => u.Nickname == user.Nickname))
            {
                ModelState.AddModelError("Nickname", "Nickname already exists!");
            }

            if (ModelState.IsValid)
            {
                user.HashedPassword = PasswordHash.Create(user.Password);
                user.Password = "";

                _context.Add(user);

                // Set default roles for the user
                var defaultRoles = _context.SiteRoles.Where(r => r.IsDefault).ToList();
                foreach (var role in defaultRoles)
                {
                    _context.UserSiteRoles.Add(new UserSiteRole()
                    {
                        User = user,
                        Role = role
                    });
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }   

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Nickname,AccountCreationDate")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (_context.Users.Any(u => u.Nickname == user.Nickname))
            {
                ModelState.AddModelError("Nickname", "Nickname already exists!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
