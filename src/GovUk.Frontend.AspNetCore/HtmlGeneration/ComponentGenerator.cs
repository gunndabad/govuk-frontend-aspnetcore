using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    internal partial class ComponentGenerator : IGovUkHtmlGenerator
    {
        internal const string FormGroupElement = "div";

        public virtual TagBuilder GenerateFormGroup(
            bool haveError,
            IHtmlContent content,
            AttributeDictionary? attributes)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            var tagBuilder = new TagBuilder(FormGroupElement);
            tagBuilder.MergeOptionalAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-form-group");

            if (haveError)
            {
                tagBuilder.MergeCssClass("govuk-form-group--error");
            }

            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }

        private static void AppendToDescribedBy(ref string? describedBy, string value)
        {
            if (value == null)
            {
                return;
            }

            if (describedBy == null)
            {
                describedBy = value;
            }
            else
            {
                describedBy += $" {value}";
            }
        }
    }
}
