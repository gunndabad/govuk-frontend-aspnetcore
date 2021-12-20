#nullable enable
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class CheckboxesFieldsetContext : FormGroupFieldsetContext
    {
        public CheckboxesFieldsetContext(IDictionary<string, string>? attributes) :
            base(CheckboxesFieldsetTagHelper.TagName, CheckboxesFieldsetLegendTagHelper.TagName, attributes)
        {
        }
    }
}
