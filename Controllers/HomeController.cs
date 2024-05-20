using dotnet_facebook.Models;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using dotnet_facebook.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Policy;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace dotnet_facebook.Controllers
{
    public class HomeController : Controller
    {
        private readonly TestContext _context;
        //private readonly ILogger;

        public HomeController(TestContext context,ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string user, string password)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Nickname == user);
            if (!userExists)
            {
                return RedirectToAction("Login", new { error = "User not found." });
                //return BadRequest("User not found.");
            }

            var userDetails = await _context.Users.SingleOrDefaultAsync(u => u.Nickname == user);
            if (userDetails == null || userDetails.HashedPassword == null)
            {
                return RedirectToAction("Login", new { error = "User details are corrupted." });
                //return BadRequest("User details are corrupted.");
            }

            if (PasswordHash.Match(password, userDetails.HashedPassword) == false)
            {
                return RedirectToAction("Login", new { error = "Incorrect password." });
                //return BadRequest("Incorrect password.");
            }
            
            var isAdmin = _context.UserSiteRoles.Any(r => (r.User.Nickname == user & r.Role.AdministrativePerms == true));
            if (!isAdmin)
            {
                return RedirectToAction("Login", new { error = "You are not authorized to access this resource." });
                //return BadRequest("You are not authorized to access this resource.");
            }

            List<Claim> list =
            [
                new(ClaimTypes.NameIdentifier, user),
                new(ClaimTypes.Name, user)
            ];
            ClaimsIdentity identity = new(list, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new(identity);
            await HttpContext.SignInAsync(principal);

            //return Ok("Login successful.");
            return RedirectToAction("Index");
        }

        private readonly ILogger<HomeController> _logger;

  //      public HomeController(ILogger<HomeController> logger)
    //    {
      //      _logger = logger;
        //}

        public IActionResult Index()
        {
            // if not logged in, redirect to login page
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Login(string? error = null)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                ModelState.AddModelError("error", error);
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            // deauthenticate user
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
