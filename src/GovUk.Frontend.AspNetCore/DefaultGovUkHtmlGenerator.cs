using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
#if NETSTANDARD2_0
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
#endif

namespace GovUk.Frontend.AspNetCore
{
    public class DefaultGovUkHtmlGenerator : IGovUkHtmlGenerator
    {
        public const int DefaultAccordionItemHeadingLevel = 2;
        public const string DefaultErrorMessageVisuallyHiddenText = "Error";
        public const string DefaultInputType = "text";
        public const int DefaultPanelHeadingLevel = 1;
        public const string DefaultTabsTitle = "Contents";
        public const int DefaultTextAreaRows = 5;

        private static readonly GetFullHtmlFieldNameDelegate s_getFullHtmlFieldNameDelegate;

        static DefaultGovUkHtmlGenerator()
        {
            s_getFullHtmlFieldNameDelegate =
#if NETSTANDARD2_0
                NameAndIdProvider.GetFullHtmlFieldName;
#else
                (GetFullHtmlFieldNameDelegate)typeof(IHtmlGenerator).Assembly
                    .GetType("Microsoft.AspNetCore.Mvc.ViewFeatures.NameAndIdProvider", throwOnError: true)
                    .GetMethod("GetFullHtmlFieldName", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                    .CreateDelegate(typeof(GetFullHtmlFieldNameDelegate));
#endif
        }

        private delegate string GetFullHtmlFieldNameDelegate(ViewContext viewContext, string expression);

        public virtual TagBuilder GenerateAccordion(
            string id,
            IDictionary<string, string> attributes,
            IEnumerable<AccordionItem> items)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.AddCssClass("govuk-accordion");
            tagBuilder.Attributes.Add("data-module", "govuk-accordion");
            tagBuilder.Attributes.Add("id", id);

            var index = 0;
            foreach (var item in items)
            {
                var section = new TagBuilder("div");
                section.AddCssClass("govuk-accordion__section");

                if (item.Expanded)
                {
                    section.AddCssClass("govuk-accordion__section--expanded");
                }

                var header = new TagBuilder("div");
                header.AddCssClass("govuk-accordion__section-header");

                // REVIEW Validate heading level?

                var headingId = $"{id}-heading-{index}";
                var heading = new TagBuilder($"h{item.HeadingLevel ?? DefaultAccordionItemHeadingLevel}");
                heading.AddCssClass("govuk-accordion__section-heading");
                var headingContent = new TagBuilder("span");
                headingContent.AddCssClass("govuk-accordion__section-button");
                headingContent.Attributes.Add("id", headingId);
                headingContent.InnerHtml.AppendHtml(item.HeadingContent);
                heading.InnerHtml.AppendHtml(headingContent);
                header.InnerHtml.AppendHtml(heading);

                if (item.Summary != null)
                {
                    var summaryId = $"{id}-summary-{index}";
                    var summary = new TagBuilder("div");
                    summary.AddCssClass("govuk-accordion__section-summary");
                    summary.AddCssClass("govuk-body");
                    summary.Attributes.Add("id", summaryId);
                    header.InnerHtml.AppendHtml(summary);
                }

                section.InnerHtml.AppendHtml(header);

                var contentId = $"{id}-content-{index}";
                var contentDiv = new TagBuilder("div");
                contentDiv.AddCssClass("govuk-accordion__section-content");
                contentDiv.Attributes.Add("id", contentId);
                contentDiv.Attributes.Add("aria-labelledby", headingId);
                contentDiv.InnerHtml.AppendHtml(item.Content);
                section.InnerHtml.AppendHtml(contentDiv);

                tagBuilder.InnerHtml.AppendHtml(section);

                index++;
            }

            return tagBuilder;
        }

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

        public virtual TagBuilder GenerateBackLink(
            string href,
            IDictionary<string, string> attributes,
            IHtmlContent content)
        {
            if (href == null)
            {
                throw new ArgumentNullException(nameof(href));
            }

            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("a");
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.AddCssClass("govuk-back-link");
            tagBuilder.Attributes.Add("href", href);
            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }

