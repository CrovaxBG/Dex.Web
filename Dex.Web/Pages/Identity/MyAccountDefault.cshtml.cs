using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dex.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dex.Web.Pages.Identity
{
    public class MyAccountDefaultModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public AccountInformationViewModel AccountInformationViewModel { get; set; }

        public MyAccountDefaultModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
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

    public class AccountInformationViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
}