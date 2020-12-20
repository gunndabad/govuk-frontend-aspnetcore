using AngleSharp;
using AngleSharp.Dom;

namespace GovUk.Frontend.AspNetCore.TestCommon
{
    public static class HtmlHelper
    {
        public static IElement ParseHtmlElement(string html)
        {
            var browsingContext = BrowsingContext.New();
            var doc = browsingContext.OpenAsync(req => req.Content(html)).GetAwaiter().GetResult();
            return doc.Body.FirstElementChild;
        }
    }
}
