using dotnet_facebook.Models.Contexts;
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

        public async Task GenerateFriendsBagAsync(dynamic viewBag, ClaimsPrincipal user)
        {
            var localUser = GetLocalUserAsync(user).Result;
            if (localUser == null)
            {
                return;
            }

            var friendShips = await GetFriendshipsAsync(localUser.UserId);

            viewBag.Friends = friendShips;
        }

        public async Task<List<Friendship>> GetFriendshipsAsync(int? id)
        {
            return await context.Friendships
                .Where(f => f.User1Id == id || f.User2Id == id)
                .Include(f => f.User1)
                .ThenInclude(u => u.UserProfile)
                .Include(f => f.User2)
                .ThenInclude(u => u.UserProfile)
                .ToListAsync();
        }

        public async Task<Friendship?> GetFriendshipAsync(int id1, int id2)
        {
            var friendships = await GetFriendshipsAsync(id1);
            return friendships.SingleOrDefault(f => f.User1Id == id2 || f.User2Id == id2);
        }

        public async Task<List<User>> GetFriendsAsync(int? id)
        {
            if (id == null)
            {
                return [];
            }

            var friendRelations = await GetFriendshipsAsync(id);

            var friends = new List<User>();
            foreach (var relation in friendRelations)
            {
                var friend = relation.User1.UserId == id ? relation.User2 : relation.User1;
                friends.Add(friend);
            }

            return friends;
        }

        public async Task<bool> AreFriendsAsync(int id1, int id2)
        {
            return await context.Friendships
                .AnyAsync(f => (f.User1Id == id1 && f.User2Id == id2) || (f.User1Id == id2 && f.User2Id == id1));
        }
    }
}
