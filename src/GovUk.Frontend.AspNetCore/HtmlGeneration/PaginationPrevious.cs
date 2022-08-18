#nullable enable
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public class PaginationPrevious
    {
        [DisallowNull]
        public string? Href { get; set; }
        public AttributeDictionary? Attributes { get; set; }
        public AttributeDictionary? LinkAttributes { get; set; }
        public string? LabelText { get; set; }
        public string? Text { get; set; }
    }
}
