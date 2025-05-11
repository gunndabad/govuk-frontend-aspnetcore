using AngleSharp;
using AngleSharp.Dom;

namespace GovUk.Frontend.AspNetCore.TestCommon;

public static class HtmlHelper
{
    public static IElement ParseHtmlElement(string html)
    {
        var browsingContext = BrowsingContext.New();
        var doc = browsingContext.OpenAsync(req => req.Content(html)).GetAwaiter().GetResult();
        return doc.Body.FirstElementChild ?? doc.Head.FirstElementChild;
    }

    public static IElement[] ParseHtmlElements(string html)
    {
        var browsingContext = BrowsingContext.New();
        var doc = browsingContext.OpenAsync(req => req.Content(html)).GetAwaiter().GetResult();
        return (doc.Body.Children.Length > 0 ? doc.Body.Children : doc.Head.Children).ToArray();
    }
}
