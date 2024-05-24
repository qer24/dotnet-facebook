using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_facebook.Controllers.RegularUser
{
    public class UserProfileController(TestContext context, UserService userService) : Controller
    {
        // GET: UserProfile
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userService.GetUserByIdAsync(1);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}
