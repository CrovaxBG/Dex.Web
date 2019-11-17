using System.ComponentModel.DataAnnotations;
using Dex.Common.Resources;

namespace Dex.Web.ViewModels.Identity
{
    public class LogInViewModel
    {
        [Required]
        [Display(ResourceType = typeof(IdentityResources), Name = nameof(IdentityResources.UsernameOrEmail))]
        public string UsernameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(IdentityResources), Name = nameof(IdentityResources.Password))]
        public string Password { get; set; }

        [Display(ResourceType = typeof(IdentityResources), Name = nameof(IdentityResources.StaySignedIn))]
        public bool StaySignedIn { get; set; }
    }
}