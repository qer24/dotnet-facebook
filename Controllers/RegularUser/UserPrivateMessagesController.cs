using Microsoft.AspNetCore.Mvc;

namespace dotnet_facebook.Controllers.RegularUser
{
    public class UserPrivateMessagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
