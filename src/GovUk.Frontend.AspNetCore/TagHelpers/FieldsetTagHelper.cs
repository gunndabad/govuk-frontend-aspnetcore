using System;
using System.Collections.Generic;
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

            IHtmlContent childContent;
            using (context.SetScopedContextItem(FieldsetContext.ContextName, fieldsetContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateFieldset(
                DescribedBy,
                IsPageHeading,
                Role,
                fieldsetContext.Legend.Value.content,
                fieldsetContext.Legend.Value.attributes,
                childContent,
                output.Attributes.ToAttributesDictionary());

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }

    [HtmlTargetElement("govuk-fieldset-legend", ParentTag = "govuk-fieldset")]
    public class FieldsetLegendTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fieldsetContext = (FieldsetContext)context.Items[FieldsetContext.ContextName];

            var childContent = await output.GetChildContentAsync();

            if (!fieldsetContext.TrySetLegend(output.Attributes.ToAttributesDictionary(), childContent.Snapshot()))
            {
                throw new InvalidOperationException($"Cannot render <{context.TagName}> here.");
            }

            output.SuppressOutput();
        }
    }

    public class FieldsetContext
    {
        public const string ContextName = nameof(FieldsetContext);

        public (IDictionary<string, string> attributes, IHtmlContent content)? Legend { get; private set; }

        public bool TrySetLegend(IDictionary<string, string> attributes, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Legend != null)
            {
                return false;
            }

            Legend = (attributes, content);
            return true;
        }
    }
}
