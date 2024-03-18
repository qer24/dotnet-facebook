using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_facebook.Models.DatabaseObjects.Users
{
    public class Friendship
    {
        [Key]
        public int FriendshipId { get; set; }

        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FriendshipDate { get; set; }

        public virtual ICollection<PrivateMessage>? Messages { get; set; }
    }
}
