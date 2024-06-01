using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_facebook.Models.DatabaseObjects.Users
{
    public class PrivateMessage
    {
        [Key]
        public int PrivateMessageId { get; set; }

        [ValidateNever] public virtual User Sender { get; set; }
        [ValidateNever] public virtual User Receiver { get; set; }

        [Required(ErrorMessage = "Please enter {0}.")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Message { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime MessageDate { get; set; }
    }
}
