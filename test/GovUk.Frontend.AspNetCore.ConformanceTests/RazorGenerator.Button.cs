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

            button.AddCssClass(options.Classes);
            button.MergeAttributes(options.Attributes);

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
                button.Attributes.Add("disabled", options.Disabled.Value ? "true" : "false");
            }

            if (options.Href != null)
            {
                button.Attributes.Add("href", options.Href);
            }

            if (options.PreventDoubleClick.HasValue)
            {
                button.Attributes.Add("prevent-double-click", options.PreventDoubleClick.Value ? "true" : "false");
            }

            if (options.IsStartButton.HasValue)
            {
                button.Attributes.Add("is-start-button", options.IsStartButton.Value ? "true" : "false");
            }

            var content = options.GetHtmlContent();
            button.InnerHtml.AppendHtml(content);

            return button.RenderToString();
        }
    }
}
