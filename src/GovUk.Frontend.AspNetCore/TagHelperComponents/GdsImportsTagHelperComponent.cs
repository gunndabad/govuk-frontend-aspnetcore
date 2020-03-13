using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelperComponents
{
    public class GdsImportsTagHelperComponent : TagHelperComponent
    {
        public override int Order => 1;

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.Equals(context.TagName, "head", StringComparison.OrdinalIgnoreCase))
            {
                output.PostContent.AppendHtml(HtmlSnippets.StyleImports);
            }
            else if (string.Equals(context.TagName, "body", StringComparison.OrdinalIgnoreCase))
            {
                output.AddClass("govuk-template__body", HtmlEncoder.Default);

                output.PreContent.AppendHtml(HtmlSnippets.BodyInitScript);
                output.PostContent.AppendHtml(HtmlSnippets.ScriptImports);
            }

            return Task.CompletedTask;
        }
    }
}
