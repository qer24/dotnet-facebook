using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dotnet_facebook.Models.DatabaseObjects.Posts;

namespace dotnet_facebook.Models.DatabaseObjects.Users
{
    public class UserProfile
    {
        [Key]
        public int UserProfileId { get; set; }

        public int UserID { get; set; }
        public virtual User User { get; set; }

        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string UserBio { get; set; }

        public string? ProfilePictureFileName { get; set; }
        //public IFormFile? ProfilePicture { get; set; }

        public virtual ICollection<Post>? UserPosts { get; set; }
    }
}
