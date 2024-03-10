using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dotnet_facebook.Models.DatabaseObjects.Users;

namespace dotnet_facebook.Models.DatabaseObjects.Posts
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [ForeignKey("OwnerUser")]
        public int OwnerUserId { get; set; }
        public User OwnerUser { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }

        [Required(ErrorMessage = "Please enter {0}.")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Content { get; set; }

        [Required(ErrorMessage = "Please enter file name")]
        public string PostFileName { get; set; }
        [Required(ErrorMessage = "Please select file")]
        public IFormFile PostPicture { get; set; }

        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
