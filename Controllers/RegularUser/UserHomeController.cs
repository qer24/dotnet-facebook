using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace dotnet_facebook.Controllers.RegularUser;

public class UserHomeController(TestContext context) : Controller
{
    public IActionResult Index()
    {
        return View();
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
