#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string BackLinkDefaultContent = "Back";
        internal const string BackLinkElement = "a";

        public virtual TagBuilder GenerateBackLink(
            string href,
            IHtmlContent content,
            IDictionary<string, string> attributes)
        {
            if (href == null)
            {
                throw new ArgumentNullException(nameof(href));
            }

            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder(BackLinkElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.AddCssClass("govuk-back-link");
            tagBuilder.Attributes.Add("href", href);
            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }
    }
}
