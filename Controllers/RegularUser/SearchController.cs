using dotnet_facebook.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_facebook.Controllers.RegularUser
{
    public class SearchController : Controller
    {
        // GET: Search
        [HttpGet]
        public IActionResult Index(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return RedirectToAction("Index", "UserHome");
            }

            var searchModel = new SearchModel
            {
                Query = q
            };

            return View(searchModel);
        }
    }
}
