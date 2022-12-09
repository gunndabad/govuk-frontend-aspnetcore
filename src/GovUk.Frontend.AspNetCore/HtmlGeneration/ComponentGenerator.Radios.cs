using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    internal partial class ComponentGenerator
    {
        internal const string RadiosElement = "div";
        internal const string RadiosDividerItemElement = "div";
        internal const string RadiosItemElement = "div";
        internal const bool RadiosItemDefaultChecked = false;
        internal const bool RadiosItemDefaultDisabled = false;

        public TagBuilder GenerateRadios(
            string? idPrefix,
            string name,
            IEnumerable<RadiosItemBase> items,
            AttributeDictionary attributes)
        {
            Guard.ArgumentNotNull(nameof(name), name);
            Guard.ArgumentNotNull(nameof(items), items);

            idPrefix ??= name;

            var tagBuilder = new TagBuilder(RadiosElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-radios");
            tagBuilder.Attributes.Add("data-module", "govuk-radios");

            var itemIndex = 0;
            foreach (var item in items)
            {
                if (item is RadiosItem i)
                {
                    AddItem(i);
                }
                else if (item is RadiosItemDivider divider)
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

            void AddItem(RadiosItem item)
            {
                Guard.ArgumentValidNotNull(
                    nameof(items),
                    $"Item {itemIndex} is not valid; {nameof(CheckboxesItem.Value)} cannot be null.",
                    item.Value,
                    item.Value != null);

                var itemId = item.Id ?? (itemIndex == 0 ? idPrefix : $"{idPrefix}-{itemIndex + 1}");
                var hintId = itemId + "-item-hint";
                var conditionalId = "conditional-" + itemId;

                var itemTagBuilder = new TagBuilder(RadiosItemElement);
                itemTagBuilder.MergeOptionalAttributes(item.Attributes);
                itemTagBuilder.MergeCssClass("govuk-radios__item");

                var input = new TagBuilder("input");
                input.TagRenderMode = TagRenderMode.SelfClosing;
                input.MergeOptionalAttributes(item.InputAttributes);
                input.MergeCssClass("govuk-radios__input");
                input.Attributes.Add("id", itemId);
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

                if (item.Conditional != null)
                {
                    input.Attributes.Add("data-aria-controls", conditionalId);
                }

                if (item.Hint != null)
                {
                    input.Attributes.Add("aria-describedby", hintId);
                }

                itemTagBuilder.InnerHtml.AppendHtml(input);

                var label = GenerateLabel(itemId, isPageHeading: false, content: item.LabelContent, item.LabelAttributes)!;
                label.MergeCssClass("govuk-radios__label");
                itemTagBuilder.InnerHtml.AppendHtml(label);

                if (item.Hint != null)
                {
                    Guard.ArgumentValidNotNull(
                        nameof(items),
                        $"Item {itemIndex} is not valid; {nameof(RadiosItem.Hint)}.{nameof(RadiosItemHint.Content)} cannot be null.",
                        item.Hint.Content,
                        item.Hint.Content != null);

                    var hint = GenerateHint(hintId, item.Hint.Content, item.Hint.Attributes);
                    hint.MergeCssClass("govuk-radios__hint");
                    itemTagBuilder.InnerHtml.AppendHtml(hint);
                }

                tagBuilder.InnerHtml.AppendHtml(itemTagBuilder);

                if (item.Conditional != null)
                {
                    Guard.ArgumentValidNotNull(
                        nameof(items),
                        $"Item {itemIndex} is not valid; {nameof(CheckboxesItem.Conditional.Content)} cannot be null.",
                        item.Conditional.Content,
                        item.Conditional.Content != null);

                    var conditional = new TagBuilder("div");
                    conditional.MergeOptionalAttributes(item.Conditional.Attributes);
                    conditional.MergeCssClass("govuk-radios__conditional");

                    if (!item.Checked)
                    {
                        conditional.MergeCssClass("govuk-radios__conditional--hidden");
                    }

                    conditional.Attributes.Add("id", conditionalId);

                    conditional.InnerHtml.AppendHtml(item.Conditional.Content);

                    tagBuilder.InnerHtml.AppendHtml(conditional);
                }
            }

            void AddDivider(RadiosItemDivider divider)
            {
                Guard.ArgumentValidNotNull(
                    nameof(items),
                    $"Item {itemIndex} is not valid; {nameof(CheckboxesItemDivider.Content)} cannot be null.",
                    divider.Content,
                    divider.Content != null);

                var dividerTagBuilder = new TagBuilder(RadiosDividerItemElement);
                dividerTagBuilder.MergeOptionalAttributes(divider.Attributes);
                dividerTagBuilder.MergeCssClass("govuk-radios__divider");
                dividerTagBuilder.InnerHtml.AppendHtml(divider.Content);
                
                tagBuilder.InnerHtml.AppendHtml(dividerTagBuilder);
            }
        }
    }
}
