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
            string visuallyHiddenText);

        TagBuilder GenerateErrorMessage(
            string visuallyHiddenText,
            IHtmlContent content);

        TagBuilder GenerateHint(IHtmlContent content);

        TagBuilder GeneratePhaseBanner(string tag, IHtmlContent content);

        TagBuilder GenerateTag(IHtmlContent content);
    }
}