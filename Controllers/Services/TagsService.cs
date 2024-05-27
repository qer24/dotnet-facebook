using dotnet_facebook.Models.Contexts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dotnet_facebook.Controllers.Services;

public class TagsService(TestContext context)
{
    public void GenerateTagsBag(dynamic viewBag)
    {
        viewBag.Tags = new SelectList(context.Tags, "TagId", "TagName");
    }
}
