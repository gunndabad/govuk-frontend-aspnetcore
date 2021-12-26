#nullable enable
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class CheckboxesFieldsetContext : FormGroupFieldsetContext
    {
        public CheckboxesFieldsetContext(AttributeDictionary? attributes) :
            base(CheckboxesFieldsetTagHelper.TagName, CheckboxesFieldsetLegendTagHelper.TagName, attributes)
        {
        }
    }
}
