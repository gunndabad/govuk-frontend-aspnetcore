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
            IHtmlContent content,
            IDictionary<string, string> attributes)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            var tagBuilder = new TagBuilder(BackLinkElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.AddCssClass("govuk-back-link");
            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }
    }
}
