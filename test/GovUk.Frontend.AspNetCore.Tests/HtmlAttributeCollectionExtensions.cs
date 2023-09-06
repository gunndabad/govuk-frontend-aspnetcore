using System.Collections.Generic;
using HtmlAgilityPack;

namespace GovUk.Frontend.AspNetCore.Tests
{
    public static class HtmlAttributeCollectionExtensions
    {
        public static IReadOnlyCollection<string> GetCssClasses(this HtmlNode htmlNode) =>
            GetCssClasses(htmlNode.Attributes);

        public static IReadOnlyCollection<string> GetCssClasses(this HtmlAttributeCollection htmlAttributeCollection) =>
            htmlAttributeCollection["class"].Value.Split(" ");
    }
}
