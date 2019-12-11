using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

namespace Dex.Web.Helpers
{
    public static class HtmlContentExtensions
    {
        public static string GetString(this IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}