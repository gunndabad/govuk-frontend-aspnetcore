using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using GovUk.Frontend.AspNetCore.ComponentGeneration;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class PhaseBannerContext
{
    internal record TagInfo(ImmutableDictionary<string, string?> Attributes, string Html);

    // internal for testing
    internal TagInfo? Tag;

    public TagOptions GetTagOptions()
    {
        ThrowIfIncomplete();

        return new TagOptions()
        {
            Text = null,
            Html = Tag.Html,
            Attributes = Tag.Attributes.Remove("class", out var classes),
            Classes = classes
        };
    }

    public void SetTag(ImmutableDictionary<string, string?> attributes, string html)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(html);

        if (Tag != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                PhaseBannerTagTagHelper.TagName, PhaseBannerTagHelper.TagName);
        }

        Tag = new TagInfo(attributes, html);
    }

    [MemberNotNull(nameof(Tag))]
    private void ThrowIfIncomplete()
    {
        if (Tag == null)
        {
            throw ExceptionHelper.AChildElementMustBeProvided(PhaseBannerTagTagHelper.TagName);
        }
    }
}
