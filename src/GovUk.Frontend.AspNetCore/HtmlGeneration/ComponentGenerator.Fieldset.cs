#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string FieldsetElement = "fieldset";
        internal const bool FieldsetLegendDefaultIsPageHeading = false;
        internal const string FieldsetLegendElement = "legend";

        public TagBuilder GenerateFieldset(
            string? describedBy,
            string? role,
            bool? legendIsPageHeading,
            IHtmlContent? legendContent,
            IDictionary<string, string>? legendAttributes,
            IHtmlContent content,
            IDictionary<string, string>? attributes)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            var tagBuilder = new TagBuilder(FieldsetElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-fieldset");

            if (role != null)
            {
                tagBuilder.Attributes.Add("role", role);
            }

            if (describedBy != null)
            {
                tagBuilder.Attributes.Add("aria-describedby", describedBy);
            }

            if (legendContent != null)
            {
                var legend = new TagBuilder(FieldsetLegendElement);
                legend.MergeAttributes(legendAttributes);
                legend.MergeCssClass("govuk-fieldset__legend");

                if (legendIsPageHeading == true)
                {
                    var h1 = new TagBuilder("h1");
                    h1.MergeCssClass("govuk-fieldset__heading");
                    h1.InnerHtml.AppendHtml(legendContent);
                    legend.InnerHtml.AppendHtml(h1);
                }
                else
                {
                    legend.InnerHtml.AppendHtml(legendContent);
                }

                tagBuilder.InnerHtml.AppendHtml(legend);
            }

            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }
    }
}
