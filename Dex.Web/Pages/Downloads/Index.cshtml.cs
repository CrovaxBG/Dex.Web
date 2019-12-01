using System;
using System.Collections.Generic;
using System.Linq;
using Dex.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dex.Web.Pages.Downloads
{
    public class IndexModel : PageModel
    {
        private const int ItemsPerPage = 10;

        private readonly DexContext _context;

        public int CurrentPage { get; set; } = 1;
        public int MaxPages => AllProjects == null ? 0 : (int) Math.Ceiling(AllProjects.Count / (double) ItemsPerPage);

        //TODO move to database
        public List<ProjectBasicViewModel> Projects { get; set; }
        private List<ProjectBasicViewModel> AllProjects;

        public IndexModel(DexContext context)
        {
            AllProjects = new List<ProjectBasicViewModel>();
            for (int i = 0; i < 100; i++)
            {
                AllProjects.Add(new ProjectBasicViewModel
                {
                    ProjectName = $"Fluent Generation #{i}",
                    ProjectDescription =
                        "Simple library for code generation in .net core. Intended purpose is replacing T4 templates.",
                    ProjectRepositoryLink = "https://github.com/CrovaxBG/FluentGeneration",
                    ProjectExecutableName = "FluentGeneration",
                    ProjectExecutableIconUrl = i%4 == 0 ? "~/images/exe_icon.png" : string.Empty,
                    ProjectRepositoryIconUrl = i%7 == 0 ? "~/images/github_repo_icon.png" : string.Empty
                });
            }
        }

        public void OnGet(int pageNumber)
        {
            pageNumber = Math.Max(1, pageNumber);
            pageNumber = Math.Min(pageNumber, MaxPages);

            CurrentPage = pageNumber;
            Projects = new List<ProjectBasicViewModel>(AllProjects
                .Skip(ItemsPerPage * (pageNumber - 1))
                .Take(ItemsPerPage));
        }
    }

    public class ProjectBasicViewModel
    {
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectRepositoryLink { get; set; }
        public string ProjectExecutableName { get; set; }

        public string ProjectRepositoryIconUrl { get; set; }
        public string ProjectExecutableIconUrl { get; set; }
    }
}
