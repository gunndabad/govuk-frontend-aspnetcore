using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    /// <summary>
    /// Utility extensions for <see cref="IHtmlContent"/>.
    /// </summary>
    public static class HtmlContentExtensions
    {
        /// <summary>
        /// Returns a <see cref="string"/> of HTML with the contents of the <paramref name="content"/>.
        /// </summary>
        public static string ToHtmlString(this IHtmlContent content)
        {
            using var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
