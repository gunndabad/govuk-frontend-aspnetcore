#nullable enable
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public class SelectItem
    {
        public string? Value { get; set; }
        [DisallowNull]
        public IHtmlContent? Content { get; set; }
        public bool Selected { get; set; }
        public bool Disabled { get; set; }
        public IDictionary<string, string>? Attributes { get; set; }
    }
}
