using System;
using System.Diagnostics;
using System.Linq;
using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

public partial class DefaultComponentGenerator
{
    /// <inheritdoc/>
    public virtual HtmlTag GenerateDateInput(DateInputOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        var hasError = options.ErrorMessage is not null;
        var describedBy = options.Fieldset?.DescribedBy ?? "";

        var items = options.Items?.Count > 0 ?
            options.Items :
            [
                new DateInputOptionsItem()
                {
                    Name = "day",
                    Classes = "govuk-input--width-2"
                },
                new DateInputOptionsItem()
                {
                    Name = "month",
                    Classes = "govuk-input--width-2"
                },
                new DateInputOptionsItem()
                {
                    Name = "year",
                    Classes = "govuk-input--width-4"
                },
            ];

        var wrapper = new HtmlTag("").NoTag();

        if (options.Hint is not null)
        {
            var hintId = $"{options.Id}-hint";
            AppendToDescribedBy(ref describedBy, hintId);

            wrapper.Append(GenerateHint(new HintOptions()
            {
                Id = hintId,
                Classes = options.Hint.Classes,
                Attributes = options.Hint.Attributes,
                Html = options.Hint.Html,
                Text = options.Hint.Text
            }));
        }

        if (options.ErrorMessage is not null)
        {
            Debug.Assert(hasError);

            var errorId = $"{options.Id}-error";
            AppendToDescribedBy(ref describedBy, errorId);

            wrapper.Append(GenerateErrorMessage(new ErrorMessageOptions()
            {
                Id = errorId,
                Classes = options.ErrorMessage.Classes,
                Attributes = options.ErrorMessage.Attributes,
                Html = options.ErrorMessage.Html,
                Text = options.ErrorMessage.Text,
                VisuallyHiddenText = options.ErrorMessage.VisuallyHiddenText
            }));
        }

        wrapper.Append(new HtmlTag("div")
            .AddClass("govuk-date-input")
            .AddClasses(ExplodeClasses(options.Classes))
            .MergeEncodedAttributes(options.Attributes)
            .AddEncodedAttributeIfNotNull("id", options.Id)
            .Append(items.Select(item => new HtmlTag("div")
                .AddClass("govuk-date-input__item")
                .AppendHtmlIf(
                    options.FormGroup?.BeforeInputs is not null,
                    () => GetEncodedTextOrHtml(options.FormGroup!.BeforeInputs!.Text, options.FormGroup!.BeforeInputs!.Html) ?? "")
                .Append(GenerateTextInput(new TextInputOptions()
                {
                    Label = new LabelOptions()
                    {
                        Html = item.LabelHtml,  // Non-standard, will be preferred over Text if set
                        Text = item.Label ?? item.Name!.Capitalize(),
                        Classes = "govuk-date-input__label",
                        Attributes = item.LabelAttributes
                    },
                    Id = item.Id ?? (options.Id + "-" + item.Name),
                    Classes = "govuk-date-input__input " + item.Classes,
                    Name = options.NamePrefix is not null ? options.NamePrefix + "-" + item.Name : item.Name,
                    Value = item.Value,
                    Type = "text",
                    Inputmode = item.InputMode ?? "numeric",
                    Autocomplete = item.Autocomplete,
                    Pattern = item.Pattern,
                    Attributes = item.Attributes
                }))
                .AppendHtmlIf(
                    options.FormGroup?.AfterInputs is not null,
                    () => GetEncodedTextOrHtml(options.FormGroup!.AfterInputs!.Text, options.FormGroup!.AfterInputs!.Html) ?? ""))));

        var formGroup = GenerateFormGroup(options.FormGroup, hasError);

        if (options.Fieldset is not null)
        {
            formGroup.Append(
                GenerateFieldset(new FieldsetOptions()
                {
                    DescribedBy = describedBy,
                    Classes = options.Fieldset.Classes,
                    Role = "group",
                    Attributes = options.Fieldset.Attributes,
                    Legend = options.Fieldset.Legend
                }).Append(wrapper));
        }
        else
        {
            formGroup.Append(wrapper);
        }

        return formGroup;
    }
}
