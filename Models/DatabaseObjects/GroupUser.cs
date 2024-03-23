using dotnet_facebook.Models.DatabaseObjects.Users;
using System.ComponentModel.DataAnnotations;

namespace dotnet_facebook.Models.DatabaseObjects
{
    public class GroupUser
    {
        [Key]
        public int GroupUserID { get; set; }
        public bool IsModerator { get; set; }
        public virtual User Users { get; set; }
        public virtual Group Groups { get; set; }

    }
}
