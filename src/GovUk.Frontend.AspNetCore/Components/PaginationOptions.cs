namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record PaginationOptions
{
    public IReadOnlyCollection<PaginationOptionsItem>? Items { get; set; }
    public PaginationOptionsPrevious? Previous { get; set; }
    public PaginationOptionsNext? Next { get; set; }
    public TemplateString? LandmarkLabel { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record PaginationOptionsItem : IPaginationOptionsItem
{
    public TemplateString? Number { get; set; }
    public TemplateString? VisuallyHiddenText { get; set; }
    public TemplateString? Href { get; set; }
    public bool? Current { get; set; }
    public bool? Ellipsis { get; set; }
    public AttributeCollection? Attributes { get; set; }
}

public record PaginationOptionsPrevious : IPaginationOptionsItem
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? LabelText { get; set; }
    public TemplateString? Href { get; set; }
    public AttributeCollection? Attributes { get; set; }

    [NonStandardParameter]
    public AttributeCollection? ContainerAttributes { get; set; }
}

public record PaginationOptionsNext : IPaginationOptionsItem
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? LabelText { get; set; }
    public TemplateString? Href { get; set; }
    public AttributeCollection? Attributes { get; set; }

    [NonStandardParameter]
    public AttributeCollection? ContainerAttributes { get; set; }
}

internal interface IPaginationOptionsItem
{
}
