using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dotnet_facebook.Models.DatabaseObjects.Users;

namespace dotnet_facebook.Models.DatabaseObjects.Posts
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }

        public virtual User User { get; set; }
        public virtual Post Post { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LikeDate { get; set; }
    }
}
