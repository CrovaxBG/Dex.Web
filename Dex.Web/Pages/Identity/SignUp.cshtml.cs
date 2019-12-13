using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Dex.DataAccess.Models;
using Dex.Infrastructure.Contracts.IServices;
using Dex.Web.Helpers;
using Dex.Web.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Dex.Web.Pages.Identity
{
    [BindProperties]
    public class SignUpModel : PageModel
    {
        private readonly SignInManager<AspNetUsers> _signInManager;
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly ILoggerService _logger;
        private readonly IEmailSender _emailService;

        public SignUpViewModel ViewModel { get; set; }

        public string ReturnUrl { get; set; }

        public SignUpModel(
            UserManager<AspNetUsers> userManager,
            SignInManager<AspNetUsers> signInManager,
            ILoggerService logger,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Home/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new AspNetUsers { UserName = ViewModel.Username, Email = ViewModel.Email };
                var result = await _userManager.CreateAsync(user, ViewModel.Password);

                if (result.Succeeded)
                {
                    await _logger.Log("User created a new account with password.");

                    await _userManager.AddClaimsAsync(user, new[] {new Claim(ClaimTypes.Role, "User")});

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.PageLink("/Identity/ConfirmEmail", values: new { code, userId = user.Id });

                    var message = $@"<html>
                      <body>
                      <p>Thank you for your interest in joining my website's community</p>
                      <p>Please verify your account's email by clicking 
                        <a href=""{HtmlEncoder.Default.Encode(callbackUrl)}"">here</a>
                      </p>
                      </body>
                      </html>
                     ";

                    await _emailService.SendEmailAsync(ViewModel.Email, "Confirm your email", message);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        this.SetRedirectMessage("Registration successful! Please check your email to confirm your account.");
                        return RedirectToPage("/Home/Index");
                    }

                    this.SetRedirectMessage("Registration successful! You have been automatically logged in.");
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
