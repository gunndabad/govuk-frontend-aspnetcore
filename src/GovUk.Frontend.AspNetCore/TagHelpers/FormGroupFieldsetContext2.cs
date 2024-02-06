using System;
using System.Collections.Immutable;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal abstract class FormGroupFieldsetContext2
{
    internal record LegendInfo(bool IsPageHeading, ImmutableDictionary<string, string?> Attributes, string? Html);

    private readonly string _fieldsetTagName;
    private readonly string _legendTagName;
    private readonly ModelExpression? _aspFor;

    // internal for testing
    internal LegendInfo? Legend;

    private ImmutableDictionary<string, string?> _attributes;

    protected FormGroupFieldsetContext2(
        string fieldsetTagName,
        string legendTagName,
        ImmutableDictionary<string, string?> attributes,
        ModelExpression? aspFor)
    {
        ArgumentNullException.ThrowIfNull(fieldsetTagName);
        ArgumentNullException.ThrowIfNull(legendTagName);
        ArgumentNullException.ThrowIfNull(attributes);
        _fieldsetTagName = fieldsetTagName;
        _legendTagName = legendTagName;
        _attributes = attributes;
        _aspFor = aspFor;
    }

    public FieldsetOptions GetFieldsetOptions(ModelExpression? aspFor, IModelHelper modelHelper)
    {
        ThrowIfNotComplete();

        var legendHtml = Legend?.Html ??
            modelHelper.GetDisplayName(aspFor!.ModelExplorer, aspFor.Name);

        return new FieldsetOptions()
        {
            DescribedBy = null,
            Legend = new FieldsetOptionsLegend()
            {
                Text = null,
                Html = legendHtml,
                IsPageHeading = Legend?.IsPageHeading,
                Attributes = (Legend?.Attributes ?? ImmutableDictionary<string, string?>.Empty).Remove("class", out var legendClasses),
                Classes = legendClasses
            },
            Role = null,
            Text = null,
            Html = null,
            Attributes = _attributes.Remove("class", out var classes),
            Classes = classes
        };
    }

    public virtual void SetLegend(bool isPageHeading, ImmutableDictionary<string, string?> attributes, string? html)
    {
        if (Legend != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(_legendTagName, _fieldsetTagName);
        }

        Legend = new LegendInfo(isPageHeading, attributes, html);
    }

    public void ThrowIfNotComplete()
    {
        if (Legend == null && _aspFor is null)
        {
            throw ExceptionHelper.AChildElementMustBeProvided(_legendTagName);
        }
    }
}
