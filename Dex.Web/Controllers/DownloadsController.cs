using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dex.Common.DTO;
using Dex.DataAccess.Models;
using Dex.Infrastructure.Contracts.IServices;
using Dex.Web.ViewModels.Downloads;
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

    public interface IHandler<T>
    {
        IHandler<T> SetNext(IHandler<T> handler);

        T Handle(T request, object additionalData);
    }

    public abstract class PagingHandler : IHandler<IEnumerable<ProjectViewModel>>
    {
        private IHandler<IEnumerable<ProjectViewModel>> _nextHandler;

        public IHandler<IEnumerable<ProjectViewModel>> SetNext(
            IHandler<IEnumerable<ProjectViewModel>> handler)
        {
            this._nextHandler = handler;
            return handler;
        }

        public virtual IEnumerable<ProjectViewModel> Handle(IEnumerable<ProjectViewModel> request,
            object additionalData)
        {
            if (_nextHandler == null)
            {
                return request;
            }

            return _nextHandler.Handle(request, additionalData);
        }
    }

    public class SearchHandler : PagingHandler
    {
        public override IEnumerable<ProjectViewModel> Handle(IEnumerable<ProjectViewModel> request,
            object additionalData)
        {
            if (additionalData is PagingData pagingData && !string.IsNullOrEmpty(pagingData.SearchCriteria))
            {
                return base.Handle(
                    request.Where(p =>
                        p.ProjectName.Contains(pagingData.SearchCriteria, StringComparison.OrdinalIgnoreCase)),
                    additionalData);
            }

            return base.Handle(request, additionalData);
        }
    }

    public class SortHandler : PagingHandler
    {
        public override IEnumerable<ProjectViewModel> Handle(IEnumerable<ProjectViewModel> request,
            object additionalData)
        {
            if (additionalData is PagingData pagingData && !string.IsNullOrEmpty(pagingData.Sort))
            {
                return base.Handle(
                    ProjectSortResolver.Resolve(pagingData.Sort).Sort(request, pagingData.IsAscending),
                    additionalData);
            }

            return base.Handle(request, additionalData);
        }
    }

    public static class ProjectSortResolver
    {
        private static readonly Dictionary<string, ISortStrategy<ProjectViewModel>> _sortStrategies;

        static ProjectSortResolver()
        {
            _sortStrategies =
                new Dictionary<string, ISortStrategy<ProjectViewModel>>(StringComparer.OrdinalIgnoreCase)
                {
                    ["name"] = new ProjectNameSortStrategy(),
                    ["date"] = new ProjectDateSortStrategy(),
                    ["favorite"] = new ProjectFavoriteSortStrategy(),

                };
        }

        public static ISortStrategy<ProjectViewModel> Resolve(string type)
        {
            return _sortStrategies.TryGetValue(type, out var strategy) ? strategy : null;
        }
    }

    public interface ISortStrategy<T>
    {
        IEnumerable<T> Sort(IEnumerable<T> data, bool isAscending);
    }

    public class ProjectNameSortStrategy : ISortStrategy<ProjectViewModel>
    {
        public IEnumerable<ProjectViewModel> Sort(IEnumerable<ProjectViewModel> data, bool isAscending)
        {
            if (isAscending)
            {
                return data.OrderBy(p => p.ProjectName);
            }

            return data.OrderByDescending(p => p.ProjectName);
        }
    }

    public class ProjectDateSortStrategy : ISortStrategy<ProjectViewModel>
    {
        public IEnumerable<ProjectViewModel> Sort(IEnumerable<ProjectViewModel> data, bool isAscending)
        {
            if (isAscending)
            {
                return data.OrderBy(p => p.ProjectDate);
            }

            return data.OrderByDescending(p => p.ProjectDate);
        }
    }

    public class ProjectFavoriteSortStrategy : ISortStrategy<ProjectViewModel>
    {
        public IEnumerable<ProjectViewModel> Sort(IEnumerable<ProjectViewModel> data, bool isAscending)
        {
            if (isAscending)
            {
                return data.OrderBy(p => p.IsFavorite);
            }

            return data.OrderByDescending(p => p.IsFavorite);
        }
    }

    public class PagingData
    {
        public int CurrentPage { get; set; }
        public string SearchCriteria { get; set; }
        public string Sort { get; set; }
        public bool IsAscending { get; set; }
    }
}