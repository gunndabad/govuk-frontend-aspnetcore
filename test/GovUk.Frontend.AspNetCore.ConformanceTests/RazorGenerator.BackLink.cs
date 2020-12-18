using GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class RazorGenerator
    {
        public string GenerateBackLink(BackLink options)
        {
            var backLink = new TagBuilder("govuk-back-link");

            backLink.AddCssClass(options.Classes);
            backLink.MergeAttributes(options.Attributes);

            if (options.Href != null)
            {
                backLink.Attributes.Add("href", options.Href);
            }

            var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html);
            if (content == null)
            {
                backLink.TagRenderMode = TagRenderMode.SelfClosing;
            }
            else
            {
                backLink.InnerHtml.AppendHtml(content);
            }

            return backLink.RenderToString();
        }
    }
}
