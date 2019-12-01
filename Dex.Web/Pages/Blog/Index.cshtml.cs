using Dex.Infrastructure.Contracts.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Dex.Web.Pages.Blog
{
    [Authorize(Policy = "Admin")]
    public class IndexModel : PageModel
    {
        public string TestMessage { get; set; }

        public IndexModel(ILoggerService logger)
        {
            //fetch data from service
        }

        public void OnGet()
        {
            
        }

        public void OnPostLike(int postId)
        {
            TestMessage = $"You liked post with id {postId}";
        }

        public void OnPostComment(int postId, string postComment)
        {
            TestMessage = $"You commented: '{postComment}' on post with id {postId}";
        }

        public void OnPostPost(int postId)
        {
            TestMessage = $"You posted on post with id {postId}";
        }
    }
}
