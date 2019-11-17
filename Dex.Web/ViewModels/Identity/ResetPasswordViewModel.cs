using System.ComponentModel.DataAnnotations;
using Dex.Common.Resources;

namespace Dex.Web.ViewModels.Identity
{
    public class ResetPasswordViewModel
    {
        [EmailAddress]
        [Required]
        [Display(ResourceType = typeof(IdentityResources), Name = nameof(IdentityResources.Email))]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(IdentityResources), Name = nameof(IdentityResources.NewPassword))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        [Display(ResourceType = typeof(IdentityResources), Name = nameof(IdentityResources.ConfirmNewPassword))]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}