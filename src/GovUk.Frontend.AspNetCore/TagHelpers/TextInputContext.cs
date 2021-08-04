#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class TextInputContext : FormGroupContext
    {
        public (IDictionary<string, string>? Attributes, IHtmlContent Content)? Prefix { get; private set; }

        public (IDictionary<string, string>? Attributes, IHtmlContent Content)? Suffix { get; private set; }

        protected override string ErrorMessageTagName => TextInputTagHelper.ErrorMessageTagName;

        protected override string HintTagName => TextInputTagHelper.HintTagName;

        protected override string LabelTagName => TextInputTagHelper.LabelTagName;

        protected override string RootTagName => TextInputTagHelper.TagName;

        public override void SetErrorMessage(
            string? visuallyHiddenText,
            IDictionary<string, string>? attributes,
            IHtmlContent? content)
        {
            if (Prefix != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    ErrorMessageTagName,
                    TextInputPrefixTagHelper.TagName);
            }

            if (Suffix != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    ErrorMessageTagName,
                    TextInputSuffixTagHelper.TagName);
            }

            base.SetErrorMessage(visuallyHiddenText, attributes, content);
        }

        public override void SetHint(IDictionary<string, string>? attributes, IHtmlContent? content)
        {
            if (Prefix != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    HintTagName,
                    TextInputPrefixTagHelper.TagName);
            }

            if (Suffix != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    HintTagName,
                    TextInputSuffixTagHelper.TagName);
            }

            base.SetHint(attributes, content);
        }

        public override void SetLabel(bool isPageHeading, IDictionary<string, string>? attributes, IHtmlContent? content)
        {
            if (Prefix != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    LabelTagName,
                    TextInputPrefixTagHelper.TagName);
            }

            if (Suffix != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    LabelTagName,
                    TextInputSuffixTagHelper.TagName);
            }

            base.SetLabel(isPageHeading, attributes, content);
        }

        public void SetPrefix(IDictionary<string, string>? attributes, IHtmlContent content)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            if (Prefix != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TextInputPrefixTagHelper.TagName,
                    TextInputTagHelper.TagName);
            }

            if (Suffix != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    TextInputPrefixTagHelper.TagName,
                    TextInputSuffixTagHelper.TagName);
            }

            Prefix = (attributes, content);
        }

        public void SetSuffix(IDictionary<string, string>? attributes, IHtmlContent content)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            if (Suffix != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TextInputSuffixTagHelper.TagName,
                    TextInputTagHelper.TagName);
            }

            Suffix = (attributes, content);
        }
    }
}
