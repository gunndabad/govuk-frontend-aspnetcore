using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class SummaryCardContext
    {
        public (AttributeDictionary Attributes, IHtmlContent Content)? Title { get; private set; }
        public int HeadingLevel { get; internal set; }

        private readonly List<SummaryCardAction> _actions = new();

        public IReadOnlyList<SummaryCardAction> Actions => _actions;

        public AttributeDictionary ActionsAttributes { get; private set; }

        public void SetTitle(AttributeDictionary attributes, IHtmlContent content)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            if (Title != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    SummaryCardTitleTagHelper.TagName,
                    SummaryCardTagHelper.TagName);
            }

            Title = (attributes, content);
        }

        public void AddAction(SummaryCardAction action)
        {
            Guard.ArgumentNotNull(nameof(action), action);

            _actions.Add(action);
        }

        public void SetActionsAttributes(AttributeDictionary attributes)
        {
            Guard.ArgumentNotNull(nameof(attributes), attributes);

            if (ActionsAttributes != null)
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

            ActionsAttributes = attributes;
        }
    }
}
