using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-panel")]
    public class PanelTagHelper : TagHelper
    {
        private const string AttributesPrefix = "panel-";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public PanelTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var panelContext = new PanelContext();

            IHtmlContent childContent;
            using (context.SetScopedContextItem(PanelContext.ContextName, panelContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            if (panelContext.Title == null)
            {
                throw new InvalidOperationException("Missing <govuk-panel-title> element.");
            }

            var tagBuilder = _htmlGenerator.GeneratePanel(
                panelContext.Title.Value.headingLevel,
                panelContext.Title.Value.content,
                Attributes,
                childContent);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }

    [HtmlTargetElement("govuk-panel-title", ParentTag = "govuk-panel")]
    public class PanelTitleTagHelper : TagHelper
    {
        private const string HeadingLevelAttributeName = "heading-level";

        [HtmlAttributeName(HeadingLevelAttributeName)]
        public int? HeadingLevel { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (HeadingLevel != null && (HeadingLevel < 1 || HeadingLevel > 6))
            {
                throw new InvalidOperationException($"The '{HeadingLevelAttributeName}' attribute must be between 1 and 6.");
            }

            var panelContext = (PanelContext)context.Items[PanelContext.ContextName];

            var content = await output.GetChildContentAsync();

            if (!panelContext.TrySetHeading(HeadingLevel, content.Snapshot()))
            {
                throw new InvalidOperationException($"Cannot render <{output.TagName}> here.");
            }

            output.SuppressOutput();
        }
    }

    internal class PanelContext
    {
        public const string ContextName = nameof(PanelContext);

        public (int? headingLevel, IHtmlContent content)? Title { get; private set; }

        public bool TrySetHeading(int? headingLevel, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Title != null)
            {
                return false;
            }

            Title = (headingLevel, content);
            return true;
        }
    }
}
