#nullable enable
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class DateInputItemContext
    {
        private readonly string _itemTagName;
        private readonly string _labelTagName;

        public DateInputItemContext(string itemTagName, string labelTagName)
        {
            _itemTagName = Guard.ArgumentNotNull(nameof(itemTagName), itemTagName);
            _labelTagName = Guard.ArgumentNotNull(nameof(labelTagName), labelTagName);
        }

        public (IHtmlContent Content, AttributeDictionary Attributes)? Label { get; private set; }

        public void SetLabel(IHtmlContent content, AttributeDictionary attributes)
        {
            Guard.ArgumentNotNull(nameof(content), content);
            Guard.ArgumentNotNull(nameof(attributes), attributes);

            if (Label != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(_labelTagName, _itemTagName);
            }

            Label = (content, attributes);
        }
    }
}
