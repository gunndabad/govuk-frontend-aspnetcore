using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.TagHelperComponents
{
    public class GdsImportsTagHelperComponent : TagHelperComponent
    {
        private readonly GovUkFrontendAspNetCoreOptions _options;

        public GdsImportsTagHelperComponent(IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
        {
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        }

        public override int Order => 1;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!_options.AddImportsToHtml || ViewContext.ViewData.ContainsKey(nameof(NoAppendHtmlSnippetsMarker)))
            {
                return;
            }

            if (string.Equals(context.TagName, "head", StringComparison.OrdinalIgnoreCase))
            {
                output.PostContent.AppendHtml(HtmlSnippets.StyleImports + "\n");
            }
            else if (string.Equals(context.TagName, "body", StringComparison.OrdinalIgnoreCase))
            {
                output.AddClass("govuk-template__body", HtmlEncoder.Default);

                output.PreContent.AppendHtml("\n" + HtmlSnippets.BodyInitScript);
                output.PostContent.AppendHtml(HtmlSnippets.ScriptImports + "\n");
            }
        }
    }
}
