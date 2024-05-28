using Azure.Core;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Posts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using System.Security.Claims;

namespace dotnet_facebook.Controllers.Services;

public class PostService(TestContext context, UserService userService)
{
    public async Task Create(MainPost mainPost, ClaimsPrincipal user, ModelStateDictionary modelState, IRequestCookieCollection cookies)
    {
        mainPost.PostDate = DateTime.Now;

        var lat = double.Parse(cookies["latitude"]!, CultureInfo.InvariantCulture);
        var lon = double.Parse(cookies["longitude"]!, CultureInfo.InvariantCulture);

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

        var localUser = await userService.GetLocalUserAsync(user);
        if (localUser == null)
        {
            modelState.AddModelError("Content", "Logged in User not found!");
        }
        else
        {
            mainPost.OwnerUser = localUser;
        }

        if (modelState.IsValid)
        {
            context.Add(mainPost);
            await context.SaveChangesAsync();
        }
    }
}
