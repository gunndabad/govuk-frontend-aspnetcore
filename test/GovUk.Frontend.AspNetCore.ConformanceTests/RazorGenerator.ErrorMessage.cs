using GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class RazorGenerator
    {
        public string GenerateErrorMessage(ErrorMessage options)
        {
            var errorMessage = new TagBuilder("govuk-error-message");

            errorMessage.AddAttributes(options.Attributes);
            errorMessage.AddCssClass(options.Classes);

            if (options.Id != null)
            {
                errorMessage.Attributes.Add("id", options.Id);
            }

            var visuallyHiddenTextType = options.VisuallyHiddenText?.Type;
            if (visuallyHiddenTextType != null)
            {
                if (visuallyHiddenTextType == Newtonsoft.Json.Linq.JTokenType.String)
                {
                    errorMessage.Attributes.Add("visually-hidden-text", options.VisuallyHiddenText.ToString());
                }
                else if (options.VisuallyHiddenText.ToObject<bool?>().GetValueOrDefault() == false)  // falsey
                {
                    errorMessage.Attributes.Add("visually-hidden-text", "");
                }
            }

            var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html);
            errorMessage.InnerHtml.AppendHtml(content);

            return errorMessage.RenderToString();
        }
    }
}
