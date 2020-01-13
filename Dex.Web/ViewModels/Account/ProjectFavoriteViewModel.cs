using System.ComponentModel.DataAnnotations;

namespace Dex.Web.ViewModels.Account
{
    public class ProjectFavoriteViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Project Name")]
        public string Name { get; set; }
    }
}