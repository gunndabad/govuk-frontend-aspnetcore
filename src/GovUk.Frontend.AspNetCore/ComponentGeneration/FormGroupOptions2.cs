namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record FormGroupOptions2
{
    public string? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public FormGroupOptions2BeforeInput? BeforeInput { get; set; }
    public FormGroupOptions2AfterInput? AfterInput { get; set; }
}

public record FormGroupOptions2BeforeInput
{
    public string? Text { get; set; }
    public string? Html { get; set; }
}

public record FormGroupOptions2AfterInput
{
    public string? Text { get; set; }
    public string? Html { get; set; }
}
