using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class DateInputOptions
{
    public string? Id { get; set; }
    public string? NamePrefix { get; set; }
    public IReadOnlyCollection<DateInputOptionsItem>? Items { get; set; }
    public HintOptions? Hint { get; set; }
    public ErrorMessageOptions? ErrorMessage { get; set; }
    public DateInputOptionsFormGroup? FormGroup { get; set; }
    public FieldsetOptions? Fieldset { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }

    internal void Validate()
    {
        if (Id is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Id)} must be specified.");
        }

        if (Items is not null)
        {
            int i = 0;
            foreach (var item in Items)
            {
                item.Validate(i++);
            }
        }

        FormGroup?.Validate();

        Hint?.Validate();

        ErrorMessage?.Validate();
    }
}

public class DateInputOptionsFormGroup : FormGroupOptions
{
    public DateInputOptionsFormGroupBeforeInputs? BeforeInputs { get; set; }
    public DateInputOptionsFormGroupAfterInputs? AfterInputs { get; set; }

    internal override void Validate()
    {
        BeforeInputs?.Validate();
        AfterInputs?.Validate();
    }
}

public class DateInputOptionsFormGroupBeforeInputs
{
    public string? Html { get; set; }
    public string? Text { get; set; }

    internal void Validate()
    {
    }
}

public class DateInputOptionsFormGroupAfterInputs
{
    public string? Html { get; set; }
    public string? Text { get; set; }

    internal void Validate()
    {
    }
}

public class DateInputOptionsItem
{
    public string? Id { get; set; }
    [DisallowNull]
    public string? Name { get; set; }
    public string? Label { get; set; }
    public string? Value { get; set; }
    public string? Autocomplete { get; set; }
    public string? InputMode { get; set; }
    public string? Pattern { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }

    [NonStandardParameter]
    public string? LabelHtml { get; set; }
    [NonStandardParameter]
    public IReadOnlyDictionary<string, string?>? LabelAttributes { get; set; }

    internal void Validate(int itemIndex)
    {
        if (Name is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Name)} must be specified on item {itemIndex}.");
        }
    }
}