        public virtual TagBuilder GenerateBreadcrumbs(
            IDictionary<string, string> attributes,
            IEnumerable<BreadcrumbsItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.AddCssClass("govuk-breadcrumbs");

            var ol = new TagBuilder("ol");
            ol.AddCssClass("govuk-breadcrumbs__list");

            foreach (var item in items)
            {
                var li = new TagBuilder("li");
                li.AddCssClass("govuk-breadcrumbs__list-item");

                if (item.IsCurrentPage)
                {
                    li.Attributes.Add("aria-current", "page");
                }

                IHtmlContent itemContent;

                if (item.Href != null)
                {
                    var itemLink = new TagBuilder("a");
                    itemLink.AddCssClass("govuk-breadcrumbs__link");
                    itemLink.Attributes.Add("href", item.Href);
                    itemLink.InnerHtml.AppendHtml(item.Content);
                    itemContent = itemLink;
                }
                else
                {
                    itemContent = item.Content;
                }

                li.InnerHtml.AppendHtml(itemContent);

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
                var generatedHint = GenerateHint(hintId, attributes: null, hintContent);

                generatedHint.AddCssClass("govuk-character-count__message");
                generatedHint.Attributes.Add("aria-live", "polite");

                return generatedHint;
            }
        }

        public virtual TagBuilder GenerateCheckboxes(
            string name,
            bool isConditional,
            string describedBy,
            IDictionary<string, string> attributes,
            IEnumerable<CheckboxesItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.AddCssClass("govuk-checkboxes");

            if (isConditional)
            {
                tagBuilder.AddCssClass("govuk-checkboxes--conditional");
                tagBuilder.Attributes.Add("data-module", "govuk-checkboxes");
            }

            foreach (var item in items)
            {
                var itemContent = GenerateCheckboxItem(item);

                tagBuilder.InnerHtml.AppendHtml(itemContent);
            }

            return tagBuilder;

            IHtmlContent GenerateCheckboxItem(CheckboxesItem item)
            {
                // REVIEW Validate properties?

                //if (item.Id == null)
                //{
                //    throw new ArgumentNullException(nameof(id));
                //}

                //if (value == null)
                //{
                //    throw new ArgumentNullException(nameof(value));
                //}

                //if (conditionalId == null && conditionalContent != null)
                //{
                //    throw new ArgumentNullException(
                //        nameof(conditionalId),
                //        $"{nameof(conditionalId)} must be provided when {nameof(conditionalContent)} is specified.");
                //}

                //if (hintId == null && hintContent != null)
                //{
                //    throw new ArgumentNullException(
                //        nameof(hintId),
                //        $"{nameof(hintId)} must be provided when {nameof(hintContent)} is specified.");
                //}

                var tagBuilder = new TagBuilder("div");
                tagBuilder.AddCssClass("govuk-checkboxes__item");

                var input = new TagBuilder("input");
                input.MergeAttributes(item.Attributes);
                input.TagRenderMode = TagRenderMode.SelfClosing;
                input.AddCssClass("govuk-checkboxes__input");
                input.Attributes.Add("id", item.Id);
                input.Attributes.Add("name", name);
                input.Attributes.Add("type", "checkbox");
                input.Attributes.Add("value", item.Value);

                if (item.Checked)
                {
                    input.Attributes.Add("checked", "checked");
                }

                if (item.Disabled)
                {
                    input.Attributes.Add("disabled", "disabled");
                }

                if (item.ConditionalContent != null)
                {
                    input.Attributes.Add("data-aria-controls", item.ConditionalId);
                }

                var itemDescribedBy = describedBy;
                if (item.HintContent != null)
                {
                    itemDescribedBy = ((describedBy ?? "") + $" {item.HintId}").Trim();
                }

                if (itemDescribedBy != null)
                {
                    input.Attributes.Add("aria-describedby", itemDescribedBy);
                }

                tagBuilder.InnerHtml.AppendHtml(input);

                var label = GenerateLabel(item.Id, isPageHeading: false, item.Content);
                label.AddCssClass("govuk-checkboxes__label");
                tagBuilder.InnerHtml.AppendHtml(label);

                if (item.HintContent != null)
                {
                    var hint = GenerateHint(item.HintId, item.HintAttributes, item.HintContent);
                    hint.AddCssClass("govuk-checkboxes__hint");
                    tagBuilder.InnerHtml.AppendHtml(hint);
                }

                if (item.ConditionalContent != null)
                {
                    var conditional = new TagBuilder("div");
                    conditional.AddCssClass("govuk-checkboxes__conditional");

                    if (!item.Checked)
                    {
                        conditional.AddCssClass("govuk-checkboxes__conditional--hidden");
                    }

                    conditional.Attributes.Add("id", item.ConditionalId);

                    conditional.InnerHtml.AppendHtml(item.ConditionalContent);

                    tagBuilder.InnerHtml.AppendHtml(conditional);
                }

                return tagBuilder;
            }
        }

