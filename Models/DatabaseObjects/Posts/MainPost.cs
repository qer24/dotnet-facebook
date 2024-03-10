using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace dotnet_facebook.Models.DatabaseObjects.Posts
{
    public class MainPost : Post
    {
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public Vector2 PostGeolocation { get; set; }
    }
}
