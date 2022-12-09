using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class CheckboxesItemContext
    {
        public (AttributeDictionary Attributes, IHtmlContent Content)? Conditional { get; private set; }
        public (AttributeDictionary Attributes, IHtmlContent Content)? Hint { get; private set; }

        public void SetConditional(AttributeDictionary attributes, IHtmlContent content)
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

        public void SetHint(AttributeDictionary attributes, IHtmlContent content)
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
