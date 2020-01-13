using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dex.Web.ViewModels.Account
{
    public class PrivilegeViewModel
    {
        [Display(Name = "Privilege Type")]
        [Required]
        public string Type { get; set; }
        
        [Display(Name = "Privilege Value")]
        [Required]
        public string Value { get; set; }

        public List<SelectListItem> AvailablePrivilegeTypes { get; set; }
        public List<SelectListItem> AvailablePrivilegeValues { get; set; }
    }
}