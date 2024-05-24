using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Users;
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
                    return NotFound();
                }

                var localUser = await userService.GetUserByIdAsync(int.Parse(localUserId));

                if (localUser == null)
                {
                    return NotFound();
                }
                
                return View(localUser);
            }

            var user = await userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}
