using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> Messages(int id, string? error)
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

        var messages = await context.PrivateMessages
            .Where(m => (m.Sender == localUser && m.Receiver == otherUser) || (m.Sender == otherUser && m.Receiver == localUser))
            .OrderBy(m => m.MessageDate)
            .ToListAsync();

        ViewBag.Messages = messages;
        userService.GenerateLocalUserBag(ViewBag, User);

        if (error != null)
        {
            ModelState.AddModelError("Message", error);
        }

        return View(otherUser);
    }

    public async Task<IActionResult> SendMessage([Bind("PrivateMessageId,Message")] PrivateMessage privateMessage, int id)
    {
        var localUser = await userService.GetLocalUserAsync(User);
        if (localUser == null)
        {
            return RedirectToAction("Login", "Home");
        }

        var otherUser = await userService.GetUserByIdAsync(id);
        if (otherUser == null)
        {
            return RedirectToAction("Index");
        }

        var isFriends = await userService.AreFriendsAsync(localUser.UserId, id);
        if (!isFriends)
        {
            return RedirectToAction("Index");
        }

        privateMessage.Sender = localUser;
        privateMessage.Receiver = otherUser;

        privateMessage.MessageDate = DateTime.Now;

        if (ModelState.IsValid)
        {
            context.PrivateMessages.Add(privateMessage);
            await context.SaveChangesAsync();
        }

        // error will be in the second model state value
        var error = ModelState.Values.ElementAtOrDefault(1)?.Errors.FirstOrDefault()?.ErrorMessage;

        return RedirectToAction("Messages", new { id, error });
    }
}
