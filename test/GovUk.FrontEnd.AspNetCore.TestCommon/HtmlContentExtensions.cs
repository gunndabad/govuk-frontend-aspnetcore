using AngleSharp.Dom;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TestCommon;

public static class HtmlContentExtensions
{
    public static IElement RenderToElement(this IHtmlContent content)
    {
        var html = content.ToHtmlString();
        return HtmlHelper.ParseHtmlElement(html);
    }

    public static IElement[] RenderToElements(this IHtmlContent content)
    {
        var html = content.ToHtmlString();
        return HtmlHelper.ParseHtmlElements(html);
    }
}
