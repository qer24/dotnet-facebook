using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Posts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace dotnet_facebook.Controllers.RegularUser;

public class UserHomeController(TestContext context, UserService userService) : Controller
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
    public async Task<IActionResult> LikePost(int postId)
    {
        var likeResult = await Like(postId);

        switch (likeResult)
        {
            case LikeResult.AlreadyLiked:
                return Json(new { success = true, alreadyLiked = true });
            case LikeResult.Success:
                return Json(new { success = true });
            default:
                return Json(new { success = false, message = "An error occurred while liking the post." });
        }
    }

    private enum LikeResult { Success, Error, AlreadyLiked }

    private async Task<LikeResult> Like(int? postId)
    {
        if (postId == null)
        {
            return LikeResult.Error;
        }

        var post = await context.MainPosts
            .Include(p => p.Likes)
            .FirstOrDefaultAsync(p => p.PostId == postId);

        if (post == null || post.Likes == null)
        {
            return LikeResult.Error;
        }

        // get the user id from the identity
        var localUser = await userService.GetLocalUserAsync(User);

        Console.WriteLine(localUser);

        if (localUser == null)
        {
            return LikeResult.Error;
        }

        var previousLike = post.Likes.FirstOrDefault(l => l.User.UserId == localUser.UserId);

        if (previousLike != null)
        {
            post.Likes.Remove(previousLike);
            await context.SaveChangesAsync();
            return LikeResult.AlreadyLiked;
        }

        post.Likes.Add(new Like
        {
            Post = post,
            User = localUser,
            LikeDate = DateTime.Now
        });
        await context.SaveChangesAsync();

        return LikeResult.Success;
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
