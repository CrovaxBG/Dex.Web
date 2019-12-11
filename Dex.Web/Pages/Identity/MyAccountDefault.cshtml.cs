using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dex.DataAccess.Models;
using Dex.Web.Helpers;
using Dex.Web.ViewModels.Downloads;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dex.Web.Pages.Identity
{
    public class MyAccountDefaultModel : PageModel
    {
        private readonly SignInManager<AspNetUsers> _signInManager;
        private readonly UserManager<AspNetUsers> _userManager;

        [BindProperty]
        public AccountInformationViewModel AccountInformationViewModel { get; set; }

        public MyAccountDefaultModel(SignInManager<AspNetUsers> signInManager, UserManager<AspNetUsers> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            AccountInformationViewModel = new AccountInformationViewModel
            {
                Email = user.Email,
                Username = user.UserName
            };
        }

        public async Task<IActionResult> OnPostLogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Home/Index");
        }
    }
}