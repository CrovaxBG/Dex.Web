using System.Collections.Generic;
using Dex.Web.Controllers;
using Dex.Web.ViewModels.Downloads;

namespace Dex.Web.Helpers.PagingHandlers
{
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
}