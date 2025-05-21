using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal partial class ComponentGenerator
{
    internal const bool InputDefaultDisabled = false;
    internal const string InputDefaultType = "text";
    internal const string InputElement = "input";
    internal const string InputPrefixElement = "div";
    internal const string InputSuffixElement = "div";

    public TagBuilder GenerateTextInput(
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
        AttributeDictionary? attributes,
        IHtmlContent? prefixContent,
        AttributeDictionary? prefixAttributes,
        IHtmlContent? suffixContent,
        AttributeDictionary? suffixAttributes)
    {
        Guard.ArgumentNotNull(nameof(id), id);
        Guard.ArgumentNotNull(nameof(name), name);
        Guard.ArgumentNotNull(nameof(id), type);

        var tagBuilder = new TagBuilder(InputElement);
        tagBuilder.MergeOptionalAttributes(attributes);
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

        if (!string.IsNullOrEmpty(describedBy))
        {
            tagBuilder.Attributes.Add("aria-describedby", describedBy);
        }

        if (!string.IsNullOrEmpty(autocomplete))
        {
            tagBuilder.Attributes.Add("autocomplete", autocomplete);
        }

        if (pattern != null)
        {
            tagBuilder.Attributes.Add("pattern", pattern);
        }

        if (!string.IsNullOrEmpty(inputMode))
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
                prefix.MergeOptionalAttributes(prefixAttributes);
                prefix.MergeCssClass("govuk-input__prefix");
                prefix.Attributes.Add("aria-hidden", "true");
                prefix.InnerHtml.AppendHtml(prefixContent);

                wrapper.InnerHtml.AppendHtml(prefix);
            }

            wrapper.InnerHtml.AppendHtml(tagBuilder);

            if (suffixContent != null)
            {
                var suffix = new TagBuilder(InputSuffixElement);
                suffix.MergeOptionalAttributes(suffixAttributes);
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
