using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Tests
{
    public static class HtmlContentExtensions
    {
        public static string AsString(this IHtmlContent content)
        {
            using var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
