using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dex.Web.ViewModels.Downloads
{
    public class ProjectBasicViewModel
    {
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectRepositoryLink { get; set; }
        public string ProjectExecutableName { get; set; }

        public string ProjectRepositoryIconUrl { get; set; }
        public string ProjectExecutableIconUrl { get; set; }

        public string PageUrl { get; set; }
    }

    public class DownloadsViewModel
    {
        //public int CurrentPage { get; set; }
        //public string SearchCriteria { get; set; }
    }
}
