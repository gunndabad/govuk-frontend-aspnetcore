using System;
using System.Threading.Tasks.Sources;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateTextarea(TextareaOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        var describedBy = options.DescribedBy ?? new HtmlString(null);

        return new HtmlTagBuilder(FormGroupElement)
            .WithCssClass("govuk-form-group")
            .WhenNotNull(options.ErrorMessage, (_, b) => b.WithCssClass("govuk-form-group--error"))
            .WithCssClasses(ExplodeClasses(options.FormGroup?.Classes?.ToHtmlString()))
            .WithAttributes(options.FormGroup?.Attributes)
            .WithAppendedHtml(GenerateLabel(options.Label! with { For = options.Id! }))
            .WhenNotNull(options.Hint, (hint, b) =>
            {
                var hintId = new HtmlString(options.Id + "-hint");
                AppendToDescribedBy(ref describedBy, hintId);

                b.WithAppendedHtml(GenerateHint(hint with { Id = hintId }));
            })
            .WhenNotNull(options.ErrorMessage, (error, b) =>
            {
                var errorId = new HtmlString(options.Id + "-error");
                AppendToDescribedBy(ref describedBy, errorId);

                b.WithAppendedHtml(GenerateErrorMessage(error with { Id = errorId }));
            })
            .WhenNotNull(
                options.FormGroup?.BeforeInput,
                (beforeInput, b) => b.WithAppendedHtml(GetEncodedTextOrHtml(beforeInput.Text, beforeInput.Html)!))
            .WithAppendedHtml(new HtmlTagBuilder("textarea")
                .WithCssClass("govuk-textarea")
                .WhenNotNull(options.ErrorMessage, (_, b) => b.WithCssClass("govuk-textarea--error"))
                .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
                .WithAttribute("id", options.Id!)
                .WithAttribute("name", options.Name!)
                .WithAttribute("rows", (options.Rows ?? 5).ToString(), encodeValue: false)
                .WhenNotNull(options.Spellcheck, (spellcheck, b) => b.WithAttribute("spellcheck", spellcheck == true ? "true" : "false", encodeValue: false))
                .When(options.Disabled == true, b => b.WithBooleanAttribute("disabled"))
                .WhenNotNull(describedBy.NormalizeEmptyString(), (db, b) => b.WithAttribute("aria-describedby", db))
                .WhenNotNull(options.Autocomplete, (autocomplete, b) => b.WithAttribute("autocomplete", autocomplete))
                .WithAttributes(options.Attributes)
                .WithAppendedHtml(options.Value ?? new HtmlString("")))
            .WhenNotNull(
                options.FormGroup?.AfterInput,
                (afterInput, b) => b.WithAppendedHtml(GetEncodedTextOrHtml(afterInput.Text, afterInput.Html)!));
    }
}
