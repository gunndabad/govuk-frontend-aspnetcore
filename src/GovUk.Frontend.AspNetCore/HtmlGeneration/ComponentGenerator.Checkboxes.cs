#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string CheckboxesElement = "div";
        internal const string CheckboxesDividerItemElement = "div";
        internal const string CheckboxesItemElement = "div";
        internal const CheckboxesItemBehavior CheckboxesItemDefaultBehavior = CheckboxesItemBehavior.Default;
        internal const bool CheckboxesItemDefaultChecked = false;
        internal const bool CheckboxesItemDefaultDisabled = false;

        public TagBuilder GenerateCheckboxes(
            string idPrefix,
            string? name,
            string? describedBy,
            bool hasFieldset,
            IEnumerable<CheckboxesItemBase> items,
            IDictionary<string, string>? attributes)
        {
            Guard.ArgumentNotNull(nameof(idPrefix), idPrefix);
            Guard.ArgumentNotNull(nameof(items), items);

            var isConditional = items.OfType<CheckboxesItem>().Any(i => i.Conditional != null);

            var tagBuilder = new TagBuilder(CheckboxesElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-checkboxes");
            tagBuilder.Attributes.Add("data-module", "govuk-checkboxes");

            var itemIndex = 0;
            foreach (var item in items)
            {
                if (item is CheckboxesItem checkboxesItem)
                {
                    AddItem(checkboxesItem);
                }
                else if (item is CheckboxesItemDivider divider)
                {
                    AddDivider(divider);
                }
                else
                {
                    throw new NotSupportedException($"Unknown item type: '{item.GetType().FullName}'.");
                }

                itemIndex++;
            }

            return tagBuilder;

            void AddDivider(CheckboxesItemDivider divider)
            {
                var itemTagBuilder = new TagBuilder(CheckboxesDividerItemElement);
                itemTagBuilder.MergeAttributes(divider.Attributes);
                itemTagBuilder.MergeCssClass("govuk-checkboxes__divider");
                itemTagBuilder.InnerHtml.AppendHtml(divider.Content);

                tagBuilder.InnerHtml.AppendHtml(itemTagBuilder);
            }

            void AddItem(CheckboxesItem item)
            {
                Guard.ArgumentValidNotNull(
                    nameof(items),
                    $"Item {itemIndex} is not valid; {nameof(CheckboxesItem.Value)} cannot be null.",
                    item.Value,
                    item.Value != null);

                Guard.ArgumentValid(
                    nameof(items),
                    $"Item {itemIndex} is not valid; {nameof(CheckboxesItem.Name)} cannot be null when {nameof(name)} is null.",
                    item.Name != null || name != null);

                var itemId = item.Id ?? (itemIndex == 0 ? idPrefix : $"{idPrefix}-{itemIndex + 1}");
                var hintId = itemId + "-item-hint";
                var conditionalId = "conditional-" + itemId;

                var itemName = item.Name ?? name;

                var itemTagBuilder = new TagBuilder(CheckboxesItemElement);
                itemTagBuilder.MergeAttributes(item.Attributes);
                itemTagBuilder.MergeCssClass("govuk-checkboxes__item");

                var input = new TagBuilder("input");
                input.MergeAttributes(item.InputAttributes);
                input.TagRenderMode = TagRenderMode.SelfClosing;
                input.MergeCssClass("govuk-checkboxes__input");
                input.Attributes.Add("id", itemId);
                input.Attributes.Add("name", itemName);
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

                if (item.Conditional != null)
                {
                    input.Attributes.Add("data-aria-controls", conditionalId);
                }

                if (item.Behavior == CheckboxesItemBehavior.Exclusive)
                {
                    input.Attributes.Add("data-behaviour", "exclusive");
                }

                var itemDescribedBy = !hasFieldset ? describedBy : null;

                if (item.Hint != null)
                {
                    AppendToDescribedBy(ref itemDescribedBy, hintId);
                }

                if (itemDescribedBy != null)
                {
                    input.Attributes.Add("aria-describedby", itemDescribedBy);
                }

                itemTagBuilder.InnerHtml.AppendHtml(input);

                var label = GenerateLabel(itemId, isPageHeading: false, content: item.LabelContent, item.LabelAttributes);
                label?.MergeCssClass("govuk-checkboxes__label");
                itemTagBuilder.InnerHtml.AppendHtml(label);

                if (item.Hint != null)
                {
                    Guard.ArgumentValidNotNull(
                        nameof(items),
                        $"Item {itemIndex} is not valid; {nameof(CheckboxesItem.Hint)}.{nameof(CheckboxesItemHint.Content)} cannot be null.",
                        item.Hint.Content,
                        item.Hint.Content != null);

                    var hint = GenerateHint(hintId, item.Hint.Content, item.Hint.Attributes);
                    hint.MergeCssClass("govuk-checkboxes__hint");
                    itemTagBuilder.InnerHtml.AppendHtml(hint);
                }

                tagBuilder.InnerHtml.AppendHtml(itemTagBuilder);

                if (item.Conditional != null)
                {
                    var conditional = new TagBuilder("div");
                    conditional.MergeAttributes(item.Conditional.Attributes);
                    conditional.MergeCssClass("govuk-checkboxes__conditional");

                    if (!item.Checked)
                    {
                        conditional.MergeCssClass("govuk-checkboxes__conditional--hidden");
                    }

                    conditional.Attributes.Add("id", conditionalId);

                    conditional.InnerHtml.AppendHtml(item.Conditional.Content);

                    tagBuilder.InnerHtml.AppendHtml(conditional);
                }
            }
        }
    }
}
