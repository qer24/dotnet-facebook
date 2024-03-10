using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_facebook.Models.DatabaseObjects.Users
{
    public class PrivateMessage
    {
        public int PrivateMessageId { get; set; }

        [ForeignKey("Friendship")]
        public string FriendshipId { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }

        [Required(ErrorMessage = "Please enter {0}.")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Message { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime MessageDate { get; set; }
    }
}
