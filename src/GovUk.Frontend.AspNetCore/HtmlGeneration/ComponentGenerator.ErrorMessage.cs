using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal partial class ComponentGenerator
{
    internal const string ErrorMessageElement = "p";
    internal const string ErrorMessageDefaultVisuallyHiddenText = "Error";

    public TagBuilder GenerateErrorMessage(
        string visuallyHiddenText,
        IHtmlContent content,
        AttributeDictionary? attributes
    )
    {
        Guard.ArgumentNotNull(nameof(content), content);

        var tagBuilder = new TagBuilder(ErrorMessageElement);
        tagBuilder.MergeOptionalAttributes(attributes);
        tagBuilder.MergeCssClass("govuk-error-message");

        if (!string.IsNullOrEmpty(visuallyHiddenText))
        {
            var vht = new TagBuilder("span");
            vht.MergeCssClass("govuk-visually-hidden");
            vht.InnerHtml.Append(visuallyHiddenText + ":");

            tagBuilder.InnerHtml.AppendHtml(vht);
        }

        tagBuilder.InnerHtml.AppendHtml(content);

        return tagBuilder;
    }
}
