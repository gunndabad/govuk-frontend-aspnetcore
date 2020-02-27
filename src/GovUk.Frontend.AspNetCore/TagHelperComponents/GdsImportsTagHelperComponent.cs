using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelperComponents
{
    public class GdsImportsTagHelperComponent : TagHelperComponent
    {
        private const string StyleImports = @"
<!--[if !IE 8]><!-->
    <link rel=""stylesheet"" href=""/govuk-frontend-3.5.0.min.css"">
<!--<![endif]-->
<!--[if IE 8]>
    <link rel = ""stylesheet"" href=""/govuk-frontend-ie8-3.5.0.min.css"">
<![endif]-->";

        private const string ScriptImports = @"
<script src=""/govuk-frontend-3.5.0.min.js""></script>
<script>window.GOVUKFrontend.initAll()</script>";

        public override int Order => 1;

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.Equals(context.TagName, "head", StringComparison.OrdinalIgnoreCase))
            {
                output.PostContent.AppendHtml(StyleImports);
            }
            else if (string.Equals(context.TagName, "body", StringComparison.OrdinalIgnoreCase))
            {
                output.AddClass("govuk-template__body", HtmlEncoder.Default);

                output.PostContent.AppendHtml(ScriptImports);
            }

            return Task.CompletedTask;
        }
    }
}
