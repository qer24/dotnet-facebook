using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Groups;
using dotnet_facebook.Models.DatabaseObjects.Users;
using dotnet_facebook.Models.DatabaseObjects;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;


namespace dotnet_facebook.Controllers.Services
{
    public class GroupService(TestContext context)
    {

        public async Task<Group?> GetGroupByIdAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await context.Groups
                .Include(g => g.Tags)
                .Include(g => g.Users)
                .ThenInclude(gu => gu.User)
                .SingleOrDefaultAsync(g => g.GroupId == id);
        }

    }

}
