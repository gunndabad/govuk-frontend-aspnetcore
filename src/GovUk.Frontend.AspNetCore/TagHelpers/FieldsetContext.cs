using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using GovUk.Frontend.AspNetCore.ComponentGeneration;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class FieldsetContext
{
    internal record LegendInfo(bool IsPageHeading, ImmutableDictionary<string, string?> Attributes, string Html);

    // internal for testing
    internal LegendInfo? Legend;

    public FieldsetOptionsLegend GetLegendOptions()
    {
        ThrowIfNotComplete();

        var attributes = Legend.Attributes
            .Remove("class", out var classes);

        return new FieldsetOptionsLegend()
        {
            Text = null,
            Html = Legend.Html,
            IsPageHeading = Legend.IsPageHeading,
            Classes = classes,
            Attributes = attributes
        };
    }

    public void SetLegend(
        bool isPageHeading,
        ImmutableDictionary<string, string?> attributes,
        string html)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(html);

        if (Legend != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                FieldsetLegendTagHelper.TagName,
                FieldsetTagHelper.TagName);
        }

        Legend = new LegendInfo(isPageHeading, attributes, html);
    }

    [MemberNotNull(nameof(Legend))]
    private void ThrowIfNotComplete()
    {
        if (Legend == null)
        {
            throw ExceptionHelper.AChildElementMustBeProvided(FieldsetLegendTagHelper.TagName);
        }
    }
}
