#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class InputContext : FormGroupContext
    {
        public (IDictionary<string, string>? Attributes, IHtmlContent Content)? Prefix { get; private set; }

        public (IDictionary<string, string>? Attributes, IHtmlContent Content)? Suffix { get; private set; }

        protected override string ErrorMessageTagName => InputTagHelper.ErrorMessageTagName;

        protected override string HintTagName => InputTagHelper.HintTagName;

        protected override string LabelTagName => InputTagHelper.LabelTagName;

        protected override string RootTagName => InputTagHelper.TagName;

        public override void SetErrorMessage(
            string? visuallyHiddenText,
            IDictionary<string, string>? attributes,
            IHtmlContent? content)
        {
            if (Prefix != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    ErrorMessageTagName,
                    InputPrefixTagHelper.TagName);
            }

            if (Suffix != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    ErrorMessageTagName,
                    InputSuffixTagHelper.TagName);
            }

            base.SetErrorMessage(visuallyHiddenText, attributes, content);
        }

        public override void SetHint(IDictionary<string, string>? attributes, IHtmlContent? content)
        {
            if (Prefix != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    HintTagName,
                    InputPrefixTagHelper.TagName);
            }

            if (Suffix != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    HintTagName,
                    InputSuffixTagHelper.TagName);
            }

            base.SetHint(attributes, content);
        }

        public override void SetLabel(bool isPageHeading, IDictionary<string, string>? attributes, IHtmlContent? content)
        {
            if (Prefix != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    LabelTagName,
                    InputPrefixTagHelper.TagName);
            }

            if (Suffix != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    LabelTagName,
                    InputSuffixTagHelper.TagName);
            }

            base.SetLabel(isPageHeading, attributes, content);
        }

        public void SetPrefix(IDictionary<string, string>? attributes, IHtmlContent content)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            if (Prefix != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    InputPrefixTagHelper.TagName,
                    InputTagHelper.TagName);
            }

            if (Suffix != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    InputPrefixTagHelper.TagName,
                    InputSuffixTagHelper.TagName);
            }

            Prefix = (attributes, content);
        }

        public void SetSuffix(IDictionary<string, string>? attributes, IHtmlContent content)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            if (Suffix != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    InputSuffixTagHelper.TagName,
                    InputTagHelper.TagName);
            }

            Suffix = (attributes, content);
        }
    }
}
