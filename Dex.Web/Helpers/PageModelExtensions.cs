using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dex.Common.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dex.Web.Helpers
{
    public static class PageModelExtensions
    {
        public static void SetRedirectMessage(this PageModel page, string message, RedirectMessageType messageType = RedirectMessageType.Notice)
        {
            page.TempData["RedirectMessageType"] = messageType.ToString();
            page.TempData["RedirectMessage"] = message;
        }
    }
}
