using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace dotnet_facebook.Models.DatabaseObjects.Posts
{
    public class MainPost : Post
    {
        public virtual Group? Group { get; set; }

        public int PostGeolocation { get; set; }
    }
}
