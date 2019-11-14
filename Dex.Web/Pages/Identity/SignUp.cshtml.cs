using System.Text;
using System.Threading.Tasks;
using Dex.Infrastructure.Contracts.IServices;
using Dex.Web.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Dex.Web.Pages.Identity
{
    [BindProperties]
    public class SignUpModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<SignUpModel> _logger;
        private readonly IEmailSender _emailService;

        public SignUpViewModel ViewModel { get; set; }

        public SignUpModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<SignUpModel> logger,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = ViewModel.Email, Email = ViewModel.Email };
                _userManager.PasswordValidators.Clear();
                var result = await _userManager.CreateAsync(user, ViewModel.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.PageLink("/Identity/ConfirmEmail", values: new {code, userId = user.Id});

                    var message = $@"<html>
                      <body>
                      <p>Thank you for your interest in joining my website's community</p>
                      <p>Please verify your account's email by clicking 
                        <a href=""{callbackUrl}"">here</a>
                      </p>
                      </body>
                      </html>
                     ";

                    await _emailService.SendEmailAsync(ViewModel.Email, "Confirm your email", message);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("/Identity/RegistrationSuccess");
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Redirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
