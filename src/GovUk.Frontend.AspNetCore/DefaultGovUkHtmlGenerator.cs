using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
#if netstandard2
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
#endif

namespace GovUk.Frontend.AspNetCore
{
    public class DefaultGovUkHtmlGenerator : IGovUkHtmlGenerator
    {
        public const string DefaultErrorMessageVisuallyHiddenText = "Error";

        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHtmlGenerator _innerGenerator;

        private static readonly GetFullHtmlFieldNameDelegate s_getFullHtmlFieldNameDelegate;

        static DefaultGovUkHtmlGenerator()
        {
            s_getFullHtmlFieldNameDelegate =
#if netstandard2
                NameAndIdProvider.GetFullHtmlFieldName;
#else
                (GetFullHtmlFieldNameDelegate)typeof(IHtmlGenerator).Assembly
                    .GetType("Microsoft.AspNetCore.Mvc.ViewFeatures.NameAndIdProvider", throwOnError: true)
                    .GetMethod("GetFullHtmlFieldName", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                    .CreateDelegate(typeof(GetFullHtmlFieldNameDelegate));
#endif
        }

        public DefaultGovUkHtmlGenerator(IUrlHelperFactory urlHelperFactory, IHtmlGenerator innerGenerator)
        {
            _urlHelperFactory = urlHelperFactory ?? throw new ArgumentNullException(nameof(urlHelperFactory));
            _innerGenerator = innerGenerator ?? throw new ArgumentNullException(nameof(innerGenerator));
        }

        private delegate string GetFullHtmlFieldNameDelegate(ViewContext viewContext, string expression);

        public TagBuilder GenerateAnchor(string href)
        {
            if (href == null)
            {
                throw new ArgumentNullException(nameof(href));
            }

            var tagBuilder = new TagBuilder("a");
            tagBuilder.Attributes.Add("href", href);

            return tagBuilder;
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

        public virtual TagBuilder GenerateHint(string id, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("span");
            tagBuilder.AddCssClass("govuk-hint");

            if (!string.IsNullOrEmpty(id))
            {
                tagBuilder.Attributes.Add("id", id);
            }

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

        public virtual string GetActionLinkHref(
            ViewContext viewContext,
            string action,
            string controller,
            object values,
            string protocol,
            string host,
            string fragment)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(viewContext);
            return urlHelper.Action(action, controller, values, protocol, host, fragment);
        }

        public virtual string GetPageLinkHref(
            ViewContext viewContext,
            string pageName,
            string pageHandler,
            object values,
            string protocol,
            string host,
            string fragment)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(viewContext);
            return urlHelper.Page(pageName, pageHandler, values, protocol, host, fragment);
        }

        public virtual string GetRouteLinkHref(
            ViewContext viewContext,
            string routeName,
            object values,
            string protocol,
            string host,
            string fragment)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(viewContext);
            return urlHelper.RouteUrl(routeName, values, protocol, host, fragment);
        }

        public virtual string GetDisplayName(
            ViewContext viewContext,
            ModelExplorer modelExplorer,
            string expression)
        {
            // See https://github.com/aspnet/AspNetCore/blob/master/src/Mvc/Mvc.ViewFeatures/src/DefaultHtmlGenerator.cs#L427

            var displayName = modelExplorer.Metadata.DisplayName ?? modelExplorer.Metadata.PropertyName;

            if (displayName != null && expression != null)
            {
                displayName = displayName.Split('.').Last();
            }

            return displayName;
        }

        public virtual string GetFullHtmlFieldName(ViewContext viewContext, string expression) =>
            s_getFullHtmlFieldNameDelegate(viewContext, expression);

        public virtual string GetValidationMessage(
            ViewContext viewContext,
            ModelExplorer modelExplorer,
            string expression)
        {
            // See https://github.com/aspnet/AspNetCore/blob/master/src/Mvc/Mvc.ViewFeatures/src/DefaultHtmlGenerator.cs#L795

            var fullName = GetFullHtmlFieldName(viewContext, expression);

            if (!viewContext.ViewData.ModelState.ContainsKey(fullName))
            {
                return null;
            }

            var tryGetModelStateResult = viewContext.ViewData.ModelState.TryGetValue(fullName, out var entry);
            var modelErrors = tryGetModelStateResult ? entry.Errors : null;

            ModelError modelError = null;
            if (modelErrors != null && modelErrors.Count != 0)
            {
                modelError = modelErrors.FirstOrDefault(m => !string.IsNullOrEmpty(m.ErrorMessage)) ?? modelErrors[0];
            }

            if (modelError == null)
            {
                return null;
            }

            return modelError.ErrorMessage;
        }
    }
}
