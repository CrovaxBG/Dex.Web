using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dex.Web.ViewModels.Identity;

namespace Dex.Web.ViewModels.Account
{
    //TODO maybe separate in subclasses for each partial view
    public class AccountInformationViewModel
    {
        public string CurrentUserId { get; set; }

        public string SelectedUserId { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Privileges")]
        public List<PrivilegeViewModel> Privileges { get; set; }

        [Display(Name = "Project Favorites")]
        public List<ProjectFavoriteViewModel> ProjectFavorites { get; set; }
    }
}