using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class ComponentTests
    {
        [Theory]
        [ComponentFixtureData("error-message", typeof(OptionsJson.ErrorMessage))]
        public void ErrorMessage(ComponentTestCaseData<OptionsJson.ErrorMessage> data) =>
            CheckComponentHtmlMatchesExpectedHtml(
                data,
                (generator, options) => BuildErrorMessage(generator, options).RenderToString());

        private static IHtmlContent BuildErrorMessage(ComponentGenerator generator, OptionsJson.ErrorMessage options)
        {
            var visuallyHiddenText = options.VisuallyHiddenText switch
            {
                bool flag when flag == false => string.Empty,
                string str => str,
                _ => ComponentGenerator.ErrorMessageDefaultVisuallyHiddenText
            };

            var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html) ?? _emptyContent;

            var attributes = options.Attributes.ToAttributesDictionary()
                .MergeAttribute("class", options.Classes)
                .MergeAttribute("id", options.Id);

            return generator.GenerateErrorMessage(visuallyHiddenText, content, attributes);
        }
    }
}
