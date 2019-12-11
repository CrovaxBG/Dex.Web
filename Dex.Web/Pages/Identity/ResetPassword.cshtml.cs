using System.Text;
using System.Threading.Tasks;
using Dex.Common.Utils;
using Dex.DataAccess.Models;
using Dex.Web.Helpers;
using Dex.Web.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Dex.Web.Pages.Identity
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<AspNetUsers> _userManager;

        [BindProperty]
        public ResetPasswordViewModel ViewModel { get; set; }

        public ResetPasswordModel(UserManager<AspNetUsers> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult OnGet(string code)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }

            ViewModel = new ResetPasswordViewModel {Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))};
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(ViewModel.Email);
            if (user == null)
            {
                this.SetRedirectMessage("Invalid email.", RedirectMessageType.Warning);
                return Page();
            }

            var result = await _userManager.ResetPasswordAsync(user, ViewModel.Code, ViewModel.Password);
            if (result.Succeeded)
            {
                this.SetRedirectMessage("Password successfully changed.");
                return RedirectToPage("/Home/Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}