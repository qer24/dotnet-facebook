using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_facebook.Controllers.RegularUser;

public class MessagesController(TestContext context, UserService userService) : Controller
{
    // GET: Messages
    [HttpGet]
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

    // GET: Messages/5
    [Route("Messages/{id}")]
    [HttpGet]
    public async Task<IActionResult> Messages(int id)
    {
        var localUser = await userService.GetLocalUserAsync(User);
        if (localUser == null)
        {
            return RedirectToAction("Login", "Home");
        }

        var isFriends = await userService.AreFriendsAsync(localUser.UserId, id);
        if (!isFriends)
        {
            return RedirectToAction("Index");
        }

        var otherUser = await userService.GetUserByIdAsync(id);

        return View(otherUser);
    }
}
