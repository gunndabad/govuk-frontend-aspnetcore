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

        TagBuilder GenerateTag(IHtmlContent content);
    }
}