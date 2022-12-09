using System;
using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class RadiosContext : FormGroupContext
    {
        private bool _fieldsetIsOpen;
        private readonly List<RadiosItemBase> _items = new List<RadiosItemBase>();

        public RadiosContext(string? name, ModelExpression? aspFor)
        {
            Name = name;
            AspFor = aspFor;
        }

        public string? Name { get; }

        public ModelExpression? AspFor { get; }

        public IReadOnlyCollection<RadiosItemBase> Items => _items;

        public FormGroupFieldsetContext? Fieldset { get; private set; }

        protected override string ErrorMessageTagName => RadiosTagHelper.ErrorMessageTagName;

        protected string FieldsetTagName => RadiosFieldsetTagHelper.TagName;

        protected string ItemTagName => RadiosItemTagHelper.TagName;

        protected override string HintTagName => RadiosTagHelper.HintTagName;

        protected override string LabelTagName => throw new NotSupportedException();

        protected override string RootTagName => RadiosTagHelper.TagName;

        public void AddItem(RadiosItemBase item)
        {
            Guard.ArgumentNotNull(nameof(item), item);

            if (Fieldset != null)
            {
                throw new InvalidOperationException($"<{ItemTagName}> must be inside <{FieldsetTagName}>.");
            }

            _items.Add(item);
        }

        public void OpenFieldset()
        {
            if (_fieldsetIsOpen)
            {
                throw new InvalidOperationException($"<{FieldsetTagName}> cannot be nested inside another <{FieldsetTagName}>.");
            }

            if (Fieldset != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(FieldsetTagName, RootTagName);
            }

            if (Items.Count > 0 || Hint != null || ErrorMessage != null)
            {
                throw new InvalidOperationException($"<{FieldsetTagName}> must be the only direct child of the <{RootTagName}>.");
            }

            _fieldsetIsOpen = true;
        }

        public void CloseFieldset(RadiosFieldsetContext fieldsetContext)
        {
            if (!_fieldsetIsOpen)
            {
                throw new InvalidOperationException("Fieldset has not been opened.");
            }

            _fieldsetIsOpen = false;
            Fieldset = fieldsetContext;
        }

        public override void SetErrorMessage(
            string? visuallyHiddenText,
            AttributeDictionary? attributes,
            IHtmlContent? content)
        {
            if (Fieldset != null)
            {
                throw new InvalidOperationException($"<{ErrorMessageTagName}> must be inside <{FieldsetTagName}>.");
            }

            if (Items.Count > 0)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(ErrorMessageTagName, ItemTagName);
            }

            base.SetErrorMessage(visuallyHiddenText, attributes, content);
        }

        public override void SetHint(AttributeDictionary? attributes, IHtmlContent? content)
        {
            if (Fieldset != null)
            {
                throw new InvalidOperationException($"<{HintTagName}> must be inside <{FieldsetTagName}>.");
            }

            if (Items.Count > 0)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(HintTagName, ItemTagName);
            }

            base.SetHint(attributes, content);
        }

        public override void SetLabel(
            bool isPageHeading,
            AttributeDictionary? attributes,
            IHtmlContent? content)
        {
            throw new NotSupportedException();
        }
    }
}
