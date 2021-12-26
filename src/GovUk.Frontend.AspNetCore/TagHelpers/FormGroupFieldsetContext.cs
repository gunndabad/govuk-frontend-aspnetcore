#nullable enable
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal abstract class FormGroupFieldsetContext
    {
        private readonly string _fieldsetTagName;
        private readonly string _legendTagName;

        protected FormGroupFieldsetContext(
            string fieldsetTagName,
            string legendTagName,
            AttributeDictionary? attributes)
        {
            _fieldsetTagName = Guard.ArgumentNotNull(nameof(fieldsetTagName), fieldsetTagName);
            _legendTagName = Guard.ArgumentNotNull(nameof(legendTagName), legendTagName);
            Attributes = attributes;
        }

        public AttributeDictionary? Attributes { get; private set; }

        public (bool IsPageHeading, AttributeDictionary? Attributes, IHtmlContent Content)? Legend { get; private set; }

        public virtual void SetLegend(bool isPageHeading, AttributeDictionary? attributes, IHtmlContent content)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            if (Legend != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(_legendTagName, _fieldsetTagName);
            }

            Legend = (isPageHeading, attributes, content);
        }

        public void ThrowIfNotComplete()
        {
            if (Legend == null)
            {
                throw ExceptionHelper.AChildElementMustBeProvided(_legendTagName);
            }
        }
    }
}
