using System.Linq;
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
        //private readonly SignInManager<AspNetUsers> _signInManager;

        //public IndexModel(SignInManager<AspNetUsers> signInManager)
        //{
        //    _signInManager = signInManager;
        //}

        //public async Task<IActionResult> OnGet()
        //{
        //    //await _signInManager.PasswordSignInAsync("CrovaxBG", "1", true, false);
        //    return Page();
        //}
    }
}
