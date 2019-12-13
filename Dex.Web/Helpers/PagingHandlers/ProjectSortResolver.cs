using System;
using System.Collections.Generic;
using Dex.Web.ViewModels.Downloads;

namespace Dex.Web.Helpers.PagingHandlers
{
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
}