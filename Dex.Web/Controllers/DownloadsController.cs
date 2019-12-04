using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dex.DataAccess.Models;
using Dex.Web.ViewModels.Downloads;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Dex.Web.Controllers
{
    public class DownloadsController : Controller
    {
        private const int ItemsPerPage = 10;

        private readonly DexContext _context;

        //TODO move to database
        private List<ProjectBasicViewModel> AllProjects;

        public DownloadsController(DexContext context)
        {
            _context = context;
            Mock();
        }

        private void Mock()
        {
            AllProjects = new List<ProjectBasicViewModel>();
            for (int i = 0; i < 20; i++)
            {
                AllProjects.Add(new ProjectBasicViewModel
                {
                    ProjectName = $"Fluent Generation #{i}",
                    ProjectDescription =
                        "Simple library for code generation in .net core. Intended purpose is replacing T4 templates.",
                    ProjectRepositoryLink = "https://github.com/CrovaxBG/FluentGeneration",
                    ProjectExecutableName = "FluentGeneration",
                    ProjectExecutableIconUrl = i % 4 == 0 ? "~/images/exe_icon.png" : string.Empty,
                    ProjectRepositoryIconUrl = i % 7 == 0 ? "~/images/github_repo_icon.png" : string.Empty,
                });
            }
        }

        [HttpGet]
        public IActionResult Index(int pageNumber = 1)
        {
            TempData["currentPage"] = pageNumber;
            return View();
        }

        [HttpGet]
        public JsonResult GetProjects(int pageNumber, string searchCriteria)
        {
            Debug.WriteLine("search: " + searchCriteria);
            TempData["currentPage"] = pageNumber;
            TempData["searchCriteria"] = searchCriteria;

            if (string.IsNullOrEmpty(searchCriteria))
            {
                var projects = GetProjectsByPage(pageNumber);
                return Json(new { items = projects, maxPages = GetMaxPages(_ => true) });
            }

            var filteredProjects = GetFilteredProjectsByPage(pageNumber, Predicate);
            return Json(new {items = filteredProjects, maxPages = GetMaxPages(Predicate) });

            bool Predicate(ProjectBasicViewModel p) => p.ProjectName.Contains(searchCriteria, StringComparison.OrdinalIgnoreCase);
        }

        [HttpGet]
        public IActionResult GetPartialProject(string json)
        {
            return PartialView("_ProjectPartial", JsonConvert.DeserializeObject<ProjectBasicViewModel>(json));
        }

        private int GetMaxPages(Func<ProjectBasicViewModel, bool> searchCriteria)
        {
            return AllProjects == null ? 1 : (int) Math.Ceiling(AllProjects.Where(searchCriteria).Count() / (double) ItemsPerPage);
        }

        private List<ProjectBasicViewModel> GetProjectsByPage(int pageNumber)
        {
            return new List<ProjectBasicViewModel>(AllProjects
                .Skip(ItemsPerPage * (pageNumber - 1))
                .Take(ItemsPerPage));
        }

        private List<ProjectBasicViewModel> GetFilteredProjectsByPage(int pageNumber, Func<ProjectBasicViewModel, bool> predicate)
        {
            return new List<ProjectBasicViewModel>(AllProjects
                .Where(predicate)
                .Skip(ItemsPerPage * (pageNumber - 1))
                .Take(ItemsPerPage));
        }
    }
}