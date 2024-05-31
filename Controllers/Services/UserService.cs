﻿using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Roles;
using dotnet_facebook.Models.DatabaseObjects.Users;
using dotnet_facebook.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dotnet_facebook.Controllers.Services
{
    public class UserService(TestContext context)
    {
        public async Task Create(User user, ModelStateDictionary modelState)
        {
            user.UserProfile = new UserProfile()
            {
                User = user,
                UserBio = "Hey, I'm a user!"
            };

            user.AccountCreationDate = DateTime.Now;

            if (context.Users.Any(u => u.Nickname == user.Nickname))
            {
                modelState.AddModelError("Nickname", "Nickname already exists!");
            }

            if (modelState.IsValid)
            {
                user.HashedPassword = PasswordHash.Create(user.Password);
                user.Password = "";

                context.Add(user);
                AddDefaultRoles(context, user);

                await context.SaveChangesAsync();
            }
        }

        public static void AddDefaultRoles(TestContext context, User user)
        {
            // Set default roles for the user
            var defaultRoles = context.SiteRoles.Where(r => r.IsDefault).ToList();
            foreach (var role in defaultRoles)
            {
                context.UserSiteRoles.Add(new UserSiteRole()
                {
                    User = user,
                    Role = role
                });
            }
        }

        public async Task<User?> GetUserByIdAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await context.Users
                .Include(u => u.UserProfile)
                .SingleOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User?> GetLocalUserAsync(ClaimsPrincipal user)
        {
            var userClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return null;
            }

            var localUserId = int.Parse(userClaim);
            var localUser = await GetUserByIdAsync(localUserId);

            return localUser;
        }

        public void GenerateLocalUserBag(dynamic viewBag, ClaimsPrincipal user)
        {
            viewBag.LocalUser = GetLocalUserAsync(user).Result;
        }
        public async Task<List<User>> GetUserFriendsAsync(int? id)
        {
            // Znajdź użytkownika w bazie danych
            var user = await context.Users
                .Include(u => u.Friendships)
                .ThenInclude(f => f.User1)
                .Include(u => u.Friendships)
                .ThenInclude(f => f.User2)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return null;
            }

            var friendRelations = user.Friendships;

            var friends = new List<User>();
            foreach (var relation in friendRelations)
            {
                var friend = relation.User1.UserId == id ? relation.User2 : relation.User1;
                friends.Add(friend);
            }

            return friends;
        }
    }
}
