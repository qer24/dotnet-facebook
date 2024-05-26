using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Posts;
using System.Security.Claims;
using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Utils;
using System.Globalization;

namespace dotnet_facebook.Controllers
{
    public class MainPostsController(TestContext context, UserService userService) : Controller
    {

        // GET: MainPosts
        public async Task<IActionResult> Index()
        {
            return View(await context.MainPosts
                .Include(p => p.OwnerUser)
                .ToListAsync());
        }

        // GET: MainPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainPost = await context.MainPosts
                .Include(p => p.OwnerUser)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (mainPost == null)
            {
                return NotFound();
            }

            return View(mainPost);
        }

        // GET: MainPosts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MainPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Content")] MainPost mainPost)
        {
            // get userid from identity
            var localUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (localUserId == null)
            {
                ModelState.AddModelError("Content", "Logged in User not found!");
            }

            mainPost.PostDate = DateTime.Now;

            var lat = double.Parse(Request.Cookies["latitude"]!, CultureInfo.InvariantCulture);
            var lon = double.Parse(Request.Cookies["longitude"]!, CultureInfo.InvariantCulture);

            mainPost.PostLatitude = MainPost.FromDouble(lat);
            mainPost.PostLongitude = MainPost.FromDouble(lon);

            if (ModelState.IsValid)
            {
                var localUser = await userService.GetUserByIdAsync(int.Parse(localUserId!));
                mainPost.OwnerUser = localUser!;

                context.Add(mainPost);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mainPost);
        }

        // GET: MainPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainPost = await context.MainPosts.FindAsync(id);
            if (mainPost == null)
            {
                return NotFound();
            }
            return View(mainPost);
        }

        // POST: MainPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostLongitude,PostLatitude,PostId,PostDate,Content,PostFileName")] MainPost mainPost)
        {
            if (id != mainPost.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(mainPost);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MainPostExists(mainPost.PostId))
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
            return View(mainPost);
        }

        // GET: MainPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainPost = await context.MainPosts
                .Include(p => p.OwnerUser)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (mainPost == null)
            {
                return NotFound();
            }

            return View(mainPost);
        }

        // POST: MainPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mainPost = await context.MainPosts.FindAsync(id);
            if (mainPost != null)
            {
                context.MainPosts.Remove(mainPost);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MainPostExists(int id)
        {
            return context.MainPosts.Any(e => e.PostId == id);
        }
    }
}
