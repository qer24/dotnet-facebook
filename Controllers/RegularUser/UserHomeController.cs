using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Posts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace dotnet_facebook.Controllers.RegularUser;

[Authorize]
public class UserHomeController(TestContext context, UserService userService, PostService postService, TagsService tagsService) : Controller
{
    // GET: UserHome/Error
    [HttpGet("UserHome/{route?}")]
    public async Task<IActionResult> Index(string? route = null)
    {
        // if error is a name of any action, show just the index
        if (route != null && route == nameof(LoadPosts) && !route.Equals("Index", StringComparison.CurrentCultureIgnoreCase))
        {
            route = null;
        }

        if (int.TryParse(route, out var tagFilter))
        {
            return await Index([], null, tagFilter);
        }

        return await Index([], route);
    }

    // GET: UserHome
    public async Task<IActionResult> Index(List<MainPost> postsToView, string? error = null, int? tagFilter = null)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Home");
        }

        tagsService.GenerateTagsBag(ViewBag);
        userService.GenerateLocalUserBag(ViewBag, User);

        if (postsToView.Count == 0)
        {
            return await LoadPosts(error, tagFilter);
        }

        if (error != null)
        {
            ModelState.AddModelError("Content", error);
        }

        return View(postsToView);
    }

    public async Task<IActionResult> LoadPosts(string? error = null, int? tagFilter = null)
    {
        tagsService.GenerateTagsBag(ViewBag);
        userService.GenerateLocalUserBag(ViewBag, User);

        // tagFilter = -2 - all posts
        // tagFilter = -1 - all posts made by friends or user
        // tagFilter >= 0 - all posts with the tag id
        List<MainPost> posts = [];
        if (tagFilter == null || tagFilter == -2)
        {
            posts = await context.MainPosts
                .Where(p => p.ParentGroup == null)
                .OrderByDescending(p => p.PostId)
                .Include(p => p.OwnerUser)
                .ThenInclude(u => u.UserProfile)
                .Include(p => p.Likes)
                .Include(p => p.Tags)
                .ToListAsync();
        }
        else if (tagFilter == -1)
        {
            var localUser = await userService.GetLocalUserAsync(User);

            if (localUser == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var userFriends = await userService.GetFriendsAsync(localUser.UserId);
            var friendIds = userFriends.Select(f => f.UserId).ToList();

            posts = await context.MainPosts
                .Where(p => p.ParentGroup == null)
                .OrderByDescending(p => p.PostId)
                .Where(p => p.OwnerUser.UserId == localUser.UserId || friendIds.Contains(p.OwnerUser.UserId))
                .Include(p => p.OwnerUser)
                .ThenInclude(u => u.UserProfile)
                .Include(p => p.Likes)
                .Include(p => p.Tags)
                .ToListAsync();
        }
        else
        {
            posts = await context.MainPosts
                .Where(p => p.ParentGroup == null)
                .Where(p => p.Tags.Any(t => t.TagId == tagFilter))
                .OrderByDescending(p => p.PostId)
                .Include(p => p.OwnerUser)
                .ThenInclude(u => u.UserProfile)
                .Include(p => p.Likes)
                .Include(p => p.Tags)
                .ToListAsync();
        }

        if (error != null)
        {
            ModelState.AddModelError("Content", error);
        }

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

        Post? post = await context.MainPosts
            .Include(p => p.Likes)
            .ThenInclude(l => l.User)
            .FirstOrDefaultAsync(p => p.PostId == postId);

        if (post == null || post.Likes == null)
        {
            post = await context.Comments
                .Include(p => p.Likes)
                .ThenInclude(l => l.User)
                .FirstOrDefaultAsync(p => p.PostId == postId);
        }

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
    public async Task<IActionResult> NewPost([Bind("PostId,Content")] MainPost mainPost, List<string> selectedTagIds)
    {
        var selectedTagsIdsInt = selectedTagIds.Select(int.Parse).ToArray();
        var tags = tagsService.GetTagsByIds(selectedTagsIdsInt);

        mainPost.Tags = tags.ToArray();

        await postService.Create(mainPost, User, ModelState, Request.Cookies);

        if (ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index));
        }
        return RedirectToAction("Index", new { route = ModelState.IsValid ? null : ModelState.Values.First().Errors.First().ErrorMessage });
    }

    // GET: UserHome/ViewPost/5
    [HttpGet("UserHome/ViewPost/{postId}.{error?}")]
    public async Task<IActionResult> ViewPost([FromRoute] int postId, string? error = null)
    {
        var post = await context.MainPosts
            .Include(p => p.OwnerUser)
            .ThenInclude(u => u.UserProfile)
            .Include(p => p.Likes)
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(p => p.PostId == postId);

        if (post == null)
        {
            return NotFound();
        }

        post.Comments = await context.Comments
            .Include(c => c.OwnerUser)
            .ThenInclude(u => u.UserProfile)
            .Include(c => c.Likes)
            .Where(c => c.ParentPost.PostId == postId)
            .OrderByDescending(c => c.PostId)
            .ToListAsync();

        tagsService.GenerateTagsBag(ViewBag);
        userService.GenerateLocalUserBag(ViewBag, User);

        if (error != null)
        {
            ModelState.AddModelError("Content", error);
        }

        return View(post);
    }

    [HttpPost]
    public async Task<IActionResult> Reply([Bind("PostId,Content")] Comment comment, int? parentPostId)
    {
        if (parentPostId == null)
        {
            return BadRequest();
        }

        var mainPost = await context.MainPosts.FirstOrDefaultAsync(p => p.PostId == parentPostId);

        if (mainPost == null)
        {
            return NotFound();
        }

        comment.ParentPost = mainPost;

        // get new post id for the comment because it tries to bind the post id from the parent post
        //comment.PostId = postService.GetNewPostId();

        await postService.Create(comment, User, ModelState, Request.Cookies);

        if (ModelState.IsValid)
        {
            return RedirectToAction("ViewPost", new { postId = comment.ParentPost.PostId });
        }
        return RedirectToAction("ViewPost", new { postId = comment.ParentPost.PostId, error = ModelState.IsValid ? null : ModelState.Values.First().Errors.First().ErrorMessage });
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
