using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_facebook.Models.DatabaseObjects.Posts
{
    public class Comment : Post
    {
        public virtual Post ParentPost { get; set; }
    }
}
