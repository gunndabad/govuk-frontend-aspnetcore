using System.Collections.Immutable;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class FormGroupOptions
{
    public string? Classes { get; set; }
    public ImmutableDictionary<string, string?>? Attributes { get; set; }

    internal virtual void Validate() { }
}
