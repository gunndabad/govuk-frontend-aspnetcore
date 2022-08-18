#nullable enable
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public abstract class PaginationItemBase
    {
        private protected PaginationItemBase() { }

        public AttributeDictionary? Attributes { get; set; }
    }

    public class PaginationItem : PaginationItemBase
    {
        [DisallowNull]
        public string? Number { get; set; }
        [DisallowNull]
        public string? Href { get; set; }
        public bool IsCurrent { get; set; }
        public string? VisuallyHiddenText { get; set; }
    }

    public class PaginationItemEllipsis : PaginationItemBase
    {
    }
}
