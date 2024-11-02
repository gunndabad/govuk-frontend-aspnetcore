using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class DetailsContext
{
    internal record SummaryInfo(ImmutableDictionary<string, string?> Attributes, string Html);

    internal record TextInfo(ImmutableDictionary<string, string?> Attributes, string Html);

    // internal for testing
    internal SummaryInfo? Summary;
    internal TextInfo? Text;

    public (ImmutableDictionary<string, string?> Attributes, string Html) GetSummaryOptions()
    {
        ThrowIfNotComplete();

        return (Summary.Attributes, Summary.Html);
    }

    public (ImmutableDictionary<string, string?> Attributes, string Html) GetTextOptions()
    {
        ThrowIfNotComplete();

        return (Text.Attributes, Text.Html);
    }

    public void SetSummary(ImmutableDictionary<string, string?> attributes, string html)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(html);

        if (Summary != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                DetailsSummaryTagHelper.TagName,
                DetailsTagHelper.TagName
            );
        }

        if (Text != null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                DetailsSummaryTagHelper.TagName,
                DetailsTextTagHelper.TagName
            );
        }

        Summary = new SummaryInfo(attributes, html);
    }

    public void SetText(ImmutableDictionary<string, string?> attributes, string html)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(html);

        if (Text != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(DetailsTextTagHelper.TagName, DetailsTagHelper.TagName);
        }

        Text = new TextInfo(attributes, html);
    }

    [MemberNotNull(nameof(Summary), nameof(Text))]
    private void ThrowIfNotComplete()
    {
        if (Summary == null)
        {
            throw ExceptionHelper.AChildElementMustBeProvided(DetailsSummaryTagHelper.TagName);
        }

        if (Text == null)
        {
            throw ExceptionHelper.AChildElementMustBeProvided(DetailsTextTagHelper.TagName);
        }
    }
}
