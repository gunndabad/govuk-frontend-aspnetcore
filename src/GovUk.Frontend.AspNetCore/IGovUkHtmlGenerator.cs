using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore
{
    public interface IGovUkHtmlGenerator
    {
        TagBuilder GenerateLink(string href);
        TagBuilder GenerateActionLink(ViewContext viewContext, string action, string controller, object values, string protocol, string host, string fragment);
        TagBuilder GeneratePageLink(ViewContext viewContext, string pageName, string pageHandler, object values, string protocol, string host, string fragment);
        TagBuilder GenerateRouteLink(ViewContext viewContext, string routeName, object values, string protocol, string host, string fragment);
        TagBuilder GenerateBreadcrumbs(IEnumerable<IHtmlContent> items, IHtmlContent currentPageItem);
        TagBuilder GenerateDetails(bool open, IHtmlContent summary, IHtmlContent text);
        TagBuilder GenerateErrorMessage(string visuallyHiddenText, string id, IHtmlContent content);
        TagBuilder GenerateHint(IHtmlContent content);
        TagBuilder GenerateInsetText(IHtmlContent content);
        TagBuilder GenerateLabel(ViewContext viewContext, ModelExplorer modelExplorer, string expression, bool isPageHeading, IHtmlContent content);
        TagBuilder GenerateLabel(string @for, bool isPageHeading, IHtmlContent content);
        TagBuilder GeneratePhaseBanner(string tag, IHtmlContent content);
        TagBuilder GenerateTag(IHtmlContent content);
        TagBuilder GenerateWarningText(IHtmlContent content, string iconFallbackText);
    }
}