using System.Collections.Generic;
using System.Linq;
using Dex.Web.ViewModels.Downloads;

namespace Dex.Web.Helpers.PagingHandlers
{
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
}