using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
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
    public class _ResendConfirmationEmailModel : PageModel
    {
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly IEmailService _emailService;

        [Required]
        [EmailAddress]
        [BindProperty]
        public string Email { get; set; }

        public _ResendConfirmationEmailModel(UserManager<AspNetUsers> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null || user.EmailConfirmed)
            {
                this.SetRedirectMessage("Invalid email.", RedirectMessageType.Warning);
                return RedirectToPage("/Home/Index");
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.PageLink("/Identity/ConfirmEmail", values: new { code, userId = user.Id });

            await _emailService.SendEmailAsync(
                Email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            this.SetRedirectMessage("Verification email sent. Please check your email.");
            return RedirectToPage("/Home/Index");
        }
    }
}