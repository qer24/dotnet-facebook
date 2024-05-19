using Microsoft.AspNetCore.Mvc;

namespace dotnet_facebook.Controllers.RegularUser
{
    public class UserProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
