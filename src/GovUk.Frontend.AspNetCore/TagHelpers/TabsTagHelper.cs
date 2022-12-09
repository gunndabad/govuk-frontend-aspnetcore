using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-tabs", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("govuk-tabs-item")]
    public class TabsTagHelper : TagHelper
    {
        internal const string IdPrefixAttributeName = "id-prefix";
        private const string IdAttributeName = "id";
        private const string TitleAttributeName = "title";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public TabsTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = Guard.ArgumentNotNull(nameof(htmlGenerator), htmlGenerator);
        }

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        [HtmlAttributeName(IdPrefixAttributeName)]
        public string IdPrefix { get; set; }

        [HtmlAttributeName(TitleAttributeName)]
        public string Title { get; set; } = ComponentDefaults.Tabs.Title;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var tabsContext = new TabsContext(IdPrefix);

            using (context.SetScopedContextItem(typeof(TabsContext), tabsContext))
            {
                await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateTabs(Id, Title, output.Attributes.ToAttributeDictionary(), tabsContext.Items);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }

    [HtmlTargetElement("govuk-tabs-item", ParentTag = "govuk-tabs", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class TabsItemTagHelper : TagHelper
    {
        private const string IdAttributeName = "id";
        private const string LabelAttributeName = "label";

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        [HtmlAttributeName(LabelAttributeName)]
        public string Label { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Label == null)
            {
                throw new InvalidOperationException($"The '{LabelAttributeName}' attribute must be specified.");
            }

            var tabsContext = (TabsContext)context.Items[typeof(TabsContext)];

            if (Id == null && tabsContext.IdPrefix == null)
            {
                throw new InvalidOperationException(
                    $"Item must have the '{IdAttributeName}' attribute specified when its parent doesn't specify the '{TabsTagHelper.IdPrefixAttributeName}' attribute.");
            }

            var index = tabsContext.Items.Count;
            var resolvedId = Id ?? $"{tabsContext.IdPrefix}-{index}";

            var content = await output.GetChildContentAsync();

            tabsContext.AddItem(new TabsItem()
            {
                Id = resolvedId,
                Label = Label,
                PanelAttributes = output.Attributes.ToAttributeDictionary(),
                PanelContent = content.Snapshot()
            });

            output.SuppressOutput();
        }
    }

    internal class TabsContext
    {
        private readonly List<TabsItem> _items;

        public TabsContext(string idPrefix)
        {
            IdPrefix = idPrefix;
            _items = new List<TabsItem>();
        }

        public string IdPrefix { get; }
        public IReadOnlyList<TabsItem> Items => _items;

        public void AddItem(TabsItem item)
        {
            Guard.ArgumentNotNull(nameof(item), item);

            _items.Add(item);
        }
    }
}
