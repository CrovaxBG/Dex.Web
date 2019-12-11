using System.Text;
using System.Threading.Tasks;
using Dex.DataAccess.Models;
using Dex.Infrastructure.Contracts.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Dex.Web.Pages.Identity
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly ILoggerService _logger;

        public string StatusMessage { get; set; }

        public ConfirmEmailModel(UserManager<AspNetUsers> userManager, ILoggerService logger)
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
                await _logger.Log($@"Unable to load user with ID '{userId}'.");
                return NotFound("Unable to load user with requested ID.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

            return Page();
        }
    }
}
