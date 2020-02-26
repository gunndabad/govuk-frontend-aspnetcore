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
        public const string DefaultInputType = "text";
        public const string DefaultTabsTitle = "Contents";
        public const int DefaultTextAreaRows = 5;

        private readonly IUrlHelperFactory _urlHelperFactory;

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

        public DefaultGovUkHtmlGenerator(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory ?? throw new ArgumentNullException(nameof(urlHelperFactory));
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

        public virtual TagBuilder GenerateCharacterCount(
            string elementId,
            int? maxLength,
            int? maxWords,
            decimal? threshold,
            IHtmlContent formGroup)
        {
            if (elementId == null)
            {
                throw new ArgumentNullException(nameof(elementId));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("govuk-character-count");
            tagBuilder.Attributes.Add("data-module", "govuk-character-count");

            if (maxLength.HasValue)
            {
                tagBuilder.Attributes.Add("data-maxlength", maxLength.Value.ToString());
            }

            if (threshold.HasValue)
            {
                tagBuilder.Attributes.Add("data-threshold", threshold.Value.ToString());
            }

            if (maxWords.HasValue)
            {
                tagBuilder.Attributes.Add("data-maxwords", maxWords.Value.ToString());
            }
            
            tagBuilder.InnerHtml.AppendHtml(formGroup);
            tagBuilder.InnerHtml.AppendHtml(GetHint());

            return tagBuilder;

            IHtmlContent GetHint()
            {
                var content = maxWords.HasValue ?
                    $"You can enter up to {maxWords.Value} words" :
                    $"You can enter up to {maxLength.Value} characters";

                var hintId = $"{elementId}-info";
                var hintContent = new HtmlString(content);
                var generatedHint = GenerateHint(hintId, hintContent);

                generatedHint.AddCssClass("govuk-character-count__message");
                generatedHint.Attributes.Add("aria-live", "polite");

                return generatedHint;
            }
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

        public virtual TagBuilder GenerateFieldset(
            string describedBy,
            bool isPageHeading,
            string role,
            IHtmlContent legendContent,
            IHtmlContent content)
        {
            var tagBuilder = new TagBuilder("fieldset");
            tagBuilder.AddCssClass("govuk-fieldset");

            if (role != null)
            {
                tagBuilder.Attributes.Add("role", role);
            }

            if (describedBy != null)
            {
                tagBuilder.Attributes.Add("aria-describedby", describedBy);
            }

            var legend = new TagBuilder("legend");
            legend.AddCssClass("govuk-fieldset__legend");

            if (isPageHeading)
            {
                var h1 = new TagBuilder("h1");
                h1.AddCssClass("govuk-fieldset__heading");
                h1.InnerHtml.AppendHtml(legendContent);
                legend.InnerHtml.AppendHtml(h1);
            }
            else
            {
                legend.InnerHtml.AppendHtml(legendContent);
            }

            tagBuilder.InnerHtml.AppendHtml(legend);
            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }

        public virtual TagBuilder GenerateFormGroup(bool haveError, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("govuk-form-group");

            if (haveError)
            {
                tagBuilder.AddCssClass("govuk-form-group--error");
            }

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

        public virtual TagBuilder GenerateInput(
            bool haveError,
            string id,
            string name,
            string type,
            string value,
            string describedBy,
            string autocomplete,
            string pattern,
            string inputMode)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var tagBuilder = new TagBuilder("input");
            tagBuilder.AddCssClass("govuk-input");

            if (haveError)
            {
                tagBuilder.AddCssClass("govuk-input--error");
            }

            tagBuilder.Attributes.Add("id", id);
            tagBuilder.Attributes.Add("name", name);
            tagBuilder.Attributes.Add("type", type ?? DefaultInputType);

            if (value != null)
            {
                tagBuilder.Attributes.Add("value", value);
            }

            if (describedBy != null)
            {
                tagBuilder.Attributes.Add("aria-describedby", describedBy);
            }

            if (autocomplete != null)
            {
                tagBuilder.Attributes.Add("autocomplete", autocomplete);
            }

            if (pattern != null)
            {
                tagBuilder.Attributes.Add("pattern", pattern);
            }

            if (inputMode != null)
            {
                tagBuilder.Attributes.Add("inputmode", inputMode);
            }

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

        public virtual TagBuilder GenerateTabs(
            string id,
            string title,
            IEnumerable<(string id, string label, IHtmlContent content)> items)
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("govuk-tabs");
            tagBuilder.Attributes.Add("data-module", "govuk-tabs");

            if (id != null)
            {
                tagBuilder.Attributes.Add("id", id);
            }

            var h2 = new TagBuilder("h2");
            h2.AddCssClass("govuk-tabs__title");
            h2.InnerHtml.Append(title ?? DefaultTabsTitle);
            tagBuilder.InnerHtml.AppendHtml(h2);

            var ul = new TagBuilder("ul");
            ul.AddCssClass("govuk-tabs__list");

            foreach (var item in items)
            {
                var li = new TagBuilder("li");
                li.AddCssClass("govuk-tabs__list-item");

                if (item == items.First())
                {
                    li.AddCssClass("govuk-tabs__list-item--selected");
                }

                var a = new TagBuilder("a");
                a.AddCssClass("govuk-tabs__tab");
                a.Attributes.Add("href", $"#{item.id}");
                a.InnerHtml.Append(item.label);
                li.InnerHtml.AppendHtml(a);

                ul.InnerHtml.AppendHtml(li);
            }

            tagBuilder.InnerHtml.AppendHtml(ul);

            foreach (var item in items)
            {
                var section = new TagBuilder("section");
                section.AddCssClass("govuk-tabs__panel");
                section.Attributes.Add("id", item.id);

                if (item != items.First())
                {
                    section.AddCssClass("govuk-tabs__panel--hidden");
                }

                section.InnerHtml.AppendHtml(item.content);

                tagBuilder.InnerHtml.AppendHtml(section);
            }

            return tagBuilder;
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

        public virtual TagBuilder GenerateRadios(bool isConditional, IHtmlContent content)
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("govuk-radios");

            if (isConditional)
            {
                tagBuilder.AddCssClass("govuk-radios--conditional");
                tagBuilder.Attributes.Add("data-module", "govuk-radios");
            }

            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }

        public virtual TagBuilder GenerateRadioItem(
            string id,
            string name,
            string value,
            bool @checked,
            bool disabled,
            IHtmlContent content,
            string conditionalId,
            IHtmlContent conditionalContent,
            string hintId,
            IHtmlContent hintContent)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (conditionalId == null && conditionalContent != null)
            {
                throw new ArgumentNullException(
                    nameof(conditionalId),
                    $"{nameof(conditionalId)} must be provided when {nameof(conditionalContent)} is specified.");
            }

            if (hintId == null && hintContent != null)
            {
                throw new ArgumentNullException(
                    nameof(hintId),
                    $"{nameof(hintId)} must be provided when {nameof(hintContent)} is specified.");
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("govuk-radios__item");

            var input = new TagBuilder("input");
            input.AddCssClass("govuk-radios__input");
            input.Attributes.Add("id", id);
            input.Attributes.Add("name", name);
            input.Attributes.Add("type", "radio");
            input.Attributes.Add("value", value);

            if (@checked)
            {
                input.Attributes.Add("checked", "checked");
            }

            if (disabled)
            {
                input.Attributes.Add("disabled", "disabled");
            }

            if (conditionalContent != null)
            {
                input.Attributes.Add("data-aria-controls", conditionalId);
            }

            if (hintContent != null)
            {
                input.Attributes.Add("aria-describedby", hintId);
            }

            tagBuilder.InnerHtml.AppendHtml(input);

            var label = GenerateLabel(id, isPageHeading: false, content);
            label.AddCssClass("govuk-radios__label");
            tagBuilder.InnerHtml.AppendHtml(label);

            if (hintContent != null)
            {
                var hint = GenerateHint(hintId, hintContent);
                hint.AddCssClass("govuk-radios__hint");
                tagBuilder.InnerHtml.AppendHtml(hint);
            }

            if (conditionalContent != null)
            {
                var conditional = new TagBuilder("div");
                conditional.AddCssClass("govuk-radios__conditional");

                if (!@checked)
                {
                    conditional.AddCssClass("govuk-radios__conditional--hidden");
                }

                conditional.Attributes.Add("id", conditionalId);

                conditional.InnerHtml.AppendHtml(conditionalContent);

                tagBuilder.InnerHtml.AppendHtml(conditional);
            }

            return tagBuilder;
        }

        public virtual TagBuilder GenerateRadioItemDivider(IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("govuk-radios__divider");
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

        public virtual TagBuilder GenerateTextArea(
            bool haveError,
            string id,
            string name,
            int? rows,
            string describedBy,
            string autocomplete,
            IHtmlContent content)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("textarea");
            tagBuilder.AddCssClass("govuk-textarea");

            if (haveError)
            {
                tagBuilder.AddCssClass("govuk-textarea--error");
            }

            tagBuilder.Attributes.Add("id", id);
            tagBuilder.Attributes.Add("name", name);
            tagBuilder.Attributes.Add("rows", (rows ?? DefaultTextAreaRows).ToString());

            if (describedBy != null)
            {
                tagBuilder.Attributes.Add("aria-describedby", describedBy);
            }

            if (autocomplete != null)
            {
                tagBuilder.Attributes.Add("autocomplete", autocomplete);
            }

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

        public virtual string GetModelValue(ViewContext viewContext, ModelExplorer modelExplorer, string expression)
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

            var fullName = GetFullHtmlFieldName(viewContext, expression);

            // See https://github.com/dotnet/aspnetcore/blob/9a3aacb56af7221bfb29d851ee6b7c883650ddf6/src/Mvc/Mvc.ViewFeatures/src/DefaultHtmlGenerator.cs#L714-L724

            viewContext.ViewData.ModelState.TryGetValue(fullName, out var entry);

            var value = string.Empty;
            if (entry != null && entry.AttemptedValue != null)
            {
                value = entry.AttemptedValue;
            }
            else if (modelExplorer.Model != null)
            {
                value = modelExplorer.Model.ToString();
            }

            return value;
        }

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
