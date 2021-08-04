#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const bool SelectDefaultDisabled = false;
        internal const bool SelectItemDefaultDisabled = false;
        internal const bool SelectItemDefaultSelected = false;
        internal const string SelectItemDefaultValue = "";

        public TagBuilder GenerateSelect(
            bool haveError,
            string id,
            string name,
            string? describedBy,
            bool disabled,
            IEnumerable<SelectItem> items,
            IDictionary<string, string>? attributes)
        {
            Guard.ArgumentNotNull(nameof(id), id);
            Guard.ArgumentNotNull(nameof(name), name);
            Guard.ArgumentNotNull(nameof(items), items);

            var tagBuilder = new TagBuilder("select");
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-select");

            if (haveError)
            {
                tagBuilder.MergeCssClass("govuk-select--error");
            }

            tagBuilder.Attributes.Add("id", id);
            tagBuilder.Attributes.Add("name", name);

            if (describedBy != null)
            {
                tagBuilder.Attributes.Add("aria-describedby", describedBy);
            }

            var index = 0;
            foreach (var item in items)
            {
                Guard.ArgumentValidNotNull(
                    nameof(items),
                    $"Item {index} is not valid; {nameof(SelectItem.Content)} cannot be null.",
                    item.Content,
                    item.Content != null);

                var option = new TagBuilder("option");
                option.MergeAttributes(item.Attributes);

                if (item.Value != null)
                {
                    option.Attributes.Add("value", item.Value);
                }

                if (item.Selected)
                {
                    option.Attributes.Add("selected", "selected");
                }

                if (item.Disabled)
                {
                    option.Attributes.Add("disabled", "disabled");
                }

                option.InnerHtml.AppendHtml(item.Content);

                tagBuilder.InnerHtml.AppendHtml(option);

                index++;
            }

            return tagBuilder;
        }
    }
}
