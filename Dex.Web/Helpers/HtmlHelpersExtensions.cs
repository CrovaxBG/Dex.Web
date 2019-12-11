using System.Security.Policy;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dex.Web.Helpers
{
    public static class HtmlHelpersExtensions
    {
        public static HtmlString ResourceBasedActionLink<TModel>(this IHtmlHelper<TModel> htmlHelper,
            string resourceName, string linkText, string actionName, string controllerName, object htmlAttributes)
        {
            var link = htmlHelper.ActionLink(linkText, actionName, controllerName, null, null, null, null,
                htmlAttributes);
            var raw = link.GetString();
            return new HtmlString(string.Format(resourceName, raw));
        }

        public static HtmlString ResourceBasedPageLink<TModel>(this IHtmlHelper<TModel> htmlHelper,
            string resourceName, string linkText, string pageUrl, object htmlAttributes)
        {
            var link =
                $@"<a {htmlAttributes?.ToString().Replace("{", string.Empty).Replace("}", string.Empty)} href=""{pageUrl}"" >{linkText}</a>";
            return new HtmlString(string.Format(resourceName, link));
        }
    }
}