using dotnet_facebook.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Policy;

namespace dotnet_facebook.Controllers
{
    public class HomeController : Controller
    {

        [HttpPost]
        public void Login(string user, string password)
        {
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
        }
        public void onPost()
        {
            


        }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

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
