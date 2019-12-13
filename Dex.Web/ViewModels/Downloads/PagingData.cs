namespace Dex.Web.ViewModels.Downloads
{
    public class PagingData
    {
        public int CurrentPage { get; set; }
        public string SearchCriteria { get; set; }
        public string Sort { get; set; }
        public bool IsAscending { get; set; }
    }
}