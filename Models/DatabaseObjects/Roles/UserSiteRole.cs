using dotnet_facebook.Models.DatabaseObjects.Users;
using System.ComponentModel.DataAnnotations;

namespace dotnet_facebook.Models.DatabaseObjects.Roles
{
    public class UserSiteRole
    {
        [Key]
        public int UserSiteRoleId { get; set; }

        public virtual User User { get; set; }
        public virtual SiteRole Role { get; set; }
    }
}
