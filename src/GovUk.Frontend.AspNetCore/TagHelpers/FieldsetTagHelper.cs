using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-fieldset", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FieldsetTagHelper : TagHelper
    {
        private const string DescribedByAttributeName = "described-by";
        private const string IsPageHeadingAttributeName = "is-page-heading";
        private const string RoleAttributeName = "role";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public FieldsetTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator;
        }

        [HtmlAttributeName(DescribedByAttributeName)]
        public string DescribedBy { get; set; }

        [HtmlAttributeName(IsPageHeadingAttributeName)]
        public bool IsPageHeading { get; set; }

        [HtmlAttributeName(RoleAttributeName)]
        public string Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fieldsetContext = new FieldsetContext();
            using (context.SetScopedContextItem(FieldsetContext.ContextName, fieldsetContext))
            {
                var childContent = await output.GetChildContentAsync();

                var tagBuilder = _htmlGenerator.GenerateFieldset(
                    DescribedBy,
                    IsPageHeading,
                    Role,
                    fieldsetContext.LegendContent,
                    childContent);

                output.TagName = tagBuilder.TagName;
                output.TagMode = TagMode.StartTagAndEndTag;

                output.MergeAttributes(tagBuilder);
                output.Content.SetHtmlContent(tagBuilder.InnerHtml);
            }
        }
    }

    [HtmlTargetElement("govuk-fieldset-legend", ParentTag = "govuk-fieldset")]
    public class FieldsetLegendTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fieldsetContext = (FieldsetContext)context.Items[FieldsetContext.ContextName];

            var content = await output.GetChildContentAsync();

            // We have to copy the content here since something in the underlying structure gets re-used
            // before we've had a change to consume it.
            var copiedContent = new HtmlString(content.GetContent());

            if (!fieldsetContext.TrySetLegendContent(copiedContent))
            {
                throw new InvalidOperationException($"Cannot render <{context.TagName}> here.");
            }
        }
    }

    public class FieldsetContext
    {
        public const string ContextName = nameof(FieldsetContext);

        public IHtmlContent LegendContent { get; private set; }

        public bool TrySetLegendContent(IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (LegendContent != null)
            {
                return false;
            }

            LegendContent = content;
            return true;
        }
    }
}
