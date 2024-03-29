﻿using System.ComponentModel.DataAnnotations;
using Dex.Common.Resources;

namespace Dex.Web.ViewModels.Identity
{
    public class SignUpViewModel
    {
        [Required]
        [Display(ResourceType = typeof(IdentityResources), Name = nameof(IdentityResources.Username))]
        [StringLength(maximumLength: 256)]
        public string Username { get; set; }

        [EmailAddress]
        [Required]
        [Display(ResourceType = typeof(IdentityResources), Name = nameof(IdentityResources.Email))]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(IdentityResources), Name = nameof(IdentityResources.Password))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        [Display(ResourceType = typeof(IdentityResources), Name = nameof(IdentityResources.ConfirmPassword))]
        public string ConfirmPassword { get; set; }
    }
}
