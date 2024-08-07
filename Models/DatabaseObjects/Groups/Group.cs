﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dotnet_facebook.Models.DatabaseObjects.Posts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace dotnet_facebook.Models.DatabaseObjects.Groups
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Please enter {0}.")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string GroupName { get; set; }

        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string GroupDescription { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime GroupCreationDate { get; set; }

        public string? GroupPictureFileName { get; set; }
        //public IFormFile? GroupPicture { get; set; }

        public virtual ICollection<Post>? GroupPosts { get; set; }
        [ValidateNever] public virtual ICollection<GroupUser> Users { get; set; }
        [ValidateNever] public virtual ICollection<Tag>? Tags { get; set; }
    }
}
