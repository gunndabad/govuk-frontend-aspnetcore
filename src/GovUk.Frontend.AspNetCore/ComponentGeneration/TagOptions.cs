using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class TagOptions
{
    public string? Text { get; set; }
    public string? Html { get; set; }
    public string? Classes { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }

    internal void Validate(string? messageSuffix = null)
    {
        if (Html.NormalizeEmptyString() is null && Text.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(Html)} or {nameof(Text)} must be specified{(messageSuffix is not null ? " " + messageSuffix : "")}.");
        }
    }
}
