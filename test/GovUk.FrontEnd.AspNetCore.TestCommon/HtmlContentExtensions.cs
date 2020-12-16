using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TestCommon
{
    public static class HtmlContentExtensions
    {
        public static async Task<IElement> RenderToElement(this IHtmlContent content)
        {
            var html = content.RenderToString();

            var browsingContext = BrowsingContext.New();
            var doc = await browsingContext.OpenAsync(req => req.Content(html));
            return doc.Body.FirstElementChild;
        }

        public static string RenderToString(this IHtmlContent content)
        {
            using var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
