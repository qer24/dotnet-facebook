using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_facebook.Controllers.RegularUser
{
    [Route("UserProfile")]
    public class UserProfileController(TestContext context, UserService userService) : Controller
    {
        // GET: UserProfile
        [HttpGet("{userId}")]
        public async Task<IActionResult> Index(int? userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var user = await userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}
