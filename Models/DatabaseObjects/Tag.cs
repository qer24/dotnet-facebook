using System.ComponentModel.DataAnnotations;

namespace dotnet_facebook.Models.DatabaseObjects
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }

        [Required(ErrorMessage = "Please enter {0}.")]
        [StringLength(16, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string TagName { get; set; }
    }
}
