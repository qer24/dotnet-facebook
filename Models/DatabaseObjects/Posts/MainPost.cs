using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.DatabaseObjects.Groups;

namespace dotnet_facebook.Models.DatabaseObjects.Posts
{
    public class MainPost : Post
    {
        public virtual Group? ParentGroup { get; set; }
        public string? PostLocation { get; set; }

        public virtual ICollection<Tag>? Tags { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
    }
}
