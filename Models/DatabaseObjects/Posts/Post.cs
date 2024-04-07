using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dotnet_facebook.Models.DatabaseObjects.Users;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace dotnet_facebook.Models.DatabaseObjects.Posts
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [ValidateNever]
        public virtual User OwnerUser { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }

        [Required(ErrorMessage = "Please enter {0}.")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Content { get; set; }

        public string? PostFileName { get; set; }
        //public IFormFile? PostPicture { get; set; }

        public virtual ICollection<Like>? Likes { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Tag>? Tags { get; set; }
    }
}
