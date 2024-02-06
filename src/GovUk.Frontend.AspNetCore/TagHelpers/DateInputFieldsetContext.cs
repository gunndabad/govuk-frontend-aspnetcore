using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class DateInputFieldsetContext : FormGroupFieldsetContext2
{
    public DateInputFieldsetContext(ImmutableDictionary<string, string?> attributes, ModelExpression? aspFor) :
        base(DateInputFieldsetTagHelper.TagName, DateInputFieldsetLegendTagHelper.TagName, attributes, aspFor)
    {
    }
}
