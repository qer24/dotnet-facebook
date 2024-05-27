﻿using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dotnet_facebook.Controllers.RegularUser
{
    [Route("UserProfile")]
    public class UserProfileController(TestContext context, UserService userService) : Controller
    {
        // GET: UserProfile
        [HttpGet("{id?}")]
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                // get the user id from the identity
                var localUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (localUserId == null)
                {
                    return RedirectToAction("UserNotFound");
                }

                var localUser = await userService.GetUserByIdAsync(int.Parse(localUserId));

                if (localUser == null)
                {
                    return RedirectToAction("UserNotFound");
                }

                return RedirectToAction("Index", new { id = localUserId });
            }

            var user = await userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("UserNotFound");
            }

            return View(user);
        }
        [HttpGet("UserNotFound")]
        public IActionResult UserNotFound()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> UpdateBio(int UserId, string UserBio)
        {
            var user = await userService.GetUserByIdAsync(UserId);
            if (user == null)
            {
                return RedirectToAction("UserNotFound");
            }

            user.UserProfile.UserBio = UserBio;

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }

}
