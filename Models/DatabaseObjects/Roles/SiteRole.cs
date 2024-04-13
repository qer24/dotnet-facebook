using System.ComponentModel.DataAnnotations;

namespace dotnet_facebook.Models.DatabaseObjects.Roles
{
    public class SiteRole
    {
        [Key]
        public int SiteRoleId { get; set; }

        public string SiteRoleName { get; set; }
    }
}
