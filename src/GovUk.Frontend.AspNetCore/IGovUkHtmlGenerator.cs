using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore
{
    public interface IGovUkHtmlGenerator
    {
        TagBuilder GenerateErrorMessage(
            ViewContext viewContext,
            ModelExplorer modelExplorer,
            string expression,
            string id,
            string visuallyHiddenText);

        TagBuilder GenerateErrorMessage(
            string id,
            string visuallyHiddenText,
            IHtmlContent content);

        TagBuilder GenerateHint(IHtmlContent content);

        TagBuilder GenerateLabel(
            ViewContext viewContext,
            ModelExplorer modelExplorer,
            string expression,
            bool isPageHeading,
            IHtmlContent content);

        TagBuilder GenerateLabel(
            string @for,
            bool isPageHeading,
            IHtmlContent content);

        TagBuilder GeneratePhaseBanner(string tag, IHtmlContent content);

        TagBuilder GenerateTag(IHtmlContent content);
    }
}