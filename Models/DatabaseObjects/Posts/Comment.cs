using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_facebook.Models.DatabaseObjects.Posts
{
    public class Comment : Post
    {
        [ForeignKey("ParentPost")]
        public int ParentPostId { get; set; }
        public Post ParentPost { get; set; }
    }
}
