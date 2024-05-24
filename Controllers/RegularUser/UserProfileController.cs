using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_facebook.Controllers.RegularUser
{

    public class UserProfileController : Controller
    {
        private readonly TestContext _context;
        private readonly UserService _userService;

        public async Task<IActionResult> Index(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}
