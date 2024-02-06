using System;
using System.Collections.Immutable;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class DateInputItemContext
{
    internal record LabelInfo(ImmutableDictionary<string, string?> Attributes, string? Html);

    private readonly string _itemTagName;
    private readonly string _labelTagName;

    // internal for testing
    internal LabelInfo? Label;

    public DateInputItemContext(string itemTagName, string labelTagName)
    {
        ArgumentNullException.ThrowIfNull(itemTagName);
        ArgumentNullException.ThrowIfNull(labelTagName);
        _itemTagName = itemTagName;
        _labelTagName = labelTagName;
    }

    public (ImmutableDictionary<string, string?> Attributes, string? Html)? GetLabelOptions() =>
        Label is not null ? (Label.Attributes, Label.Html) : null;

    public void SetLabel(ImmutableDictionary<string, string?> attributes, string? html)
    {
        ArgumentNullException.ThrowIfNull(attributes);

        if (Label != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(_labelTagName, _itemTagName);
        }

        Label = new LabelInfo(attributes, html);
    }
}
