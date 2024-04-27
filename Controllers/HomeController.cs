using dotnet_facebook.Models;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Policy;

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
                return BadRequest("User not found.");
            }

            var userDetails = await _context.Users.SingleOrDefaultAsync(u => u.Nickname == user);

            if (userDetails.Password != password)
            {
                return BadRequest("Incorrect password.");
            }
            
            var isAdmin = _context.UserSiteRoles.Any(r => (r.User.Nickname == user & r.Role.AdministrativePerms == true));
            if (!isAdmin)
            {
                return BadRequest("You are not authorized to access this resource.");
            }


            List<Claim> list = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user),
                new Claim(ClaimTypes.Name, user)
            };
            ClaimsIdentity identity = new ClaimsIdentity(list,
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme

                );
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            HttpContext.SignInAsync(principal);

            return Ok("Login successful.");

        }

        private readonly ILogger<HomeController> _logger;

  //      public HomeController(ILogger<HomeController> logger)
    //    {
      //      _logger = logger;
        //}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
