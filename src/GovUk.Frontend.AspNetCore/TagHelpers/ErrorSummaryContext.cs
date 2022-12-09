using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class ErrorSummaryContext
    {
        private readonly List<ErrorSummaryItem> _items;

        public ErrorSummaryContext()
        {
            _items = new List<ErrorSummaryItem>();
        }

        public IReadOnlyCollection<ErrorSummaryItem> Items => _items;

        public (AttributeDictionary Attributes, IHtmlContent Content)? Description { get; private set; }

        public (AttributeDictionary Attributes, IHtmlContent Content)? Title { get; private set; }

        public void AddItem(ErrorSummaryItem item)
        {
            Guard.ArgumentNotNull(nameof(item), item);

            _items.Add(item);
        }

        public void SetDescription(AttributeDictionary attributes, IHtmlContent content)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            if (Description != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    ErrorSummaryDescriptionTagHelper.TagName,
                    ErrorSummaryTagHelper.TagName);
            }

            Description = (attributes, content);
        }

        public void SetTitle(AttributeDictionary attributes, IHtmlContent content)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            if (Title != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    ErrorSummaryTitleTagHelper.TagName,
                    ErrorSummaryTagHelper.TagName);
            }

            Title = (attributes, content);
        }
    }
}
