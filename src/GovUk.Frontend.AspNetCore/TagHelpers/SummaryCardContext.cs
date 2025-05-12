using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class SummaryCardContext
{
    private readonly List<SummaryListAction> _actions = new();

    public (IHtmlContent Content, int? HeadingLevel, AttributeDictionary Attributes)? Title { get; private set; }

    public IReadOnlyList<SummaryListAction> Actions => _actions;

    public AttributeDictionary? ActionsAttributes { get; private set; }

    public IHtmlContent? SummaryList { get; internal set; }

    public void SetTitle(IHtmlContent content, int? headingLevel, AttributeDictionary attributes)
    {
        Guard.ArgumentNotNull(nameof(content), content);
        Guard.ArgumentNotNull(nameof(attributes), attributes);

        if (Title is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                SummaryCardTitleTagHelper.TagName,
                SummaryCardTagHelper.TagName);
        }

        if (ActionsAttributes is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                SummaryCardTitleTagHelper.TagName,
                SummaryCardActionsTagHelper.TagName);
        }

        if (SummaryList is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                SummaryCardTitleTagHelper.TagName,
                SummaryListTagHelper.TagName);
        }

        Title = (content, headingLevel, attributes);
    }

    public void AddAction(SummaryListAction action)
    {
        Guard.ArgumentNotNull(nameof(action), action);

        if (SummaryList is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                SummaryCardActionTagHelper.TagName,
                SummaryListTagHelper.TagName);
        }

        _actions.Add(action);
    }

    public void SetActionsAttributes(AttributeDictionary attributes)
    {
        Guard.ArgumentNotNull(nameof(attributes), attributes);

        if (ActionsAttributes is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                SummaryCardActionsTagHelper.TagName,
                SummaryCardTagHelper.TagName);
        }

        if (_actions.Count > 0)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                SummaryCardActionsTagHelper.TagName,
                SummaryCardActionTagHelper.TagName);
        }

        if (SummaryList is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                SummaryCardActionsTagHelper.TagName,
                SummaryListTagHelper.TagName);
        }

        ActionsAttributes = attributes;
    }

    public void SetSummaryList(IHtmlContent content)
    {
        Guard.ArgumentNotNull(nameof(content), content);

        if (SummaryList is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                SummaryListTagHelper.TagName,
                SummaryCardTagHelper.TagName);
        }

        SummaryList = content;
    }

    public void ThrowIfNotComplete()
    {
        if (SummaryList is null)
        {
            throw ExceptionHelper.AChildElementMustBeProvided(SummaryListTagHelper.TagName);
        }
    }
}
