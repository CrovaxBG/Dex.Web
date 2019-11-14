using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Dex.Web.Pages.Identity
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ConfirmEmailModel> _logger;

        public string StatusMessage { get; set; }

        public ConfirmEmailModel(UserManager<IdentityUser> userManager, ILogger<ConfirmEmailModel> logger)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet(string code, string userId)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Home/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogInformation($"Unable to load user with ID '{userId}'.");
                return NotFound($"Unable to load user with requested ID.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

            return Page();
        }
    }
}
