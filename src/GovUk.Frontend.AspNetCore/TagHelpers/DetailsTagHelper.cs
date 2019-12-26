using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-details", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("govuk-details-summary", "govuk-details-text")]
    public class DetailsTagHelper : TagHelper
    {
        private const string IdAttributeName = "id";
        private const string OpenAttributeName = "open";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public DetailsTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        [HtmlAttributeName(OpenAttributeName)]
        public bool Open { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var detailsContext = new DetailsContext();
            context.Items.Add(DetailsContext.ContextName, detailsContext);

            await output.GetChildContentAsync();

            detailsContext.ThrowIfStagesIncomplete();

            var tagBuilder = _htmlGenerator.GenerateDetails(
                Open,
                detailsContext.Summary,
                detailsContext.Text);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            if (!string.IsNullOrEmpty(Id))
            {
                output.Attributes.Add("id", Id);
            }

            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }

    [HtmlTargetElement("govuk-details-summary", ParentTag = "govuk-details", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class DetailsSummaryTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var detailsContext = (DetailsContext)context.Items[DetailsContext.ContextName];
            detailsContext.SetSummary(childContent);

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-details-text", ParentTag = "govuk-details", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class DetailsTextTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var detailsContext = (DetailsContext)context.Items[DetailsContext.ContextName];
            detailsContext.SetText(childContent);

            output.SuppressOutput();
        }
    }

    internal enum DetailsRenderStage
    {
        None = 0,
        Summary = 1,
        Text = 2
    }

    internal class DetailsContext
    {
        public const string ContextName = nameof(DetailsContext);

        public IHtmlContent Summary { get; private set; }

        public IHtmlContent Text { get; private set; }

        // Internal for testing
        internal DetailsRenderStage RenderStage { get; private set; }

        public void SetSummary(IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (RenderStage != DetailsRenderStage.None)
            {
                throw new InvalidOperationException("Cannot render <govuk-details-summary> here.");
            }

            Summary = content;
            RenderStage = DetailsRenderStage.Summary;
        }

        public void SetText(IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (RenderStage != DetailsRenderStage.Summary)
            {
                throw new InvalidOperationException("Cannot render <govuk-details-text> here.");
            }

            Text = content;
            RenderStage = DetailsRenderStage.Text;
        }

        public void ThrowIfStagesIncomplete()
        {
            if (RenderStage != DetailsRenderStage.Text)
            {
                throw new InvalidOperationException("Missing one or more child elements.");
            }
        }
    }
}
