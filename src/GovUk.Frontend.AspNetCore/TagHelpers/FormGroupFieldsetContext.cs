#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal abstract class FormGroupFieldsetContext
    {
        private readonly string _fieldsetTagName;
        private readonly string _legendTagName;

        protected FormGroupFieldsetContext(
            string fieldsetTagName,
            string legendTagName,
            IDictionary<string, string>? attributes)
        {
            _fieldsetTagName = Guard.ArgumentNotNull(nameof(fieldsetTagName), fieldsetTagName);
            _legendTagName = Guard.ArgumentNotNull(nameof(legendTagName), legendTagName);
            Attributes = attributes;
        }

        public IDictionary<string, string>? Attributes { get; private set; }

        public (bool IsPageHeading, IDictionary<string, string>? Attributes, IHtmlContent Content)? Legend { get; private set; }

        public virtual void SetLegend(bool isPageHeading, IDictionary<string, string>? attributes, IHtmlContent content)
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
