using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-panel")]
    public class PanelTagHelper : TagHelper
    {
        private const string HeadingLevelAttributeName = "heading-level";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public PanelTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        [HtmlAttributeName(HeadingLevelAttributeName)]
        public int? HeadingLevel { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (HeadingLevel != null && (HeadingLevel < 1 || HeadingLevel > 6))
            {
                throw new InvalidOperationException($"The '{HeadingLevelAttributeName}' attribute must be between 1 and 6.");
            }

            var panelContext = new PanelContext();

            IHtmlContent childContent;
            using (context.SetScopedContextItem(typeof(PanelContext), panelContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            if (panelContext.Title == null)
            {
                throw new InvalidOperationException("Missing <govuk-panel-title> element.");
            }

            var tagBuilder = _htmlGenerator.GeneratePanel(
                HeadingLevel,
                panelContext.Title,
                childContent,
                output.Attributes.ToAttributesDictionary());

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
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var panelContext = (PanelContext)context.Items[typeof(PanelContext)];

            var content = await output.GetChildContentAsync();

            if (!panelContext.TrySetTitle(content.Snapshot()))
            {
                throw new InvalidOperationException($"Cannot render <{output.TagName}> here.");
            }

            output.SuppressOutput();
        }
    }

    internal class PanelContext
    {
        public IHtmlContent Title { get; private set; }

        public bool TrySetTitle(IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Title != null)
            {
                return false;
            }

            Title = content;
            return true;
        }
    }
}
