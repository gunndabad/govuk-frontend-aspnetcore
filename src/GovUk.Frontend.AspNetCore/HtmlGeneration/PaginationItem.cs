using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal abstract class PaginationItemBase
{
    private protected PaginationItemBase() { }

    public AttributeDictionary? Attributes { get; set; }
}

internal class PaginationItem : PaginationItemBase
{
    [DisallowNull]
    public IHtmlContent? Number { get; set; }
    public string? Href { get; set; }
    public bool IsCurrent { get; set; }
    public string? VisuallyHiddenText { get; set; }
}

internal class PaginationItemEllipsis : PaginationItemBase
{
}
