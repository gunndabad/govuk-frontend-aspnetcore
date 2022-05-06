#nullable enable
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class DateInputFieldsetContext : FormGroupFieldsetContext
    {
        public DateInputFieldsetContext(AttributeDictionary? attributes) :
            base(DateInputFieldsetTagHelper.TagName, DateInputFieldsetLegendTagHelper.TagName, attributes)
        {
        }
    }
}
