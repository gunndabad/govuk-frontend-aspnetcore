#nullable enable
using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class SummaryListRowContext
    {
        private readonly List<SummaryListRowAction> _actions;

        public SummaryListRowContext()
        {
            _actions = new List<SummaryListRowAction>();
        }

        public IReadOnlyList<SummaryListRowAction> Actions => _actions;

        public IDictionary<string, string>? ActionsAttributes { get; private set; }

        public (IHtmlContent Content, IDictionary<string, string> Attributes)? Key { get; private set; }

        public (IHtmlContent Content, IDictionary<string, string> Attributes)? Value { get; private set; }

        public void AddAction(SummaryListRowAction action)
        {
            Guard.ArgumentNotNull(nameof(action), action);

            _actions.Add(action);
        }

        public void SetActionsAttributes(IDictionary<string, string> attributes)
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

        public void SetKey(IHtmlContent content, IDictionary<string, string> attributes)
        {
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

            Key = (content, attributes);
        }

        public void SetValue(IHtmlContent content, IDictionary<string, string> attributes)
        {
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

            Value = (content, attributes);
        }

        public void ThrowIfIncomplete()
        {
            if (Key == null)
            {
                throw ExceptionHelper.AChildElementMustBeProvided(SummaryListRowKeyTagHelper.TagName);
            }

            if (Value == null)
            {
                throw ExceptionHelper.AChildElementMustBeProvided(SummaryListRowValueTagHelper.TagName);
            }
        }
    }
}
