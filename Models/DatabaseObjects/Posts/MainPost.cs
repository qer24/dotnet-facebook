using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using dotnet_facebook.Models.DatabaseObjects.Groups;

namespace dotnet_facebook.Models.DatabaseObjects.Posts
{
    public class MainPost : Post
    {
        public virtual Group? ParentGroup { get; set; }

        public int PostLongitude { get; set; }
        public int PostLatitude { get; set; }

        // 23.1234567 => 23123456
        // 23123456 => 23.1234567

        public static int FromDouble(double value)
        {
            return (int)(value * 1000000);
        }

        public static double ToDouble(int value)
        {
            return value / 1000000.0;
        }
    }
}
