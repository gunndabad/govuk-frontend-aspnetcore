using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore
{
    public class DefaultGovUkHtmlGenerator : IGovUkHtmlGenerator
    {
        public virtual TagBuilder GenerateErrorMessage(string visuallyHiddenText, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("span");
            tagBuilder.AddCssClass("govuk-error-message");

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

        public virtual TagBuilder GenerateHint(IHtmlContent content)
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

        public virtual TagBuilder GenerateLabel(string @for, bool isPageHeading, IHtmlContent content)
        {
            if (@for == null)
            {
                throw new ArgumentNullException(nameof(@for));
            }

            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("label");
            tagBuilder.AddCssClass("govuk-label");
            tagBuilder.Attributes.Add("for", @for);
            tagBuilder.InnerHtml.AppendHtml(content);

            if (isPageHeading)
            {
                var heading = new TagBuilder("h1");
                heading.AddCssClass("govuk-label-wrapper");

                heading.InnerHtml.AppendHtml(tagBuilder);

                return heading;
            }
            else
            {
                return tagBuilder;
            }
        }

        public virtual TagBuilder GeneratePhaseBanner(string tag, IHtmlContent content)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("govuk-phase-banner");

            var contentTagBuilder = new TagBuilder("p");
            contentTagBuilder.AddCssClass("govuk-phase-banner__content");

            var tagTagBuilder = GenerateTag(new StringHtmlContent(tag));
            tagTagBuilder.AddCssClass("govuk-phase-banner__content__tag");
            contentTagBuilder.InnerHtml.AppendHtml(tagTagBuilder);

            var textTagBuilder = new TagBuilder("span");
            textTagBuilder.AddCssClass("govuk-phase-banner__text");
            textTagBuilder.InnerHtml.AppendHtml(content);
            contentTagBuilder.InnerHtml.AppendHtml(textTagBuilder);

            tagBuilder.InnerHtml.AppendHtml(contentTagBuilder);

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
