#nullable enable
using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class SelectContext : FormGroupContext
    {
        private readonly List<SelectItem> _items;

        public SelectContext(ModelExpression? aspFor)
        {
            _items = new List<SelectItem>();
            AspFor = aspFor;
        }

        public ModelExpression? AspFor { get; }

        public bool HaveModelExpression => AspFor != null;

        public IReadOnlyCollection<SelectItem> Items => _items;

        protected override string ErrorMessageTagName => SelectTagHelper.ErrorMessageTagName;

        protected override string HintTagName => SelectTagHelper.HintTagName;

        protected override string LabelTagName => SelectTagHelper.LabelTagName;

        protected override string RootTagName => SelectTagHelper.TagName;

        public void AddItem(SelectItem item)
        {
            Guard.ArgumentNotNull(nameof(item), item);

            _items.Add(item);
        }

        public override void SetErrorMessage(
            string? visuallyHiddenText,
            AttributeDictionary? attributes,
            IHtmlContent? content)
        {
            if (_items.Count != 0)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(ErrorMessageTagName, SelectItemTagHelper.TagName);
            }

            base.SetErrorMessage(visuallyHiddenText, attributes, content);
        }

        public override void SetHint(AttributeDictionary? attributes, IHtmlContent? content)
        {
            if (_items.Count != 0)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(HintTagName, SelectItemTagHelper.TagName);
            }

            base.SetHint(attributes, content);
        }

        public override void SetLabel(bool isPageHeading, AttributeDictionary? attributes, IHtmlContent? content)
        {
            if (_items.Count != 0)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(LabelTagName, SelectItemTagHelper.TagName);
            }

            base.SetLabel(isPageHeading, attributes, content);
        }
    }
}
