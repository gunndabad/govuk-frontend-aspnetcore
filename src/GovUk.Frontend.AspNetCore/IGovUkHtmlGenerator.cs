using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore
{
    public interface IGovUkHtmlGenerator
    {
        TagBuilder GenerateBreadcrumbs(IEnumerable<IHtmlContent> items, IHtmlContent currentPageItem);
        TagBuilder GenerateErrorMessage(string visuallyHiddenText, IHtmlContent content);
        TagBuilder GenerateHint(IHtmlContent content);
        TagBuilder GenerateInsetText(IHtmlContent content);
        TagBuilder GenerateLabel(string @for, bool isPageHeading, IHtmlContent content);
        TagBuilder GeneratePhaseBanner(string tag, IHtmlContent content);
        TagBuilder GenerateTag(IHtmlContent content);
    }
}