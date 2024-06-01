using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_facebook.Models.DatabaseObjects.Users
{
    public class Friendship
    {
        [Key]
        public int FriendshipId { get; set; }

        public int User1Id { get; set; }  // Foreign key property for User1
        public int User2Id { get; set; }  // Foreign key property for User2

        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FriendshipDate { get; set; }

        public virtual ICollection<PrivateMessage>? Messages { get; set; }
    }
}
