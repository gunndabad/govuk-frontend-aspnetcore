using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore
{
    public interface IGovUkHtmlGenerator
    {
        TagBuilder GenerateAnchor(string href);
        TagBuilder GenerateBreadcrumbs(IEnumerable<IHtmlContent> items, IHtmlContent currentPageItem);
        TagBuilder GenerateCharacterCount(string elementId, int? maxLength, int? maxWords, decimal? threshold, IHtmlContent formGroup);
        TagBuilder GenerateDetails(bool open, IHtmlContent summary, IHtmlContent text);
        TagBuilder GenerateErrorMessage(string visuallyHiddenText, string id, IHtmlContent content);
        TagBuilder GenerateFieldset(string describedBy, bool isPageHeading, string role, IHtmlContent legendContent, IHtmlContent content);
        TagBuilder GenerateFormGroup(bool haveError, IHtmlContent content);
        TagBuilder GenerateHint(string id, IHtmlContent content);
        TagBuilder GenerateInsetText(IHtmlContent content);
        TagBuilder GenerateInput(bool haveError, string id, string name, string type, string value, string describedBy, string autocomplete, string pattern, string inputMode);
        TagBuilder GenerateLabel(string @for, bool isPageHeading, IHtmlContent content);
        TagBuilder GeneratePhaseBanner(string tag, IHtmlContent content);
        TagBuilder GenerateTabs(string id, string title, IEnumerable<(string id, string label, IHtmlContent content)> items);
        TagBuilder GenerateTag(IHtmlContent content);
        TagBuilder GenerateTextArea(bool haveError, string id, string name, int? rows, string describedBy, string autocomplete, IHtmlContent content);
        TagBuilder GenerateWarningText(IHtmlContent content, string iconFallbackText);

        string GetDisplayName(ViewContext viewContext, ModelExplorer modelExplorer, string expression);
        string GetFullHtmlFieldName(ViewContext viewContext, string expression);
        string GetModelValue(ViewContext viewContext, ModelExplorer modelExplorer, string expression);
        string GetValidationMessage(ViewContext viewContext, ModelExplorer modelExplorer, string expression);

        string GetActionLinkHref(ViewContext viewContext, string action, string controller, object values, string protocol, string host, string fragment);
        string GetPageLinkHref(ViewContext viewContext, string pageName, string pageHandler, object values, string protocol, string host, string fragment);
        string GetRouteLinkHref(ViewContext viewContext, string routeName, object values, string protocol, string host, string fragment);
    }
}