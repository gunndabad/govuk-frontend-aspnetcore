using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public class SelectItem
    {
        public string? Value { get; set; }
        [DisallowNull]
        public IHtmlContent? Content { get; set; }
        public bool Selected { get; set; }
        public bool Disabled { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }
}
