using System.Text.Encodings.Web;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TestCommon;

public static class HtmlContentExtensions
{
    public static string ToHtmlString(this IHtmlContent content) => content.ToHtmlString(HtmlEncoder.Default);

    public static IElement RenderToElement(this IHtmlContent content)
    {
        var html = content.ToHtmlString(HtmlEncoder.Default);
        return HtmlHelper.ParseHtmlElement(html);
    }

    public static IElement[] RenderToElements(this IHtmlContent content)
    {
        var html = content.ToHtmlString(HtmlEncoder.Default);
        return HtmlHelper.ParseHtmlElements(html);
    }
}
