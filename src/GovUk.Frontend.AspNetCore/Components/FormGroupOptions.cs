namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public abstract record FormGroupOptions
{
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}
