using System.Threading.Tasks;
using Dex.DataAccess.Models;
using Dex.Infrastructure.Contracts.IServices;
using Dex.Web.Helpers;
using Dex.Web.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dex.Web.Pages.Identity
{
    [BindProperties]
    public class LogInModel : PageModel
    {
        private readonly SignInManager<AspNetUsers> _signInManager;
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly ILoggerService _loggerService;

        public LogInViewModel ViewModel { get; set; }

        public string ReturnUrl { get; set; }

        public LogInModel(
            SignInManager<AspNetUsers> signInManager,
            UserManager<AspNetUsers> userManager,
            ILoggerService loggerService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _loggerService = loggerService;
        }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Home/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(ViewModel.UsernameOrEmail);
                var userName = ViewModel.UsernameOrEmail;
                if (user != null)
                {
                    userName = user.UserName;
                }

                var result = await _signInManager.PasswordSignInAsync(userName, ViewModel.Password,ViewModel.StaySignedIn, true);

                if (result.Succeeded)
                {
                    await _loggerService.Log("User logged in.");
                    this.SetRedirectMessage($"Welcome {userName}");
                    return LocalRedirect(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    await _loggerService.Log("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
