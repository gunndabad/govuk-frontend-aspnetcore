using GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class RazorGenerator
    {
        public string GenerateButton(Button options)
        {
            var tagName = options.Element == "a" || options.Href != null ? "govuk-button-link" : "govuk-button";

            var button = new TagBuilder(tagName);

            button.AddAttributes(options.Attributes);
            button.AddCssClass(options.Classes);

            if (options.Element == "a" && options.Href == null)
            {
                options.Href = "#";
            }

            if (options.Name != null && options.Href == null)
            {
                button.Attributes.Add("name", options.Name);
            }

            if (options.Type != null)
            {
                button.Attributes.Add("type", options.Type);
            }

            if (options.Value != null)
            {
                button.Attributes.Add("value", options.Value);
            }

            if (options.Disabled.HasValue)
            {
                button.AddAttribute("disabled", options.Disabled.Value);
            }

            if (options.Href != null)
            {
                button.Attributes.Add("href", options.Href);
            }

            if (options.PreventDoubleClick.HasValue)
            {
                button.AddAttribute("prevent-double-click", options.PreventDoubleClick.Value);
            }

            if (options.IsStartButton.HasValue)
            {
                button.AddAttribute("is-start-button", options.IsStartButton.Value);
            }

            var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html);
            button.InnerHtml.AppendHtml(content);

            return button.RenderToString();
        }
    }
}
