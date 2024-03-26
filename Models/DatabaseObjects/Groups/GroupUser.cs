using dotnet_facebook.Models.DatabaseObjects.Users;
using System.ComponentModel.DataAnnotations;

namespace dotnet_facebook.Models.DatabaseObjects.Groups
{
    public class GroupUser
    {
        [Key]
        public int GroupUserID { get; set; }

        public GroupRole GroupRole { get; set; }

        public virtual User User { get; set; }
        public virtual Group Group { get; set; }
    }
}
