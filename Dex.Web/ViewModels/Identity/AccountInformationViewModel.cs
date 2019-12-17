using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dex.Web.ViewModels.Downloads;

namespace Dex.Web.ViewModels.Identity
{
    public class AccountInformationViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Privileges")]
        public List<PrivilegeViewModel> Privileges { get; set; }
    }
}