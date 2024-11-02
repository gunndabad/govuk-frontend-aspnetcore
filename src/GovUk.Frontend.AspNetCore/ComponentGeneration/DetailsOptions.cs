using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class DetailsOptions
{
    public string? Id { get; set; }
    public bool? Open { get; set; }
    public string? SummaryHtml { get; set; }
    public string? SummaryText { get; set; }
    public string? Html { get; set; }
    public string? Text { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }

    [NonStandardParameter]
    internal IReadOnlyDictionary<string, string?>? SummaryAttributes { get; set; }

    [NonStandardParameter]
    internal IReadOnlyDictionary<string, string?>? TextAttributes { get; set; }

    internal void Validate()
    {
        if (SummaryHtml.NormalizeEmptyString() is null && SummaryText.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(
                GetType(),
                $"{nameof(SummaryHtml)} or {nameof(SummaryText)} must be specified."
            );
        }

        if (Html.NormalizeEmptyString() is null && Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Html)} or {nameof(Text)} must be specified.");
        }
    }
}
