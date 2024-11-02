using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class DateInputFieldsetContext : FormGroupFieldsetContext2
{
    public DateInputFieldsetContext(ImmutableDictionary<string, string?> attributes, ModelExpression? @for, string? describedBy) :
        base(DateInputFieldsetTagHelper.TagName, DateInputFieldsetLegendTagHelper.TagName, attributes, @for, describedBy)
    {
    }
}
