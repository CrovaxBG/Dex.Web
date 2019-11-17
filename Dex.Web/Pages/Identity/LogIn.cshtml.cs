using System.Threading.Tasks;
using Dex.Infrastructure.Contracts.IServices;
using Dex.Web.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dex.Web.Pages.Identity
{
    [BindProperties]
    public class LogInModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILoggerService _loggerService;

        public LogInViewModel ViewModel { get; set; }

        public string ReturnUrl { get; set; }

        public LogInModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILoggerService loggerService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _loggerService = loggerService;
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

                var result = await _signInManager.PasswordSignInAsync(
                    userName, ViewModel.Password,
                    ViewModel.StaySignedIn, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    await _loggerService.Log("User logged in.");
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
