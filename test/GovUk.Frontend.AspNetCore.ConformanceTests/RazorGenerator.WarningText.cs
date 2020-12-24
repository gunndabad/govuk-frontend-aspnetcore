using GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class RazorGenerator
    {
        public string GenerateWarningText(WarningText options)
        {
            var warningText = new TagBuilder("govuk-warning-text");
            warningText.Attributes.Add("icon-fallback-text", options.IconFallbackText ?? "GFA_test");

            warningText.AddAttributes(options.Attributes);
            warningText.AddCssClass(options.Classes);

            var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html);
            warningText.InnerHtml.AppendHtml(content);

            return warningText.RenderToString();
        }
    }
}
