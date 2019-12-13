namespace Dex.Web.Helpers.PagingHandlers
{
    public interface IHandler<T>
    {
        IHandler<T> SetNext(IHandler<T> handler);

        T Handle(T request, object additionalData);
    }
}
