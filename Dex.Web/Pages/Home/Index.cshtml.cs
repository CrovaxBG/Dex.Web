using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dex.Common.DTO;
using Dex.DataAccess.Models;
using Dex.Infrastructure.Contracts.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dex.Web.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly SignInManager<AspNetUsers> _signInManager;

        public IndexModel(SignInManager<AspNetUsers> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            await _signInManager.PasswordSignInAsync("CrovaxBG", "1", true, false);
            //for (int i = 0; i < 20; i++)
            //{
            //    await _signInManager.UserManager.AddClaimAsync(await _signInManager.UserManager.GetUserAsync(User),
            //        new Claim($"q{i}", i.ToString()));
            //}
            return Page();
        }
    }
}
