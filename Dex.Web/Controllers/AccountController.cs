using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dex.Common.DTO;
using Dex.Common.Extensions;
using Dex.DataAccess.Models;
using Dex.Infrastructure.Contracts.IServices;
using Dex.Web.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Dex.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AspNetUsers> _signInManager;
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly IUserClaimsService _userClaimsService;

        public AccountController(SignInManager<AspNetUsers> signInManager, UserManager<AspNetUsers> userManager, IUserClaimsService userClaimsService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userClaimsService = userClaimsService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            await _signInManager.RefreshSignInAsync(user);

            var vm = await InitializeViewModel(user);

            if (User.HasClaim("Role", "Admin"))
            {
                //TODO return admin profile
                //TODO return admin profile
                //TODO return admin profile
                //TODO return admin profile
            }
            return View("MyAccountDefault", vm);
        }

        private async Task<AccountInformationViewModel> InitializeViewModel(AspNetUsers user)
        {
            var currentUserClaims = await _userManager.GetClaimsAsync(user);

            var privilegeTypes = currentUserClaims
                .Select(c => new SelectListItem { Text = c.Type, Value = c.Type })
                .DistinctBy(item => item.Text).ToList();

            var privilegeValues = currentUserClaims
                .Where(c => c.Type == privilegeTypes.FirstOrDefault()?.Text)
                .Select(c => new SelectListItem {Text = c.Value, Value = c.Value})
                .ToList();

            return new AccountInformationViewModel
            {
                Email = user.Email,
                Username = user.UserName,
                Privileges = currentUserClaims.Select(Convert).ToList()
            };

            PrivilegeViewModel Convert(Claim c)
            {
                return new PrivilegeViewModel
                {
                    Type = c.Type,
                    Value = c.Value,
                    AvailablePrivilegeTypes = privilegeTypes,
                    AvailablePrivilegeValues = privilegeValues
                };
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUserClaimsByType(string type)
        {
            var currentUser = await GetCurrentUserAsync();
            var currentUserClaims = await _userManager.GetClaimsAsync(currentUser);

            return Json(currentUserClaims.Where(c => c.Type == type));
        }

        [HttpPost]
        public IActionResult GetPartialTable(IEnumerable<AspNetUserClaimsDTO> claims)
        {
            return PartialView("_PrivilegesTablePartial", claims.Select(c => new PrivilegeViewModel
            {
                Type = c.ClaimType.Substring(c.ClaimType.LastIndexOf(@"/", StringComparison.Ordinal) + 1).ToUpperFirstChar().SplitOnCapitalLetters(),
                Value = c.ClaimValue.ToUpperFirstChar().SplitOnCapitalLetters(),
            }));
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Home/Index");
        }

        [HttpPost]
        public async Task RefreshSignIn(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var currentUser = await GetCurrentUserAsync();
                if (currentUser.Id == user.Id)
                {
                    await _signInManager.RefreshSignInAsync(user);
                }
            }
        }

        private async Task<AspNetUsers> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
    }
}