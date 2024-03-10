using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_facebook.Models.DatabaseObjects.Users
{
    public class Friendship
    {
        [Key]
        public int FriendshipId { get; set; }

        [ForeignKey("User1")]
        public int User1Id { get; set; }
        public User User1 { get; set; }

        [ForeignKey("User2")]
        public int User2Id { get; set; }
        public User User2 { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FriendshipDate { get; set; }

        public ICollection<PrivateMessage> Messages { get; set; }
    }
}
