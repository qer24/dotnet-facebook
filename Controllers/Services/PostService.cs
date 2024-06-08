using Azure.Core;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Posts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

namespace dotnet_facebook.Controllers.Services;

public class PostService(TestContext context, UserService userService)
{
    public async Task Create(Post post, ClaimsPrincipal user, ModelStateDictionary modelState, IRequestCookieCollection cookies)
    {
        post.PostDate = DateTime.Now;

        var lat = double.Parse(cookies["latitude"]!, CultureInfo.InvariantCulture);
        var lon = double.Parse(cookies["longitude"]!, CultureInfo.InvariantCulture);

        if (post is MainPost mainPost)
        {
            var geoResponse = await GeocodingService.GetCityCountryAsync(lat, lon);
            if (geoResponse != null)
            {
                var (city, country) = GeocodingService.ParseCityCountry(geoResponse);
                mainPost.PostLocation = $"{city}, {country}";
            }
            else
            {
                mainPost.PostLocation = "";
            }

            mainPost.Tags = [];
        }
        else if (post is Comment comment)
        {
            var parentPost = await context.MainPosts.FindAsync(comment.ParentPost.PostId);
            if (parentPost == null)
            {
                modelState.AddModelError("Content", "Parent Post not found!");
            }
            else
            {
                comment.ParentPost = parentPost;
            }
        }

        post.Likes = [];

        var localUser = await userService.GetLocalUserAsync(user);
        if (localUser == null)
        {
            modelState.AddModelError("Content", "Logged in User not found!");
        }
        else
        {
            post.OwnerUser = localUser;
        }

        if (modelState.IsValid)
        {
            context.Add(post);
            await context.SaveChangesAsync();
        }
    }
}
