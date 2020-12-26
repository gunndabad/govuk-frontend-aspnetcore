using GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class RazorGenerator
    {
        public string GenerateTag(Tag options)
        {
            var tag = new TagBuilder("govuk-tag");

            tag.AddAttributes(options.Attributes);
            tag.AddCssClass(options.Classes);

            var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html);
            tag.InnerHtml.AppendHtml(content);

            return tag.RenderToString();
        }
    }
}
