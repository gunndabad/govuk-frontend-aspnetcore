using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class ErrorSummaryOptions
{
    public string? TitleText { get; set; }
    public string? TitleHtml { get; set; }
    public string? DescriptionText { get; set; }
    public string? DescriptionHtml { get; set; }
    public IReadOnlyCollection<ErrorSummaryOptionsErrorItem>? ErrorList { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
    public bool? DisableAutoFocus { get; set; }

    [NonStandardParameter]
    public IReadOnlyDictionary<string, string?>? TitleAttributes { get; set; }

    [NonStandardParameter]
    public IReadOnlyDictionary<string, string?>? DescriptionAttributes { get; set; }

    internal void Validate()
    {
        if (ErrorList is not null)
        {
            int i = 0;
            foreach (var item in ErrorList)
            {
                item.Validate(i++);
            }
        }
    }
}

public class ErrorSummaryOptionsErrorItem
{
    public string? Href { get; set; }
    public string? Text { get; set; }
    public string? Html { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }

    [NonStandardParameter]
    public IReadOnlyDictionary<string, string?>? ItemAttributes { get; set; }

    internal void Validate(int itemIndex)
    {
        if (Html.NormalizeEmptyString() is null && Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(
                GetType(),
                $"{nameof(Html)} or {nameof(Text)} must be specified on item {itemIndex}."
            );
        }
    }
}
