using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dotnet_facebook.Models.DatabaseObjects.Posts;
using dotnet_facebook.Models.DatabaseObjects.Users;

namespace dotnet_facebook.Models.DatabaseObjects
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [ForeignKey("OwnerUser")]
        public int OwnerUserId { get; set; }
        public User OwnerUser { get; set; }

        [Required(ErrorMessage = "Please enter {0}.")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string GroupName { get; set; }

        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string GroupDescription { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime GroupCreationDate { get; set; }

        [Required(ErrorMessage = "Please enter file name")]
        public string GroupPictureFileName { get; set; }
        [Required(ErrorMessage = "Please select file")]
        public IFormFile GroupPicture { get; set; }

        public ICollection<Post> GroupPosts { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<User> Moderators { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
