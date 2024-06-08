using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.DatabaseObjects.Groups;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace dotnet_facebook.Models.DatabaseObjects.Posts
{
    public class MainPost : Post
    {
        public virtual Group? ParentGroup { get; set; }
        public string? PostLocation { get; set; }

        [ValidateNever]
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
    }
}
