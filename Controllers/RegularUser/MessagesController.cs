using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_facebook.Controllers.RegularUser;

public class MessagesController(TestContext context, UserService userService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var localUser = await userService.GetLocalUserAsync(User);

        if (localUser == null)
        {
            return RedirectToAction("Login", "Home");
        }

        var friends = await userService.GetFriendsAsync(localUser.UserId);

        return View(friends);
    }
}
