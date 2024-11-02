using System;
using System.Diagnostics;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const bool InputDefaultDisabled = false;
    internal const string InputDefaultType = "text";
    internal const string InputElement = "input";
    internal const string InputPrefixElement = "div";
    internal const string InputSuffixElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTag GenerateTextInput(TextInputOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        var hasError = options.ErrorMessage is not null;
        var hasPrefixOrSuffix = options.Prefix is not null || options.Suffix is not null;
        var describedBy = options.DescribedBy ?? "";

        var formGroup = GenerateFormGroup(options.FormGroup, hasError);

        if (options.Label is not null)
        {
            formGroup.Append(
                GenerateLabel(
                    new LabelOptions()
                    {
                        Html = options.Label.Html,
                        Text = options.Label.Text,
                        Classes = options.Label.Classes,
                        IsPageHeading = options.Label.IsPageHeading,
                        Attributes = options.Label.Attributes,
                        For = options.Id,
                    }
                )
            );
        }

        if (options.Hint is not null)
        {
            var hintId = $"{options.Id}-hint";
            AppendToDescribedBy(ref describedBy, hintId);

            formGroup.Append(
                GenerateHint(
                    new HintOptions()
                    {
                        Id = hintId,
                        Classes = options.Hint.Classes,
                        Attributes = options.Hint.Attributes,
                        Html = options.Hint.Html,
                        Text = options.Hint.Text,
                    }
                )
            );
        }

        if (options.ErrorMessage is not null)
        {
            Debug.Assert(hasError);

            var errorId = $"{options.Id}-error";
            AppendToDescribedBy(ref describedBy, errorId);

            formGroup.Append(
                GenerateErrorMessage(
                    new ErrorMessageOptions()
                    {
                        Id = errorId,
                        Classes = options.ErrorMessage.Classes,
                        Attributes = options.ErrorMessage.Attributes,
                        Html = options.ErrorMessage.Html,
                        Text = options.ErrorMessage.Text,
                        VisuallyHiddenText = options.ErrorMessage.VisuallyHiddenText,
                    }
                )
            );
        }

        var wrapper = hasPrefixOrSuffix ? new HtmlTag("div").AddClass("govuk-input__wrapper") : null;

        if (options.Prefix is not null)
        {
            wrapper!.Append(
                new HtmlTag(InputPrefixElement)
                    .AddClass("govuk-input__prefix")
                    .AddClasses(ExplodeClasses(options.Prefix.Classes))
                    .UnencodedAttr("aria-hidden", "true")
                    .MergeEncodedAttributes(options.Prefix.Attributes)
                    .AppendHtml(GetEncodedTextOrHtml(options.Prefix.Text, options.Prefix.Html))
            );
        }

        var input = new HtmlTag(InputElement)
            .AddClass("govuk-input")
            .AddClasses(ExplodeClasses(options.Classes))
            .AddClassIf(hasError, "govuk-input--error")
            .UnencodedAttr("id", options.Id)
            .UnencodedAttr("name", options.Name)
            .UnencodedAttr("type", options.Type ?? InputDefaultType)
            .AddEncodedAttributeIf(
                options.Spellcheck.HasValue,
                "spellcheck",
                options.Spellcheck == true ? "true" : "false"
            )
            .AddEncodedAttributeIfNotNull("value", options.Value.NormalizeEmptyString())
            .AddEncodedAttributeIf(options.Disabled == true, "disabled", string.Empty)
            .AddEncodedAttributeIfNotNull("aria-describedby", describedBy.NormalizeEmptyString())
            .AddEncodedAttributeIfNotNull("autocomplete", options.Autocomplete.NormalizeEmptyString())
            .AddEncodedAttributeIfNotNull("pattern", options.Pattern.NormalizeEmptyString())
            .AddEncodedAttributeIfNotNull("inputmode", options.Inputmode.NormalizeEmptyString())
            .MergeEncodedAttributes(options.Attributes);

        wrapper?.Append(input);

        if (options.Suffix is not null)
        {
            wrapper!.Append(
                new HtmlTag(InputSuffixElement)
                    .AddClass("govuk-input__suffix")
                    .AddClasses(ExplodeClasses(options.Suffix.Classes))
                    .UnencodedAttr("aria-hidden", "true")
                    .MergeEncodedAttributes(options.Suffix.Attributes)
                    .AppendHtml(GetEncodedTextOrHtml(options.Suffix.Text, options.Suffix.Html))
            );
        }

        formGroup.Append(wrapper ?? input);

        return formGroup;
    }
}
