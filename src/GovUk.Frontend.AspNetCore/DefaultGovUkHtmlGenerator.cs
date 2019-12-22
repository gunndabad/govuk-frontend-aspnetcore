using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore
{
    public class DefaultGovUkHtmlGenerator : IGovUkHtmlGenerator
    {
        private readonly IHtmlGenerator _htmlGenerator;

        public DefaultGovUkHtmlGenerator(IHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator;
        }

        public TagBuilder GenerateErrorMessage(
            ViewContext viewContext,
            ModelExplorer modelExplorer,
            string expression,
            string id,
            string visuallyHiddenText)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException(nameof(viewContext));
            }

            if (modelExplorer == null)
            {
                throw new ArgumentNullException(nameof(modelExplorer));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var validationMessage = _htmlGenerator.GenerateValidationMessage(
                viewContext,
                modelExplorer,
                expression,
                message: null,
                tag: null,
                htmlAttributes: null);
            var content = validationMessage.InnerHtml;

            return GenerateErrorMessage(id, visuallyHiddenText, content);
        }

        public TagBuilder GenerateErrorMessage(
            string id,
            string visuallyHiddenText,
            IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("span");
            tagBuilder.AddCssClass("govuk-error-message");

            if (!string.IsNullOrEmpty(id))
            {
                tagBuilder.Attributes.Add("id", id);
            }

            if (!string.IsNullOrEmpty(visuallyHiddenText))
            {
                var vht = new TagBuilder("span");
                vht.AddCssClass("govuk-visually-hidden");
                vht.InnerHtml.Append(visuallyHiddenText);

                tagBuilder.InnerHtml.AppendHtml(vht);
            }

            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }

        public TagBuilder GenerateHint(IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("span");
            tagBuilder.AddCssClass("govuk-hint");
            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }

        public virtual TagBuilder GenerateTag(IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("strong");
            tagBuilder.AddCssClass("govuk-tag");
            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }
    }
}
