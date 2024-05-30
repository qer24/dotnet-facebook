using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_facebook.Models.DatabaseObjects.Posts
{
    public class Comment : Post
    {
        [ValidateNever]
        public virtual Post ParentPost { get; set; }
    }
}
