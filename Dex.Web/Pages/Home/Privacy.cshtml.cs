using System.Security.Claims;
using System.Threading.Tasks;
using Dex.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dex.Web.Pages.Home
{
    public class PrivacyModel : PageModel
    {
        private readonly SignInManager<AspNetUsers> _signInManager;

        public PrivacyModel(SignInManager<AspNetUsers> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            await _signInManager.UserManager.AddClaimAsync(await _signInManager.UserManager.GetUserAsync(User),
                new Claim($"RemoveRecord", "UserPrivilege"));
            return Page();
        }
    }
}
