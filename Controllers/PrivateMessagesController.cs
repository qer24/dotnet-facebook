using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Users;

namespace dotnet_facebook.Controllers
{
    public class PrivateMessagesController : Controller
    {
        private readonly TestContext _context;

        public PrivateMessagesController(TestContext context)
        {
            _context = context;
        }

        // GET: PrivateMessages
        public async Task<IActionResult> Index()
        {
            var privateMessages = await _context.PrivateMessages
                .Include(pm => pm.Sender)
                .Include(pm => pm.Receiver)
                .ToListAsync();
            return View(privateMessages);
        }

        // GET: PrivateMessages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privateMessage = await _context.PrivateMessages
                .Include(pm => pm.Sender)
                .Include(pm => pm.Receiver)
                .FirstOrDefaultAsync(m => m.PrivateMessageId == id);
            if (privateMessage == null)
            {
                return NotFound();
            }

            return View(privateMessage);
        }

        // GET: PrivateMessages/Create
        public IActionResult Create()
        {
            ViewBag.Users = _context.Users.Select(u => new SelectListItem
            {
                Value = u.UserId.ToString(),
                Text = u.Nickname
            }).ToList();

            return View();
        }

        // POST: PrivateMessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PrivateMessageId,Message")] PrivateMessage privateMessage)
        {
            var userId1 = Request.Form["selectedUserId1"];
            var userId2 = Request.Form["selectedUserId2"];
            User? user1 = null;
            User? user2 = null;

            if (string.IsNullOrWhiteSpace(userId1))
            {
                ModelState.AddModelError("selectedUserId1", "Please select a user to send a message from!");
            }
            else
            {
                user1 = _context.Users.Find(int.Parse(userId1));
            }

            if (string.IsNullOrWhiteSpace(userId2))
            {
                ModelState.AddModelError("selectedUserId2", "Please select a user to send a message to!");
            }
            else
            {
                user2 = _context.Users.Find(int.Parse(userId2));
            }

            if (userId1 == userId2)
            {
                ModelState.AddModelError("selectedUserId2", "You can't send a message to yourself!");
            }

            privateMessage.MessageDate = DateTime.Now;

            privateMessage.Sender = user1;
            privateMessage.Receiver = user2;

            if (ModelState.IsValid)
            {
                _context.Add(privateMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Users = _context.Users.Select(u => new SelectListItem
            {
                Value = u.UserId.ToString(),
                Text = u.Nickname
            }).ToList();

            return View(privateMessage);
        }

        // GET: PrivateMessages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privateMessage = await _context.PrivateMessages.FindAsync(id);
            if (privateMessage == null)
            {
                return NotFound();
            }
            return View(privateMessage);
        }

        // POST: PrivateMessages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PrivateMessageId,Message,MessageDate")] PrivateMessage privateMessage)
        {
            if (id != privateMessage.PrivateMessageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(privateMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrivateMessageExists(privateMessage.PrivateMessageId))
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
            return View(privateMessage);
        }

        // GET: PrivateMessages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privateMessage = await _context.PrivateMessages
                .FirstOrDefaultAsync(m => m.PrivateMessageId == id);
            if (privateMessage == null)
            {
                return NotFound();
            }

            return View(privateMessage);
        }

        // POST: PrivateMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var privateMessage = await _context.PrivateMessages.FindAsync(id);
            if (privateMessage != null)
            {
                _context.PrivateMessages.Remove(privateMessage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrivateMessageExists(int id)
        {
            return _context.PrivateMessages.Any(e => e.PrivateMessageId == id);
        }
    }
}
