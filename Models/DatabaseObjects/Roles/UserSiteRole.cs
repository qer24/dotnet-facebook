using dotnet_facebook.Models.DatabaseObjects.Users;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace dotnet_facebook.Models.DatabaseObjects.Roles
{
    public class UserSiteRole
    {
        [Key]
        public int UserSiteRoleId { get; set; }

        [ValidateNever] public virtual User User { get; set; }
        [ValidateNever] public virtual SiteRole Role { get; set; }
    }
}
