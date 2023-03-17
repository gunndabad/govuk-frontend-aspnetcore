using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public class SummaryCardActions
    {
        public IReadOnlyList<SummaryCardAction> Items { get; set; }
        public AttributeDictionary Attributes { get; set; }
    }
}
