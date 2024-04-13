using dotnet_facebook.Models.DatabaseObjects.Users;

namespace dotnet_facebook.Models.DatabaseObjects.Roles
{
    public class UserSiteRole
    {
        public int UserSireRoleId { get; set; }
        public virtual User User { get; set; }

        public virtual SiteRole Role { get; set; }
    }
}
