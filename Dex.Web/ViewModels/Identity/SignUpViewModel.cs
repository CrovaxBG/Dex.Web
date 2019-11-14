using System.ComponentModel.DataAnnotations;

namespace Dex.Web.ViewModels.Identity
{
    public class SignUpViewModel
    {
        [EmailAddress]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