        public TagBuilder GenerateDetails(
            bool open,
            IHtmlContent summary,
            IHtmlContent text,
            IDictionary<string, string> attributes)
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
            tagBuilder.MergeAttributes(attributes);
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
            string visuallyHiddenText,
            string id,
            IDictionary<string, string> attributes,
            IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("span");
            tagBuilder.MergeAttributes(attributes);
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

        public virtual TagBuilder GenerateErrorSummary(
            IHtmlContent title,
            IHtmlContent description,
            IDictionary<string, string> attributes,
            IEnumerable<ErrorSummaryItem> items)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.AddCssClass("govuk-error-summary");
            tagBuilder.Attributes.Add("aria-labelledby", "error-summary-title");
            tagBuilder.Attributes.Add("role", "alert");
            tagBuilder.Attributes.Add("tabindex", "-1");
            tagBuilder.Attributes.Add("data-module", "govuk-error-summary");

            var heading = new TagBuilder("h2");
            heading.AddCssClass("govuk-error-summary__title");
            heading.Attributes.Add("id", "error-summary-title");
            heading.InnerHtml.AppendHtml(title);
            tagBuilder.InnerHtml.AppendHtml(heading);

            var body = new TagBuilder("div");
            body.AddCssClass("govuk-error-summary__body");

            if (description != null)
            {
                var p = new TagBuilder("p");
                p.InnerHtml.AppendHtml(description);
                body.InnerHtml.AppendHtml(p);
            }

            var ul = new TagBuilder("ul");
            ul.AddCssClass("govuk-list");
            ul.AddCssClass("govuk-error-summary__list");

            foreach (var item in items)
            {
                var li = new TagBuilder("li");
                li.InnerHtml.AppendHtml(item.Content);
                ul.InnerHtml.AppendHtml(li);
            }

            body.InnerHtml.AppendHtml(ul);

            tagBuilder.InnerHtml.AppendHtml(body);

            return tagBuilder;
        }

