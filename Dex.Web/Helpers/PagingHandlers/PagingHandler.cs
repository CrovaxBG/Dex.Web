using System.Collections.Generic;
using Dex.Web.ViewModels.Downloads;

namespace Dex.Web.Helpers.PagingHandlers
{
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
}