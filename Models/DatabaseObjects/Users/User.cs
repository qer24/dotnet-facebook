using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dotnet_facebook.Models.DatabaseObjects.Groups;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace dotnet_facebook.Models.DatabaseObjects.Users
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [ValidateNever]
        public virtual UserProfile UserProfile { get; set; }
        
        [Required(ErrorMessage = "Please enter {0}.")]
        [Display(Name = "User Name")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Your {0} must be at least {2} characters long.")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Please enter {0}.")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(32)]
        [DataType(DataType.Password)]
        public string? HashedPassword { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Account created on")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime AccountCreationDate { get; set; }

        public override string ToString()
        {
            return Nickname;
        }
    }
}
