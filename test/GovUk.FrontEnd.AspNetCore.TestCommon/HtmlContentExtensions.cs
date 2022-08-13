using System.IO;
using System.Text.Encodings.Web;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TestCommon
{
    public static class HtmlContentExtensions
    {
        public static IElement RenderToElement(this IHtmlContent content)
        {
            var html = content.RenderToString();
            return HtmlHelper.ParseHtmlElement(html);
        }

        public static IElement[] RenderToElements(this IHtmlContent content)
        {
            var html = content.RenderToString();
            return HtmlHelper.ParseHtmlElements(html);
        }

        public static string RenderToString(this IHtmlContent content)
        {
            using var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
