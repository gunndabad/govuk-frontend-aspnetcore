#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class CheckboxesItemContext
    {
        public (IDictionary<string, string> Attributes, IHtmlContent Content)? Conditional { get; private set; }
        public (IDictionary<string, string> Attributes, IHtmlContent Content)? Hint { get; private set; }

        public void SetConditional(IDictionary<string, string> attributes, IHtmlContent content)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            if (Conditional != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    CheckboxesItemConditionalTagHelper.TagName,
                    CheckboxesItemTagHelper.TagName);
            }

            Conditional = (attributes, content);
        }

        public void SetHint(IDictionary<string, string> attributes, IHtmlContent content)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            if (Hint != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    CheckboxesItemHintTagHelper.TagName,
                    CheckboxesItemTagHelper.TagName);
            }

            if (Conditional != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    CheckboxesItemHintTagHelper.TagName,
                    CheckboxesItemConditionalTagHelper.TagName);
            }

            Hint = (attributes, content);
        }
    }
}
