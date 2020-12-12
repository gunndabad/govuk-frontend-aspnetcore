using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public static class TextOrHtmlExtensions
    {
        public static IHtmlContent GetHtmlContent(this ITextOrHtmlContent textOrHtml)
        {
            if (textOrHtml.Html != null)
            {
                return new HtmlString(textOrHtml.Html);
            }
            else if (textOrHtml.Text != null)
            {
                return new HtmlString(HtmlEncoder.Default.Encode(textOrHtml.Text));
            }
            else
            {
                return null;
            }
        }
    }
}
