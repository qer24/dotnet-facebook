using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace dotnet_facebook.Controllers.Services;

public class TagsService(TestContext context)
{
    public void GenerateTagsBag(dynamic viewBag)
    {
        viewBag.Tags = new SelectList(context.Tags, "TagId", "TagName");
    }

    public IEnumerable<Tag> GetTagsByIds(int[] tagIds)
    {
        return context.Tags.Where(t => tagIds.Contains(t.TagId));
    }
}
