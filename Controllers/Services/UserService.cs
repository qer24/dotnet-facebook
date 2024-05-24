using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Roles;
using dotnet_facebook.Models.DatabaseObjects.Users;
using dotnet_facebook.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace dotnet_facebook.Controllers.Services
{
    public class UserService
    {
        private readonly TestContext _context;

        public UserService(TestContext context)
        {
            _context = context;
        }

        public async Task Create(User user, ModelStateDictionary modelState)
        {
            user.UserProfile = new UserProfile()
            {
                User = user,
                UserBio = "Hey, I'm a user!"
            };

            user.AccountCreationDate = DateTime.Now;

            if (_context.Users.Any(u => u.Nickname == user.Nickname))
            {
                modelState.AddModelError("Nickname", "Nickname already exists!");
            }

            if (modelState.IsValid)
            {
                user.HashedPassword = PasswordHash.Create(user.Password);
                user.Password = "";

                _context.Add(user);
                AddDefaultRoles(_context, user);

                await _context.SaveChangesAsync();
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
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.UserId == id); ;
        }
    }
}
