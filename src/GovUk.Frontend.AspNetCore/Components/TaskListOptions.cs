namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record TaskListOptions
{
    public IReadOnlyCollection<TaskListOptionsItem>? Items { get; set; }
    public TemplateString? Classes { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public TemplateString? IdPrefix { get; set; }
}

public record TaskListOptionsItem
{
    public TaskListOptionsItemTitle? Title { get; set; }
    public TaskListOptionsItemHint? Hint { get; set; }
    public TaskListOptionsItemStatus? Status { get; set; }
    public TemplateString? Href { get; set; }
    public TemplateString? Classes { get; set; }
}

public record TaskListOptionsItemTitle
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Classes { get; set; }
}

public record TaskListOptionsItemHint
{
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
}

public record TaskListOptionsItemStatus
{
    public TagOptions? Tag { get; set; }
    public TemplateString? Text { get; set; }
    public TemplateString? Html { get; set; }
    public TemplateString? Classes { get; set; }
}
