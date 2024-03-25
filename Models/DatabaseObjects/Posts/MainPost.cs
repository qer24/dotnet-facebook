using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace dotnet_facebook.Models.DatabaseObjects.Posts
{
    public class MainPost : Post
    {
        public virtual Group? ParentGroup { get; set; }

        public int PostLongitude { get; set; }

        public int PostLatitude { get; set; }
    }
}
