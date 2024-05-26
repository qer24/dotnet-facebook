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
using dotnet_facebook.Controllers.Services;
using System.Globalization;

namespace dotnet_facebook.Controllers
{
    public class HomeController : Controller
    {
        private readonly TestContext _context;
        private readonly UserService _userService;
        //private readonly ILogger;

        public HomeController(TestContext context, UserService userService, ILogger<HomeController> logger)
        {
            _context = context;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Nickname,Password")] User userLoginAttempt, string? returnUrl = null)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Nickname == userLoginAttempt.Nickname);
            if (!userExists)
            {
                return RedirectToAction("Login", new { error = "User not found." });
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Nickname == userLoginAttempt.Nickname);
            if (user == null || user.HashedPassword == null)
            {
                return RedirectToAction("Login", new { error = "User details are corrupted." });
            }

            if (PasswordHash.Match(userLoginAttempt.Password, user.HashedPassword) == false)
            {
                return RedirectToAction("Login", new { error = "Incorrect password." });
            }
            
            var isAdmin = _context.UserSiteRoles.Any(r => (r.User.Nickname == userLoginAttempt.Nickname & r.Role.AdministrativePerms == true));
            if (!isAdmin)
            {
                return RedirectToAction("Login", new { error = "You are not authorized to access this resource." });
            }

            List<Claim> list =
            [
                new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new(ClaimTypes.Name, user.Nickname)
            ];
            ClaimsIdentity identity = new(list, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new(identity);
            await HttpContext.SignInAsync(principal);

            // if no routing is specified, redirect to index
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserId,Nickname,Password")] User user)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Nickname == user.Nickname);
            if (userExists)
            {
                return RedirectToAction("Register", new { error = "User Name taken!" });
            }

            await _userService.Create(user, ModelState);

            return RedirectToAction("Login", new { snackBar = "Registration Successful! You can now log in." });
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

        public IActionResult Login(string? error = null, string? snackBar = null)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                ModelState.AddModelError("error", error);
            }

            if (!string.IsNullOrWhiteSpace(snackBar))
            {
                ViewBag.snackBar = snackBar;
            }

            return View();
        }

        public IActionResult Register(string? error = null)
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
}
