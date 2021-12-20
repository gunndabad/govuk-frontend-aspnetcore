using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator : IGovUkHtmlGenerator
    {
        internal const string FormGroupElement = "div";

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

        public virtual TagBuilder GenerateButton(
            string name,
            string type,
            string value,
            bool isStartButton,
            bool disabled,
            bool preventDoubleClick,
            string formAction,
            IHtmlContent content,
            IDictionary<string, string> attributes)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("button");
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.AddCssClass("govuk-button");
            tagBuilder.Attributes.Add("data-module", "govuk-button");
            tagBuilder.Attributes.Add("type", type);

            if (disabled)
            {
                tagBuilder.AddCssClass("govuk-button--disabled");
                tagBuilder.Attributes.Add("disabled", "disabled");
                tagBuilder.Attributes.Add("aria-disabled", "true");
            }

            if (name != null)
            {
                tagBuilder.Attributes.Add("name", name);
            }

            if (preventDoubleClick)
            {
                tagBuilder.Attributes.Add("data-prevent-double-click", "true");
            }

            if (value != null)
            {
                tagBuilder.Attributes.Add("value", value);
            }

            if (formAction != null)
            {
                tagBuilder.Attributes.Add("formaction", formAction);
            }

            tagBuilder.InnerHtml.AppendHtml(content);

            if (isStartButton)
            {
                tagBuilder.AddCssClass("govuk-button--start");

                var icon = GenerateStartButton();
                tagBuilder.InnerHtml.AppendHtml(icon);
            }

            return tagBuilder;
        }

        public virtual TagBuilder GenerateButtonLink(
            string href,
            bool isStartButton,
            bool disabled,
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

            var tagBuilder = new TagBuilder("a");
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.AddCssClass("govuk-button");
            tagBuilder.Attributes.Add("data-module", "govuk-button");
            tagBuilder.Attributes.Add("role", "button");
            tagBuilder.Attributes.Add("draggable", "false");
            tagBuilder.Attributes.Add("href", href);

            if (disabled)
            {
                tagBuilder.AddCssClass("govuk-button--disabled");
            }

            tagBuilder.InnerHtml.AppendHtml(content);

            if (isStartButton)
            {
                tagBuilder.AddCssClass("govuk-button--start");

                var icon = GenerateStartButton();
                tagBuilder.InnerHtml.AppendHtml(icon);
            }

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
            tagBuilder.MergeCssClass("govuk-character-count");
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
                var generatedHint = GenerateHint(hintId, hintContent, attributes: null);

                generatedHint.MergeCssClass("govuk-character-count__message");
                generatedHint.Attributes.Add("aria-live", "polite");

                return generatedHint;
            }
        }

        public virtual TagBuilder GenerateDateInput(
            string id,
            bool disabled,
            DateInputItem day,
            DateInputItem month,
            DateInputItem year,
            IDictionary<string, string> attributes)
        {
            if (day == null)
            {
                throw new ArgumentNullException(nameof(day));
            }

            if (month == null)
            {
                throw new ArgumentNullException(nameof(month));
            }

            if (year == null)
            {
                throw new ArgumentNullException(nameof(year));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-date-input");
            
            if (id != null)
            {
                tagBuilder.Attributes.Add("id", id);
            }

            tagBuilder.InnerHtml.AppendHtml(CreateDateComponent(day, "govuk-input--width-2"));
            tagBuilder.InnerHtml.AppendHtml(CreateDateComponent(month, "govuk-input--width-2"));
            tagBuilder.InnerHtml.AppendHtml(CreateDateComponent(year, "govuk-input--width-4"));

            return tagBuilder;

            IHtmlContent CreateDateComponent(DateInputItem item, string @class)
            {
                var div = new TagBuilder("div");
                div.MergeCssClass("govuk-date-input__item");

                var itemInput = GenerateTextInput(
                    item.HaveError,
                    item.Id,
                    item.Name,
                    type: "text",
                    item.Value,
                    describedBy: null,
                    item.Autocomplete,
                    item.Pattern ?? "[0-9]*",
                    inputMode: "numeric",
                    spellcheck: null,
                    disabled,
                    item.Attributes,
                    prefixContent: null,
                    prefixAttributes: null,
                    suffixContent: null,
                    suffixAttributes: null);
                itemInput.MergeCssClass("govuk-date-input__input");
                itemInput.MergeCssClass(@class);

                var itemLabel = GenerateLabel(
                    @for: item.Id,
                    isPageHeading: false,
                    content: item.Label,
                    attributes: null);
                itemLabel.MergeCssClass("govuk-date-input__label");

                var contentBuilder = new HtmlContentBuilder()
                    .AppendHtml(itemLabel)
                    .AppendHtml(itemInput);

                var itemFormGroup = GenerateFormGroup(
                    item.HaveError,
                    content: contentBuilder,
                    attributes: null);

                div.InnerHtml.AppendHtml(itemFormGroup);

                return div;
            }
        }

        public virtual TagBuilder GenerateErrorSummary(
            IHtmlContent titleContent,
            IDictionary<string, string> titleAttributes,
            IHtmlContent descriptionContent,
            IDictionary<string, string> descriptionAttributes,
            IDictionary<string, string> attributes,
            IEnumerable<ErrorSummaryItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-error-summary");
            tagBuilder.Attributes.Add("aria-labelledby", "error-summary-title");
            tagBuilder.Attributes.Add("role", "alert");
            tagBuilder.Attributes.Add("tabindex", "-1");
            tagBuilder.Attributes.Add("data-module", "govuk-error-summary");

            var heading = new TagBuilder("h2");
            heading.MergeAttributes(titleAttributes);
            heading.MergeCssClass("govuk-error-summary__title");
            heading.Attributes.Add("id", "error-summary-title");
            heading.InnerHtml.AppendHtml(titleContent);
            tagBuilder.InnerHtml.AppendHtml(heading);

            var body = new TagBuilder("div");
            body.MergeCssClass("govuk-error-summary__body");

            if (descriptionContent != null)
            {
                var p = new TagBuilder("p");
                p.MergeAttributes(descriptionAttributes);
                p.InnerHtml.AppendHtml(descriptionContent);
                body.InnerHtml.AppendHtml(p);
            }

            var ul = new TagBuilder("ul");
            ul.MergeCssClass("govuk-list");
            ul.MergeCssClass("govuk-error-summary__list");

            foreach (var item in items)
            {
                var li = new TagBuilder("li");
                li.MergeAttributes(item.Attributes);
                li.InnerHtml.AppendHtml(item.Content);
                ul.InnerHtml.AppendHtml(li);
            }

            body.InnerHtml.AppendHtml(ul);

            tagBuilder.InnerHtml.AppendHtml(body);

            return tagBuilder;
        }

        public virtual TagBuilder GenerateFormGroup(
            bool haveError,
            IHtmlContent content,
            IDictionary<string, string> attributes)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder(FormGroupElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-form-group");

            if (haveError)
            {
                tagBuilder.MergeCssClass("govuk-form-group--error");
            }

            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }

        public virtual TagBuilder GenerateRadios(
            string name,
            bool isConditional,
            IEnumerable<RadiosItemBase> items,
            IDictionary<string, string> attributes)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-radios");

            if (isConditional)
            {
                tagBuilder.MergeCssClass("govuk-radios--conditional");
                tagBuilder.Attributes.Add("data-module", "govuk-radios");
            }

            foreach (var item in items)
            {
                if (item is RadiosItemDivider divider)
                {
                    var dividerContent = GenerateRadioItemDivider(divider);
                    tagBuilder.InnerHtml.AppendHtml(dividerContent);
                }
                else if (item is RadiosItem i)
                {
                    AddCheckboxItem(i, tagBuilder.InnerHtml);
                }
                else
                {
                    throw new NotSupportedException($"Unknown item type: '{item.GetType().FullName}'.");
                }
            }

            return tagBuilder;

            void AddCheckboxItem(RadiosItem item, IHtmlContentBuilder container)
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
                tagBuilder.MergeAttributes(item.Attributes);
                tagBuilder.MergeCssClass("govuk-radios__item");

                var input = new TagBuilder("input");
                input.TagRenderMode = TagRenderMode.SelfClosing;
                input.MergeAttributes(item.InputAttributes);
                input.MergeCssClass("govuk-radios__input");
                input.Attributes.Add("id", item.Id);
                input.Attributes.Add("name", name);
                input.Attributes.Add("type", "radio");
                input.Attributes.Add("value", item.Value);

                if (item.IsChecked)
                {
                    input.Attributes.Add("checked", "checked");
                }

                if (item.IsDisabled)
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

                var label = GenerateLabel(item.Id, isPageHeading: false, content: item.Content, attributes: null);
                label.MergeCssClass("govuk-radios__label");
                tagBuilder.InnerHtml.AppendHtml(label);

                if (item.HintContent != null)
                {
                    var hint = GenerateHint(item.HintId, item.HintContent, item.HintAttributes);
                    hint.MergeCssClass("govuk-radios__hint");
                    tagBuilder.InnerHtml.AppendHtml(hint);
                }

                container.AppendHtml(tagBuilder);

                if (item.ConditionalContent != null)
                {
                    var conditional = new TagBuilder("div");
                    conditional.MergeAttributes(item.ConditionalAttributes);
                    conditional.MergeCssClass("govuk-radios__conditional");

                    if (!item.IsChecked)
                    {
                        conditional.MergeCssClass("govuk-radios__conditional--hidden");
                    }

                    conditional.Attributes.Add("id", item.ConditionalId);

                    conditional.InnerHtml.AppendHtml(item.ConditionalContent);

                    container.AppendHtml(conditional);
                }
            }

            IHtmlContent GenerateRadioItemDivider(RadiosItemDivider divider)
            {
                var tagBuilder = new TagBuilder("div");
                tagBuilder.MergeAttributes(divider.Attributes);
                tagBuilder.MergeCssClass("govuk-radios__divider");
                tagBuilder.InnerHtml.AppendHtml(divider.Content);
                return tagBuilder;
            }
        }

        public virtual TagBuilder GenerateTabs(
            string id,
            string title,
            IDictionary<string, string> attributes,
            IEnumerable<TabsItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            var tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-tabs");
            tagBuilder.Attributes.Add("data-module", "govuk-tabs");

            if (id != null)
            {
                tagBuilder.Attributes.Add("id", id);
            }

            var h2 = new TagBuilder("h2");
            h2.MergeCssClass("govuk-tabs__title");
            h2.InnerHtml.Append(title);
            tagBuilder.InnerHtml.AppendHtml(h2);

            var ul = new TagBuilder("ul");
            ul.MergeCssClass("govuk-tabs__list");

            foreach (var item in items)
            {
                var li = new TagBuilder("li");
                li.MergeCssClass("govuk-tabs__list-item");

                if (item == items.First())
                {
                    li.MergeCssClass("govuk-tabs__list-item--selected");
                }

                var a = new TagBuilder("a");
                a.MergeCssClass("govuk-tabs__tab");
                a.Attributes.Add("href", $"#{item.Id}");
                a.InnerHtml.Append(item.Label);
                li.InnerHtml.AppendHtml(a);

                ul.InnerHtml.AppendHtml(li);
            }

            tagBuilder.InnerHtml.AppendHtml(ul);

            foreach (var item in items)
            {
                var section = new TagBuilder("section");
                section.MergeAttributes(item.PanelAttributes);
                section.MergeCssClass("govuk-tabs__panel");
                section.Attributes.Add("id", item.Id);

                if (item != items.First())
                {
                    section.MergeCssClass("govuk-tabs__panel--hidden");
                }

                section.InnerHtml.AppendHtml(item.PanelContent);

                tagBuilder.InnerHtml.AppendHtml(section);
            }

            return tagBuilder;
        }

        public virtual TagBuilder GenerateTextArea(
            bool haveError,
            string id,
            string name,
            int rows,
            string describedBy,
            string autocomplete,
            bool? spellcheck,
            bool disabled,
            IHtmlContent content,
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

            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var tagBuilder = new TagBuilder("textarea");
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-textarea");

            if (haveError)
            {
                tagBuilder.MergeCssClass("govuk-textarea--error");
            }

            tagBuilder.Attributes.Add("id", id);
            tagBuilder.Attributes.Add("name", name);
            tagBuilder.Attributes.Add("rows", rows.ToString());

            if (describedBy != null)
            {
                tagBuilder.Attributes.Add("aria-describedby", describedBy);
            }

            if (autocomplete != null)
            {
                tagBuilder.Attributes.Add("autocomplete", autocomplete);
            }

            if (spellcheck.HasValue)
            {
                tagBuilder.Attributes.Add("spellcheck", spellcheck.Value ? "true" : "false");
            }

            if (disabled)
            {
                tagBuilder.Attributes.Add("disabled", "disabled");
            }

            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }

        private static void AppendToDescribedBy(ref string describedBy, string value)
        {
            if (value == null)
            {
                return;
            }

            if (describedBy == null)
            {
                describedBy = value;
            }
            else
            {
                describedBy += $" {value}";
            }
        }
    }
}
