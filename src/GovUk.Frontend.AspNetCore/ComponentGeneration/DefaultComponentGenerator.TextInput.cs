using System;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    internal const bool InputDefaultDisabled = false;
    internal const string InputDefaultType = "text";
    internal const string InputElement = "input";
    internal const string InputPrefixElement = "div";
    internal const string InputSuffixElement = "div";

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateTextInput(TextInputOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        var describedBy = options.DescribedBy ?? new HtmlString("");

        var hasPrefix = GetEncodedTextOrHtml(options.Prefix?.Text, options.Prefix?.Html) is not null;
        var hasSuffix = GetEncodedTextOrHtml(options.Suffix?.Text, options.Suffix?.Html) is not null;
        var hasBeforeInput = GetEncodedTextOrHtml(options.FormGroup?.BeforeInput?.Text, options.FormGroup?.BeforeInput?.Html) is not null;
        var hasAfterInput = GetEncodedTextOrHtml(options.FormGroup?.AfterInput?.Text, options.FormGroup?.AfterInput?.Html) is not null;

        HtmlTagBuilder GetInputElement() => new HtmlTagBuilder(DefaultComponentGenerator.InputElement)
            .WithCssClass("govuk-input")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WhenNotNull(options.ErrorMessage, (_, b) => b.WithCssClass("govuk-input--error"))
            .WithAttribute("id", options.Id!)
            .WithAttribute("name", options.Name!)
            .WithAttribute("type", options.Type ?? new HtmlString(InputDefaultType))
            .WhenNotNull(options.Spellcheck, (s, b) => b.WithAttribute("spellcheck", s == true ? "true" : "false", encodeValue: false))
            .WithAttributeWhenNotNull(options.Value.NormalizeEmptyString(), "value")
            .When(options.Disabled == true, b => b.WithBooleanAttribute("disabled"))
            .WithAttributeWhenNotNull(describedBy.NormalizeEmptyString(), "aria-describedby")
            .WithAttributeWhenNotNull(options.Autocomplete.NormalizeEmptyString(), "autocomplete")
            .WithAttributeWhenNotNull(options.Pattern.NormalizeEmptyString(), "pattern")
            .WithAttributeWhenNotNull(options.Inputmode.NormalizeEmptyString(), "inputmode")
            .WithAttributeWhenNotNull(options.Autocapitalize.NormalizeEmptyString(), "autocapitalize")
            .WithAttributes(options.Attributes);

        return new HtmlTagBuilder(FormGroupElement)
            .WithCssClass("govuk-form-group")
            .WhenNotNull(options.ErrorMessage, (_, b) => b.WithCssClass("govuk-form-group--error"))
            .WithCssClasses(ExplodeClasses(options.FormGroup?.Classes?.ToHtmlString()))
            .WithAttributes(options.FormGroup?.Attributes)
            .WithAppendedHtml(GenerateLabel(options.Label! with { For = options.Id }))
            .WhenNotNull(
                options.Hint,
                (hint, b) =>
                {
                    var hintId = new HtmlString($"{options.Id}-hint");
                    AppendToDescribedBy(ref describedBy, hintId);

                    b.WithAppendedHtml(GenerateHint(hint with { Id = hintId }));
                })
            .WhenNotNull(
                options.ErrorMessage,
                (errorMessage, b) =>
                {
                    var errorId = new HtmlString($"{options.Id}-error");
                    AppendToDescribedBy(ref describedBy, errorId);

                    b.WithAppendedHtml(GenerateErrorMessage(errorMessage with { Id = errorId }));
                })
            .When(
                hasPrefix || hasSuffix || hasBeforeInput || hasAfterInput,
                b => b.WithAppendedHtml(new HtmlTagBuilder("div")
                    .WithCssClass("govuk-input__wrapper")
                    .WithCssClasses(ExplodeClasses(options.InputWrapper?.Classes?.ToHtmlString()))
                    .WithAttributes(options.InputWrapper?.Attributes)
                    .When(
                        hasBeforeInput,
                        bi => bi.WithAppendedHtml(GetEncodedTextOrHtml(options.FormGroup!.BeforeInput!.Text,
                            options.FormGroup.BeforeInput!.Html)!))
                    .When(
                        hasPrefix,
                        bi => bi.WithAppendedHtml(new HtmlTagBuilder(InputPrefixElement)
                            .WithCssClass("govuk-input__prefix")
                            .WithCssClasses(ExplodeClasses(options.Prefix!.Classes?.ToHtmlString()))
                            .WithAttribute("aria-hidden", "true", encodeValue: false)
                            .WithAttributes(options.Prefix.Attributes)
                            .WithAppendedHtml(GetEncodedTextOrHtml(options.Prefix.Text, options.Prefix.Html)!)))
                    .WithAppendedHtml(GetInputElement())
                    .When(
                        hasSuffix,
                        bi => bi.WithAppendedHtml(new HtmlTagBuilder(InputSuffixElement)
                            .WithCssClass("govuk-input__suffix")
                            .WithCssClasses(ExplodeClasses(options.Suffix!.Classes?.ToHtmlString()))
                            .WithAttribute("aria-hidden", "true", encodeValue: false)
                            .WithAttributes(options.Suffix.Attributes)
                            .WithAppendedHtml(GetEncodedTextOrHtml(options.Suffix.Text, options.Suffix.Html)!)))
                    .When(
                        hasAfterInput,
                        bi => bi.WithAppendedHtml(GetEncodedTextOrHtml(options.FormGroup!.AfterInput!.Text,
                            options.FormGroup.AfterInput!.Html)!))),
                b => b.WithAppendedHtml(GetInputElement()));
    }
}