        public virtual TagBuilder GenerateFieldset(
            string describedBy,
            bool isPageHeading,
            string role,
            IDictionary<string, string> attributes,
            IHtmlContent legendContent,
            IDictionary<string, string> legendAttributes,
            IHtmlContent content)
        {
            var tagBuilder = new TagBuilder("fieldset");
            tagBuilder.MergeAttributes(attributes);
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
            legend.MergeAttributes(legendAttributes);
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

        public virtual TagBuilder GenerateFormGroup(
            bool haveError,
            IDictionary<string, string> attributes,
            IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.AddCssClass("govuk-form-group");

            if (haveError)
            {
                tagBuilder.AddCssClass("govuk-form-group--error");
            }

            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }

        public virtual TagBuilder GenerateHint(string id, IDictionary<string, string> attributes, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("span");
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.AddCssClass("govuk-hint");

            if (!string.IsNullOrEmpty(id))
            {
                tagBuilder.Attributes.Add("id", id);
            }

            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }

        public virtual TagBuilder GenerateInsetText(string id, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("govuk-inset-text");

            if (id != null)
            {
                tagBuilder.Attributes.Add("id", id);
            }

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
            string inputMode,
            bool disabled,
            IDictionary<string, string> attributes)
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
            tagBuilder.MergeAttributes(attributes);
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

            if (disabled)
            {
                tagBuilder.Attributes.Add("disabled", "disabled");
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

        public virtual TagBuilder GeneratePanel(
            int? titleHeadingLevel,
            IHtmlContent titleContent,
            IHtmlContent content)
        {
            if (titleContent == null)
            {
                throw new ArgumentNullException(nameof(titleContent));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("govuk-panel");
            tagBuilder.AddCssClass("govuk-panel--confirmation");

            var heading = new TagBuilder($"h{titleHeadingLevel ?? DefaultPanelHeadingLevel}");
            heading.AddCssClass("govuk-panel__title");
            heading.InnerHtml.AppendHtml(titleContent);
            tagBuilder.InnerHtml.AppendHtml(heading);

            if (content != null)
            {
                var body = new TagBuilder("div");
                body.AddCssClass("govuk-panel__body");
                body.InnerHtml.AppendHtml(content);
                tagBuilder.InnerHtml.AppendHtml(body);
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

        public virtual TagBuilder GenerateRadios(string name, bool isConditional, IEnumerable<RadiosItemBase> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("govuk-radios");

            if (isConditional)
            {
                tagBuilder.AddCssClass("govuk-radios--conditional");
                tagBuilder.Attributes.Add("data-module", "govuk-radios");
            }

            foreach (var item in items)
            {
                IHtmlContent itemContent;

                if (item is RadiosItemDivider divider)
                {
                    itemContent = GenerateRadioItemDivider(divider);
                }
                else if (item is RadiosItem i)
                {
                    itemContent = GenerateRadioItem(i);
                }
                else
                {
                    throw new NotSupportedException($"Unknown item type: '{item.GetType().FullName}'.");
                }

                tagBuilder.InnerHtml.AppendHtml(itemContent);
            }

            return tagBuilder;

            IHtmlContent GenerateRadioItem(RadiosItem item)
            {
                // REVIEW Validate properties?

                //if (item.Id == null)
                //{
                //    throw new ArgumentNullException(nameof(id));
                //}

                //if (value == null)
                //{
                //    throw new ArgumentNullException(nameof(value));
                //}

                //if (conditionalId == null && conditionalContent != null)
                //{
                //    throw new ArgumentNullException(
                //        nameof(conditionalId),
                //        $"{nameof(conditionalId)} must be provided when {nameof(conditionalContent)} is specified.");
                //}

                //if (hintId == null && hintContent != null)
                //{
                //    throw new ArgumentNullException(
                //        nameof(hintId),
                //        $"{nameof(hintId)} must be provided when {nameof(hintContent)} is specified.");
                //}

                var tagBuilder = new TagBuilder("div");
                tagBuilder.AddCssClass("govuk-radios__item");

                var input = new TagBuilder("input");
                input.TagRenderMode = TagRenderMode.SelfClosing;
                input.AddCssClass("govuk-radios__input");
                input.Attributes.Add("id", item.Id);
                input.Attributes.Add("name", name);
                input.Attributes.Add("type", "radio");
                input.Attributes.Add("value", item.Value);

                if (item.Checked)
                {
                    input.Attributes.Add("checked", "checked");
                }

                if (item.Disabled)
                {
                    input.Attributes.Add("disabled", "disabled");
                }

                if (item.ConditionalContent != null)
                {
                    input.Attributes.Add("data-aria-controls", item.ConditionalId);
                }

                if (item.HintContent != null)
                {
                    input.Attributes.Add("aria-describedby", item.HintId);
                }

                tagBuilder.InnerHtml.AppendHtml(input);

                var label = GenerateLabel(item.Id, isPageHeading: false, item.Content);
                label.AddCssClass("govuk-radios__label");
                tagBuilder.InnerHtml.AppendHtml(label);

                if (item.HintContent != null)
                {
                    var hint = GenerateHint(item.HintId, item.HintAttributes, item.HintContent);
                    hint.AddCssClass("govuk-radios__hint");
                    tagBuilder.InnerHtml.AppendHtml(hint);
                }

                if (item.ConditionalContent != null)
                {
                    var conditional = new TagBuilder("div");
                    conditional.AddCssClass("govuk-radios__conditional");

                    if (!item.Checked)
                    {
                        conditional.AddCssClass("govuk-radios__conditional--hidden");
                    }

                    conditional.Attributes.Add("id", item.ConditionalId);

                    conditional.InnerHtml.AppendHtml(item.ConditionalContent);

                    tagBuilder.InnerHtml.AppendHtml(conditional);
                }

                return tagBuilder;
            }

            IHtmlContent GenerateRadioItemDivider(RadiosItemDivider divider)
            {
                var tagBuilder = new TagBuilder("div");
                tagBuilder.AddCssClass("govuk-radios__divider");
                tagBuilder.InnerHtml.AppendHtml(divider.Content);
                return tagBuilder;
            }
        }

        public virtual TagBuilder GenerateSummaryList(IEnumerable<SummaryListRow> rows)
        {
            if (rows == null)
            {
                throw new ArgumentNullException(nameof(rows));
            }

            var anyRowHasActions = rows.Any(r => r.Actions.Any());

            var tagBuilder = new TagBuilder("dl");
            tagBuilder.AddCssClass("govuk-summary-list");

            foreach (var row in rows)
            {
                var rowTagBuilder = new TagBuilder("div");
                rowTagBuilder.AddCssClass("govuk-summary-list__row");

                var dt = new TagBuilder("dt");
                dt.AddCssClass("govuk-summary-list__key");
                dt.InnerHtml.AppendHtml(row.Key);
                rowTagBuilder.InnerHtml.AppendHtml(dt);

                var dd = new TagBuilder("dt");
                dd.AddCssClass("govuk-summary-list__value");
                dd.InnerHtml.AppendHtml(row.Value);
                rowTagBuilder.InnerHtml.AppendHtml(dd);

                if (row.Actions.Any())
                {
                    var actionsDd = new TagBuilder("dd");
                    actionsDd.AddCssClass("govuk-summary-list__actions");

                    if (row.Actions.Count() == 1)
                    {
                        actionsDd.InnerHtml.AppendHtml(GenerateLink(row.Actions.Single()));
                    }
                    else
                    {
                        var ul = new TagBuilder("ul");
                        ul.AddCssClass("govuk-summary-list__actions-list");

                        foreach (var action in row.Actions)
                        {
                            var li = new TagBuilder("li");
                            li.AddCssClass("govuk-summary-list__actions-list-item");
                            li.InnerHtml.AppendHtml(GenerateLink(action));

                            ul.InnerHtml.AppendHtml(li);
                        }

                        actionsDd.InnerHtml.AppendHtml(ul);
                    }

                    rowTagBuilder.InnerHtml.AppendHtml(actionsDd);
                }
                else
                {
                    var span = new TagBuilder("span");
                    span.AddCssClass("govuk-summary-list__actions");
                    rowTagBuilder.InnerHtml.AppendHtml(span);
                }

                tagBuilder.InnerHtml.AppendHtml(rowTagBuilder);
            }

            return tagBuilder;

            TagBuilder GenerateLink(SummaryListRowAction action)
            {
                var anchor = new TagBuilder("a");
                anchor.AddCssClass("govuk-link");
                anchor.Attributes.Add("href", action.Href);
                anchor.InnerHtml.AppendHtml(action.Content);

                if (action.VisuallyHiddenText != null)
                {
                    var vht = new TagBuilder("span");
                    vht.AddCssClass("govuk-visually-hidden");
                    vht.InnerHtml.Append(action.VisuallyHiddenText);
                    anchor.InnerHtml.AppendHtml(vht);
                }

                return anchor;
            }
        }

        public virtual TagBuilder GenerateTabs(string id, string title, IEnumerable<TabsItem> items)
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
                a.Attributes.Add("href", $"#{item.Id}");
                a.InnerHtml.Append(item.Label);
                li.InnerHtml.AppendHtml(a);

                ul.InnerHtml.AppendHtml(li);
            }

            tagBuilder.InnerHtml.AppendHtml(ul);

            foreach (var item in items)
            {
                var section = new TagBuilder("section");
                section.AddCssClass("govuk-tabs__panel");
                section.Attributes.Add("id", item.Id);

                if (item != items.First())
                {
                    section.AddCssClass("govuk-tabs__panel--hidden");
                }

                section.InnerHtml.AppendHtml(item.Content);

                tagBuilder.InnerHtml.AppendHtml(section);
            }

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
            bool disabled,
            IDictionary<string, string> attributes,
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
            tagBuilder.MergeAttributes(attributes);
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

            if (disabled)
            {
                tagBuilder.Attributes.Add("disabled", "disabled");
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
