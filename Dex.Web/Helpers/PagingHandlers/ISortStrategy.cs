using System.Collections.Generic;

namespace Dex.Web.Helpers.PagingHandlers
{
    public interface ISortStrategy<T>
    {
        IEnumerable<T> Sort(IEnumerable<T> data, bool isAscending);
    }
}