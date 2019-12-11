using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dex.Web.ViewModels.Downloads
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public DateTime ProjectDate { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectRepositoryLink { get; set; }
        public string ProjectExecutableName { get; set; }
        public bool IsFavorite { get; set; }
    }
}
