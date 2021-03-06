﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-phase-banner", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PhaseBannerTagHelper : TagHelper
    {
        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public PhaseBannerTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var pbContext = new PhaseBannerContext();

            TagHelperContent childContent;
            using (context.SetScopedContextItem(typeof(PhaseBannerContext), pbContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            if (!pbContext.Tag.HasValue)
            {
                throw new InvalidOperationException($"You must specify a <govuk-phase-banner-tag> child element.");
            }

            var tagBuilder = _htmlGenerator.GeneratePhaseBanner(
                pbContext.Tag.Value.content,
                pbContext.Tag.Value.attributes,
                childContent,
                output.Attributes.ToAttributesDictionary());

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }

    [HtmlTargetElement("govuk-phase-banner-tag", ParentTag = "govuk-phase-banner")]
    public class PhaseBannerTagTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var pbContext = (PhaseBannerContext)context.Items[typeof(PhaseBannerContext)];

            var childContent = await output.GetChildContentAsync();

            if (!pbContext.TrySetTag(output.Attributes.ToAttributesDictionary(), childContent.Snapshot()))
            {
                throw new InvalidOperationException($"Cannot render <{output.TagName}> here.");
            }

            output.SuppressOutput();
        }
    }

    internal class PhaseBannerContext
    {
        public (IDictionary<string, string> attributes, IHtmlContent content)? Tag { get; private set; }

        public bool TrySetTag(IDictionary<string, string> attributes, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Tag != null)
            {
                return false;
            }

            Tag = (attributes, content);
            return true;
        }
    }
}
