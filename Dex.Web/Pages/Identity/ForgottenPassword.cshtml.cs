using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Dex.Common.Resources;
using Dex.Common.Utils;
using Dex.DataAccess.Models;
using Dex.Infrastructure.Contracts.IServices;
using Dex.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Dex.Web.Pages.Identity
{
    public class ForgottenPasswordModel : PageModel
    {
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly IEmailService _emailService;

        [BindProperty]
        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof(IdentityResources), Name = nameof(IdentityResources.Email))]
        public string Email { get; set; }

        public ForgottenPasswordModel(UserManager<AspNetUsers> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Email);
                if (user == null || !user.EmailConfirmed)
                {
                    this.SetRedirectMessage("Invalid email.", RedirectMessageType.Warning);
                    return Page();
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = Url.PageLink("/Identity/ResetPassword", values: new { code });

                await _emailService.SendEmailAsync(
                    Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                this.SetRedirectMessage("A password reset link has been sent to your email.");
                return RedirectToPage("/Home/Index");
            }

            return Page();
        }
    }
}