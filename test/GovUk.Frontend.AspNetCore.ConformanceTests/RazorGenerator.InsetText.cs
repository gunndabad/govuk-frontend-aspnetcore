using GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class RazorGenerator
    {
        public string GenerateInsetText(InsetText options)
        {
            var insetText = new TagBuilder("govuk-inset-text");

            insetText.AddAttributes(options.Attributes);
            insetText.AddCssClass(options.Classes);

            if (options.Id != null)
            {
                insetText.Attributes.Add("id", options.Id);
            }

            var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html);
            insetText.InnerHtml.AppendHtml(content);

            return insetText.RenderToString();
        }
    }
}
