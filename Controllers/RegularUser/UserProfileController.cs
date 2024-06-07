using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using dotnet_facebook.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dotnet_facebook.Controllers.RegularUser
{
    [Route("UserProfile")]
    public class UserProfileController(TestContext context, UserService userService) : Controller
    {
        // GET: UserProfile
        [HttpGet("{id?}")]
        [OutputCache(NoStore = true, Duration = 0)]
        public async Task<IActionResult> Index(int? id)
        {
            userService.GenerateLocalUserBag(ViewBag, User);
            await userService.GenerateFriendsBagAsync(ViewBag, User);

            if (id == null)
            {
                // get the user id from the identity
                var localUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (localUserId == null)
                {
                    return RedirectToAction("UserNotFound");
                }

                var localUser = await userService.GetUserByIdAsync(int.Parse(localUserId));

                if (localUser == null)
                {
                    return RedirectToAction("UserNotFound");
                }

                return RedirectToAction("Index", new { id = localUserId });
            }

            var user = await userService.GetUserByIdAsync(id);
            var friends = await userService.GetFriendsAsync(id);
            if (user == null)
            {
                return RedirectToAction("UserNotFound");
            }

            return View(user);
        }
        [HttpGet("UserNotFound")]
        public IActionResult UserNotFound()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> UpdateBio(int UserId, string UserBio)
        {
            var user = await userService.GetUserByIdAsync(UserId);
            if (user == null)
            {
                return RedirectToAction("UserNotFound");
            }

            // if the user bio is null, set it to an empty string
            UserBio ??= "";

            user.UserProfile.UserBio = UserBio;

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost("UpdateProfilePicture")]
        [EnableCors("AllowAllOrigins")]
        public async Task<IActionResult> UpdateProfilePicture(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var user = await userService.GetLocalUserAsync(User);

                if (user == null)
                {
                    return Json(new { success = false, message = "User not found" });
                }

                var fileExtension = Path.GetExtension(file.FileName);
                // file name is the user id + the file extension
                var fileName = $"userpfp_{user.UserId}";

                var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/uploadedFiles", fileName + fileExtension);

                // first, delete any existing file with the same name (any extension)
                var existingFiles = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/uploadedFiles"), $"{fileName}.*");
                foreach (var existingFile in existingFiles)
                {
                    System.IO.File.Delete(existingFile);
                }

                using var stream = new FileStream(path, FileMode.Create);

                await file.CopyToAsync(stream);

                user.UserProfile.ProfilePictureFileName = fileName + fileExtension;

                await context.SaveChangesAsync();

                return Json(new { success = true, message = "File uploaded successfully" });
            }

            return Json(new { success = false, message = "No file uploaded" });
        }

        [HttpGet("AddFriend")]
        public async Task<IActionResult> AddFriend(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localUser = await userService.GetLocalUserAsync(User);
            var friendUser = await userService.GetUserByIdAsync(id);

            if (localUser == null || friendUser == null)
            {
                return NotFound();
            }

            // check if the friendship already exists
            var friendshipExists = await userService.AreFriendsAsync(localUser.UserId, friendUser.UserId);

            if (friendshipExists)
            {
                return RedirectToAction("Index", new { id });
            }

            var friendship = new Friendship
            {
                User1 = localUser,
                User2 = friendUser,
                FriendshipDate = DateTime.Now
            };

            context.Friendships.Add(friendship);

            await context.SaveChangesAsync();

            return RedirectToAction("Index", new { id });
        }

        [HttpGet("RemoveFriend")]
        public async Task<IActionResult> RemoveFriend(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localUser = await userService.GetLocalUserAsync(User);
            var friendUser = await userService.GetUserByIdAsync(id);

            if (localUser == null || friendUser == null)
            {
                return NotFound();
            }

            // check if the friendship already exists
            var friendship = await context.Friendships
                .FirstOrDefaultAsync(f => (f.User1.UserId == localUser.UserId && f.User2.UserId == friendUser.UserId) ||
                                    (f.User1.UserId == friendUser.UserId && f.User2.UserId == localUser.UserId));

            if (friendship == null)
            {
                return RedirectToAction("Index", new { id });
            }

            context.Friendships.Remove(friendship);

            await context.SaveChangesAsync();

            return RedirectToAction("Index", new { id });
        }
    }

}
