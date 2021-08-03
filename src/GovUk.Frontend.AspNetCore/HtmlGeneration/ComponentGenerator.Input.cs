#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const bool InputDefaultDisabled = false;
        internal const string InputDefaultType = "text";
        internal const string InputElement = "input";
        internal const string InputPrefixElement = "div";
        internal const string InputSuffixElement = "div";

        public TagBuilder GenerateInput(
            bool haveError,
            string id,
            string name,
            string type,
            string? value,
            string? describedBy,
            string? autocomplete,
            string? pattern,
            string? inputMode,
            bool? spellcheck,
            bool disabled,
            IDictionary<string, string>? attributes,
            IHtmlContent? prefixContent,
            IDictionary<string, string>? prefixAttributes,
            IHtmlContent? suffixContent,
            IDictionary<string, string>? suffixAttributes)
        {
            Guard.ArgumentNotNull(nameof(id), id);
            Guard.ArgumentNotNull(nameof(name), name);
            Guard.ArgumentNotNull(nameof(id), type);

            var tagBuilder = new TagBuilder(InputElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-input");

            if (haveError)
            {
                tagBuilder.MergeCssClass("govuk-input--error");
            }

            tagBuilder.Attributes.Add("id", id);
            tagBuilder.Attributes.Add("name", name);
            tagBuilder.Attributes.Add("type", type);

            if (value != null)
            {
                tagBuilder.Attributes.Add("value", value);
            }

            if (describedBy != null)
            {
                tagBuilder.Attributes.Add("aria-describedby", describedBy);
            }

            if (autocomplete != null)
            {
                tagBuilder.Attributes.Add("autocomplete", autocomplete);
            }

            if (pattern != null)
            {
                tagBuilder.Attributes.Add("pattern", pattern);
            }

            if (inputMode != null)
            {
                tagBuilder.Attributes.Add("inputmode", inputMode);
            }

            if (spellcheck.HasValue)
            {
                tagBuilder.Attributes.Add("spellcheck", spellcheck.Value ? "true" : "false");
            }

            if (disabled)
            {
                tagBuilder.Attributes.Add("disabled", "disabled");
            }

            if (prefixContent != null || suffixContent != null)
            {
                var wrapper = new TagBuilder("div");
                wrapper.MergeCssClass("govuk-input__wrapper");

                if (prefixContent != null)
                {
                    var prefix = new TagBuilder(InputPrefixElement);
                    prefix.MergeAttributes(prefixAttributes);
                    prefix.MergeCssClass("govuk-input__prefix");
                    prefix.Attributes.Add("aria-hidden", "true");
                    prefix.InnerHtml.AppendHtml(prefixContent);

                    wrapper.InnerHtml.AppendHtml(prefix);
                }

                wrapper.InnerHtml.AppendHtml(tagBuilder);

                if (suffixContent != null)
                {
                    var suffix = new TagBuilder(InputSuffixElement);
                    suffix.MergeAttributes(suffixAttributes);
                    suffix.MergeCssClass("govuk-input__suffix");
                    suffix.Attributes.Add("aria-hidden", "true");
                    suffix.InnerHtml.AppendHtml(suffixContent);

                    wrapper.InnerHtml.AppendHtml(suffix);
                }

                return wrapper;
            }
            else
            {
                return tagBuilder;
            }
        }
    }
}
