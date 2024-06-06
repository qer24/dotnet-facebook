using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Groups;
using dotnet_facebook.Models.DatabaseObjects.Users;
using dotnet_facebook.Models.DatabaseObjects;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;


namespace dotnet_facebook.Controllers.Services
{
    public class GroupService(TestContext context)
    {
        private readonly TestContext _context;

        public async Task<Group?> GetGroupByIdAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await context.Groups
                .SingleOrDefaultAsync(g => g.GroupId == id);
        }
    }
}
