using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using dotnet_facebook.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dotnet_facebook.Controllers.RegularUser
{
    [Route("UserProfile")]
    public class UserProfileController(TestContext context, UserService userService) : Controller
    {
        // GET: UserProfile
        [HttpGet("{id?}")]
        public async Task<IActionResult> Index(int? id)
        {
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
            var friends = await userService.GetUserFriendsAsync(id);
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

                var path = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileName + fileExtension);

                // first, delete any existing file with the same name (any extension)
                var existingFiles = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles"), $"{fileName}.*");
                foreach (var existingFile in existingFiles)
                {
                    System.IO.File.Delete(existingFile);
                }

                using var stream = new FileStream(path, FileMode.Create);

                await file.CopyToAsync(stream);

                return Json(new { success = true, message = "File uploaded successfully" });
            }

            return Json(new { success = false, message = "No file uploaded" });
        }
    }

}
