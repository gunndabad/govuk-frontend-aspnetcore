using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class RadiosFieldsetContext : FormGroupFieldsetContext
    {
        public RadiosFieldsetContext(AttributeDictionary? attributes, ModelExpression? aspFor) :
            base(RadiosFieldsetTagHelper.TagName, RadiosFieldsetLegendTagHelper.TagName, attributes, aspFor)
        {
        }
    }
}
