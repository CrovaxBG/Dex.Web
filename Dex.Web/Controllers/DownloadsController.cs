using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dex.Common.DTO;
using Dex.DataAccess.Models;
using Dex.Infrastructure.Contracts.IServices;
using Dex.Web.Helpers.PagingHandlers;
using Dex.Web.Pages.Identity;
using Dex.Web.ViewModels.Downloads;
using Dex.Web.ViewModels.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dex.Web.Controllers
{
    public class DownloadsController : Controller
    {
        private const int ItemsPerPage = 10;

        private readonly IMapper _mapper;
        private readonly IProjectsService _projectsService;
        private readonly IProjectFavoritesService _projectFavoritesService;
        private readonly UserManager<AspNetUsers> _userManager;

        private readonly PagingHandler _pagingHandler;

        public DownloadsController(
            IProjectsService projectsService,
            IProjectFavoritesService projectFavoritesService,
            UserManager<AspNetUsers> userManager,
            IMapper mapper)
        {
            _mapper = mapper;
            _projectsService = projectsService;
            _projectFavoritesService = projectFavoritesService;
            _userManager = userManager;
            _pagingHandler = new SearchHandler();
            _pagingHandler.SetNext(new SortHandler());
        }

        [HttpGet]
        public IActionResult Index(int pageNumber = 1)
        {
            TempData["currentPage"] = pageNumber;
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetProjects(PagingData data)
        {
            TempData["searchCriteria"] = data.SearchCriteria; //keep search after refresh in ui
            var currentUser = await GetCurrentUserAsync();
            var userFavorites = await _projectFavoritesService.GetFavoritesByUser(currentUser?.Id);
            var allProjects = (await _projectsService.GetProjects()).Select(p =>
            {
                var vm = _mapper.Map<ProjectsDTO, ProjectViewModel>(p);
                vm.IsFavorite = currentUser != null && userFavorites.Any(f => f.ProjectId == p.Id);

                return vm;
            });

            var projects = _pagingHandler.Handle(allProjects, data);

            var currentProjects = projects
                .Skip(ItemsPerPage * (data.CurrentPage - 1))
                .Take(ItemsPerPage);

            return Json(new
            {
                items = currentProjects,
                maxPages = GetMaxPages(projects),
            });
        }

        [HttpGet]
        public IActionResult GetPartialProject(string json)
        {
            return PartialView("_ProjectPartial", JsonConvert.DeserializeObject<ProjectViewModel>(json));
        }

        [HttpGet]
        public IActionResult GetPartialNoResults()
        {
            return PartialView("_NoResultsPartial");
        }

        [Authorize(Policy = "User")]
        [HttpGet]
        public async Task<IActionResult> SetFavoriteProject(int projectId)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                return Json(new {success = false});
            }

            await _projectFavoritesService.AddFavorite(new ProjectFavoritesDTO
                {UserId = currentUser.Id, ProjectId = projectId});

            return Json(new {success = true});
        }

        [Authorize(Policy = "User")]
        [HttpGet]
        public async Task<IActionResult> RemoveFavoriteProject(int projectId)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                return Json(new {success = false});
            }

            await _projectFavoritesService.RemoveFavorite(new ProjectFavoritesDTO
                {UserId = currentUser.Id, ProjectId = projectId});

            return Json(new {success = true});
        }

        private int GetMaxPages(IEnumerable<ProjectViewModel> source)
        {
            return source == null ? 1 : (int) Math.Ceiling(source.Count() / (double) ItemsPerPage);
        }

        private Task<AspNetUsers> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}