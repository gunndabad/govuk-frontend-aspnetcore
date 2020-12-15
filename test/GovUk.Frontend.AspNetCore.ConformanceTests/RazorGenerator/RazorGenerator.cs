using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class RazorGenerator
    {
        private static void MergeAttributesWithPrefix(
            TagBuilder tagBuilder,
            string prefix,
            IDictionary<string, string> attributes)
        {
            foreach (var attr in attributes.OrEmpty())
            {
                tagBuilder.MergeAttribute($"{prefix}-{attr.Key}", attr.Value);
            }
        }
    }
}
