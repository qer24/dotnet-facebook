using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace dotnet_facebook.Controllers.RegularUser;

public class UserHomeController(TestContext context) : Controller
{
    private static int _currentPostCount = 0;

    public async Task<IActionResult> Index(List<MainPost> postsToView)
    {
        if (postsToView.Count == 0)
        {
            _currentPostCount = 0;
            return await LoadMorePosts();
        }

        return View(postsToView);
    }

    public async Task<IActionResult> LoadMorePosts()
    {
        _currentPostCount += 5;

        var posts = await context.MainPosts
            .OrderByDescending(p => p.PostId)
            .Take(_currentPostCount)
            .Include(p => p.OwnerUser)
            .Include(p => p.Likes)
            .ToListAsync();

        _currentPostCount = posts.Count;

        return View("Index", posts);
    }

    [HttpPost]
    public ActionResult SaveLocation([FromBody] LocationModel location)
    {
        if (location != null)
        {
            // save to cookies
            if (Request.Cookies.ContainsKey("latitude"))
            {
                Response.Cookies.Delete("latitude");
            }
            if (Request.Cookies.ContainsKey("longitude"))
            {
                Response.Cookies.Delete("longitude");
            }

            var coookieSettings = new CookieOptions
            {
                Expires = DateTime.Now.AddYears(1),
                SameSite = SameSiteMode.Lax,
            };

            Response.Cookies.Append("latitude", location.Latitude.ToString(CultureInfo.InvariantCulture), coookieSettings);
            Response.Cookies.Append("longitude", location.Longitude.ToString(CultureInfo.InvariantCulture), coookieSettings);

            // Return a success response
            return Json(new { success = true });
        }

        // Return an error response if location is null
        return Json(new { success = false, message = "Invalid location data." });
    }

    public class LocationModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
