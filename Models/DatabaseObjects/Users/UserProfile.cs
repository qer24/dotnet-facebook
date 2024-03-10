using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dotnet_facebook.Models.DatabaseObjects.Posts;

namespace dotnet_facebook.Models.DatabaseObjects.Users
{
    public class UserProfile
    {
        [Key]
        public int UserProfileId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string UserBio { get; set; }

        [Required(ErrorMessage = "Please enter file name")]
        public string ProfilePictureFileName { get; set; }
        [Required(ErrorMessage = "Please select file")]
        public IFormFile ProfilePicture { get; set; }

        public ICollection<Post> UserPosts { get; set; }
    }
}
