using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore
{
    public class DefaultGovUkHtmlGenerator : IGovUkHtmlGenerator
    {
        public const string DefaultErrorMessageVisuallyHiddenText = "Error";

        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHtmlGenerator _innerGenerator;

        public DefaultGovUkHtmlGenerator(IUrlHelperFactory urlHelperFactory, IHtmlGenerator innerGenerator)
        {
            _urlHelperFactory = urlHelperFactory ?? throw new ArgumentNullException(nameof(urlHelperFactory));
            _innerGenerator = innerGenerator ?? throw new ArgumentNullException(nameof(innerGenerator));
        }

        public TagBuilder GenerateLink(string href)
        {
            if (href == null)
            {
                throw new ArgumentNullException(nameof(href));
            }

            var tagBuilder = new TagBuilder("a");
            tagBuilder.Attributes.Add("href", href);

            return tagBuilder;
        }

        public TagBuilder GenerateActionLink(
            ViewContext viewContext,
            string action,
            string controller,
            object values,
            string protocol,
            string host,
            string fragment)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(viewContext);
            var href = urlHelper.Action(action, controller, values, protocol, host, fragment);

            return GenerateLink(href);
        }

        public TagBuilder GeneratePageLink(
            ViewContext viewContext,
            string pageName,
            string pageHandler,
            object values,
            string protocol,
            string host,
            string fragment)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(viewContext);
            var href = urlHelper.Page(pageName, pageHandler, values, protocol, host, fragment);

            return GenerateLink(href);
        }

        public TagBuilder GenerateRouteLink(
            ViewContext viewContext,
            string routeName,
            object values,
            string protocol,
            string host,
            string fragment)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(viewContext);
            var href = urlHelper.RouteUrl(routeName, values, protocol, host, fragment);

            return GenerateLink(href);
        }

        public virtual TagBuilder GenerateBreadcrumbs(IEnumerable<IHtmlContent> items, IHtmlContent currentPageItem)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("govuk-breadcrumbs");

            var ol = new TagBuilder("ol");
            ol.AddCssClass("govuk-breadcrumbs__list");

            foreach (var item in items)
            {
                var li = new TagBuilder("li");
                li.AddCssClass("govuk-breadcrumbs__list-item");

                if (item == currentPageItem)
                {
                    li.Attributes.Add("aria-current", "page");
                }

                li.InnerHtml.AppendHtml(item);

                ol.InnerHtml.AppendHtml(li);
            }

            tagBuilder.InnerHtml.AppendHtml(ol);

            return tagBuilder;
        }

        public TagBuilder GenerateDetails(bool open, IHtmlContent summary, IHtmlContent text)
        {
            if (summary == null)
            {
                throw new ArgumentNullException(nameof(summary));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var tagBuilder = new TagBuilder("details");
            tagBuilder.AddCssClass("govuk-details");
            tagBuilder.Attributes.Add("data-module", "govuk-details");

            if (open)
            {
                tagBuilder.Attributes.Add("open", "true");
            }

            var summaryTagBuilder = new TagBuilder("summary");
            summaryTagBuilder.AddCssClass("govuk-details__summary");

            var summaryTextTagBuilder = new TagBuilder("span");
            summaryTextTagBuilder.AddCssClass("govuk-details__summary-text");
            summaryTextTagBuilder.InnerHtml.AppendHtml(summary);
            summaryTagBuilder.InnerHtml.AppendHtml(summaryTextTagBuilder);

            tagBuilder.InnerHtml.AppendHtml(summaryTagBuilder);

            var textTagBuilder = new TagBuilder("div");
            textTagBuilder.AddCssClass("govuk-details__text");
            textTagBuilder.InnerHtml.AppendHtml(text);
            tagBuilder.InnerHtml.AppendHtml(textTagBuilder);

            return tagBuilder;
        }

        public virtual TagBuilder GenerateErrorMessage(
            ViewContext viewContext,
            ModelExplorer modelExplorer,
            string expression,
            string visuallyHiddenText,
            string id)
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

            var content = GetValidationMessage(viewContext, modelExplorer, expression);

            return GenerateErrorMessage(visuallyHiddenText, id, content);
        }

        public virtual TagBuilder GenerateErrorMessage(string visuallyHiddenText, string id, IHtmlContent content)
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

            if (string.IsNullOrEmpty(visuallyHiddenText))
            {
                visuallyHiddenText = DefaultErrorMessageVisuallyHiddenText;
            }

            var vht = new TagBuilder("span");
            vht.AddCssClass("govuk-visually-hidden");
            vht.InnerHtml.Append(visuallyHiddenText);

            tagBuilder.InnerHtml.AppendHtml(vht);

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

        public virtual TagBuilder GenerateInsetText(IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("govuk-inset-text");
            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }

        public virtual TagBuilder GenerateLabel(
            ViewContext viewContext,
            ModelExplorer modelExplorer,
            string expression,
            bool isPageHeading,
            IHtmlContent content)
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

            var resolvedFor = GetId(viewContext, modelExplorer, expression);
            var resolvedContent = content ?? GetDisplayName(viewContext, modelExplorer, expression);

            return GenerateLabel(resolvedFor, isPageHeading, resolvedContent);
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

        public virtual TagBuilder GenerateWarningText(IHtmlContent content, string iconFallbackText)
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("govuk-warning-text");

            var icon = new TagBuilder("span");
            icon.AddCssClass("govuk-warning-text__icon");
            icon.Attributes.Add("aria-hidden", "true");
            icon.InnerHtml.Append("!");

            tagBuilder.InnerHtml.AppendHtml(icon);

            var text = new TagBuilder("strong");
            text.AddCssClass("govuk-warning-text__text");

            var iconFallback = new TagBuilder("span");
            iconFallback.AddCssClass("govuk-warning-text__assistive");
            iconFallback.InnerHtml.Append(iconFallbackText);

            text.InnerHtml.AppendHtml(iconFallback);

            text.InnerHtml.AppendHtml(content);

            tagBuilder.InnerHtml.AppendHtml(text);

            return tagBuilder;
        }

        public virtual IHtmlContent GetDisplayName(
            ViewContext viewContext,
            ModelExplorer modelExplorer,
            string expression)
        {
            // HACK: We can't easily get at the internal NameAndIdProvider so we delegate to a method that uses it 
            // that is accessible then pull out the value

            var tagBuilder = _innerGenerator.GenerateLabel(viewContext, modelExplorer, expression, null, null);
            return tagBuilder.InnerHtml;
        }

        public virtual string GetId(
            ViewContext viewContext,
            ModelExplorer modelExplorer,
            string expression)
        {
            // HACK: We can't easily get at the internal NameAndIdProvider so we delegate to a method that uses it 
            // that is accessible then pull out the value

            var tagBuilder = _innerGenerator.GenerateLabel(viewContext, modelExplorer, expression, null, null);
            return tagBuilder.Attributes["for"];
        }

        public virtual IHtmlContent GetValidationMessage(
            ViewContext viewContext,
            ModelExplorer modelExplorer,
            string expression)
        {
            // HACK: We can't easily get at the internal NameAndIdProvider so we delegate to a method that uses it 
            // that is accessible then pull out the value

            var tagBuilder = _innerGenerator.GenerateValidationMessage(
                viewContext,
                modelExplorer,
                expression,
                null,
                null,
                null);
            return tagBuilder.InnerHtml;
        }
    }
}
