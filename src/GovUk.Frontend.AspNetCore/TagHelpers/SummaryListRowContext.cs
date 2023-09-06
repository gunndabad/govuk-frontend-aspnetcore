using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class SummaryListRowContext
    {
        private readonly List<SummaryListAction> _actions;

        public SummaryListRowContext()
        {
            _actions = new List<SummaryListAction>();
        }

        public IReadOnlyList<SummaryListAction> Actions => _actions;

        public AttributeDictionary? ActionsAttributes { get; private set; }

        public (AttributeDictionary Attributes, IHtmlContent Content)? Key { get; private set; }

        public (AttributeDictionary Attributes, IHtmlContent Content)? Value { get; private set; }

        public void AddAction(SummaryListAction action)
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
                    SummaryListRowActionsTagHelper.TagName,
                    SummaryListRowTagHelper.TagName);
            }

            if (_actions.Count > 0)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    SummaryListRowActionsTagHelper.TagName,
                    SummaryListRowActionTagHelper.TagName);
            }

            ActionsAttributes = attributes;
        }

        public void SetKey(AttributeDictionary attributes, IHtmlContent content)
        {
            Guard.ArgumentNotNull(nameof(attributes), attributes);
            Guard.ArgumentNotNull(nameof(content), content);

            if (Key != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    SummaryListRowKeyTagHelper.TagName,
                    SummaryListRowTagHelper.TagName);
            }

            if (Value != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    SummaryListRowKeyTagHelper.TagName,
                    SummaryListRowValueTagHelper.TagName);
            }

            if (ActionsAttributes != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    SummaryListRowKeyTagHelper.TagName,
                    SummaryListRowActionsTagHelper.TagName);
            }

            if (_actions.Count > 0)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    SummaryListRowKeyTagHelper.TagName,
                    SummaryListRowActionTagHelper.TagName);
            }

            Key = (attributes, content);
        }

        public void SetValue(AttributeDictionary attributes, IHtmlContent content)
        {
            Guard.ArgumentNotNull(nameof(attributes), attributes);
            Guard.ArgumentNotNull(nameof(content), content);

            if (Value != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    SummaryListRowValueTagHelper.TagName,
                    SummaryListRowTagHelper.TagName);
            }

            if (ActionsAttributes != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    SummaryListRowValueTagHelper.TagName,
                    SummaryListRowActionsTagHelper.TagName);
            }

            if (_actions.Count > 0)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    SummaryListRowValueTagHelper.TagName,
                    SummaryListRowActionTagHelper.TagName);
            }

            Value = (attributes, content);
        }

        public void ThrowIfIncomplete()
        {
            if (Key == null)
            {
                throw ExceptionHelper.AChildElementMustBeProvided(SummaryListRowKeyTagHelper.TagName);
            }
        }
    }
}
