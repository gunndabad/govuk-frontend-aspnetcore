using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class AccordionItemContext
{
    public (AttributeDictionary Attributes, IHtmlContent Content)? Heading { get; private set; }
    public (AttributeDictionary Attributes, IHtmlContent Content)? Summary { get; private set; }
    public (AttributeDictionary Attributes, IHtmlContent Content)? Content { get; private set; }

    public void SetHeading(AttributeDictionary attributes, IHtmlContent content)
    {
        Guard.ArgumentNotNull(nameof(attributes), attributes);
        Guard.ArgumentNotNull(nameof(content), content);

        if (Heading != null)
        {
            throw new InvalidOperationException(
                $"Only one <{AccordionItemHeadingTagHelper.TagName}> is permitted for each <{AccordionItemTagHelper.TagName}>.");
        }

        if (Summary != null)
        {
            throw new InvalidOperationException(
                $"<{AccordionItemHeadingTagHelper.TagName}> must be specified before <{AccordionItemSummaryTagHelper.TagName}>.");
        }

        if (Content != null)
        {
            throw new InvalidOperationException(
                $"<{AccordionItemHeadingTagHelper.TagName}> must be specified before <{AccordionItemContentTagHelper.TagName}>.");
        }

        Heading = (attributes, content);
    }

    public void SetSummary(AttributeDictionary attributes, IHtmlContent content)
    {
        Guard.ArgumentNotNull(nameof(attributes), attributes);
        Guard.ArgumentNotNull(nameof(content), content);

        if (Summary != null)
        {
            throw new InvalidOperationException(
                $"Only one <{AccordionItemSummaryTagHelper.TagName}> is permitted for each <{AccordionItemTagHelper.TagName}>.");
        }

        if (Content != null)
        {
            throw new InvalidOperationException(
                $"<{AccordionItemSummaryTagHelper.TagName}> must be specified before <{AccordionItemContentTagHelper.TagName}>.");
        }

        Summary = (attributes, content);
    }

    public void SetContent(AttributeDictionary attributes, IHtmlContent content)
    {
        Guard.ArgumentNotNull(nameof(attributes), attributes);
        Guard.ArgumentNotNull(nameof(content), content);

        if (Content != null)
        {
            throw new InvalidOperationException(
                $"Only one <{AccordionItemContentTagHelper.TagName}> is permitted for each <{AccordionItemTagHelper.TagName}>.");
        }

        Content = (attributes, content);
    }

    public void ThrowIfIncomplete()
    {
        if (Heading == null)
        {
            throw ExceptionHelper.AChildElementMustBeProvided(AccordionItemHeadingTagHelper.TagName);
        }

        if (Content == null)
        {
            throw ExceptionHelper.AChildElementMustBeProvided(AccordionItemContentTagHelper.TagName);
        }
    }
}
