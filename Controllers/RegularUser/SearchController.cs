using dotnet_facebook.Models;
using dotnet_facebook.Models.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_facebook.Controllers.RegularUser
{
    public class SearchController(TestContext context) : Controller
    {
        // GET: Search
        [HttpGet]
        public async Task<IActionResult> Index(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return RedirectToAction("Index", "UserHome");
            }

            var searchModel = new SearchModel
            {
                Query = q
            };

            // query posts

            // match content
            searchModel.Posts = await context.MainPosts
                .Where(p => p.Content.Contains(q))            
                .Include(p => p.OwnerUser)
                .Include(p => p.Tags)
                .Include(p => p.Likes)
                .ToListAsync();

            // match tags
            searchModel.Posts.AddRange(await context.MainPosts
                .Where(p => p.Tags.Any(t => t.TagName.Contains(q)))
                .Include(p => p.OwnerUser)
                .Include(p => p.Tags)
                .Include(p => p.Likes)
                .ToListAsync());

            // match users
            searchModel.Posts.AddRange(await context.MainPosts
                .Where(p => p.OwnerUser.Nickname.Contains(q))
                .Include(p => p.OwnerUser)
                .Include(p => p.Tags)
                .Include(p => p.Likes)
                .ToListAsync());

            //query users
            searchModel.Users = await context.Users
                .Where(u => u.Nickname.Contains(q))
                .ToListAsync();

            //query groups

            //order posts by id
            searchModel.Posts = searchModel.Posts.OrderByDescending(p => p.PostId).ToList();

            return View(searchModel);
        }
    }
}
