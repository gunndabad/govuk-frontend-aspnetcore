using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal class PaginationNext
{
    public string? Href { get; set; }
    public AttributeDictionary? Attributes { get; set; }
    public AttributeDictionary? LinkAttributes { get; set; }
    public string? LabelText { get; set; }
    public IHtmlContent? Text { get; set; }
}
