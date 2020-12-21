#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class FormGroupFieldsetContext
    {
        private readonly string _fieldsetTagName;
        private readonly string _legendTagName;

        public FormGroupFieldsetContext(
            string fieldsetTagName,
            string legendTagName,
            IDictionary<string, string>? attributes,
            string? describedBy)
        {
            _fieldsetTagName = Guard.ArgumentNotNull(nameof(fieldsetTagName), fieldsetTagName);
            _legendTagName = Guard.ArgumentNotNull(nameof(legendTagName), legendTagName);
            Attributes = attributes;
            DescribedBy = describedBy;
        }

        public IDictionary<string, string>? Attributes { get; set; }

        public string? DescribedBy { get; }

        public (bool IsPageHeading, IDictionary<string, string>? Attributes, IHtmlContent Content)? Legend { get; private set; }

        public void SetLegend(bool isPageHeading, IDictionary<string, string>? attributes, IHtmlContent content)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            if (Legend != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(_legendTagName, _fieldsetTagName);
            }

            Legend = (isPageHeading, attributes, content);
        }
    }
}
