using System;
using System.Collections.Generic;
using System.Linq;
using Dex.Web.Controllers;
using Dex.Web.ViewModels.Downloads;

namespace Dex.Web.Helpers.PagingHandlers
{
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
}