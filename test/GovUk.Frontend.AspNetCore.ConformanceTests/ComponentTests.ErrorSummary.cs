using System.Linq;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class ComponentTests
    {
        [Theory]
        [ComponentFixtureData("error-summary", typeof(OptionsJson.ErrorSummary))]
        public void ErrorSummary(ComponentTestCaseData<OptionsJson.ErrorSummary> data) =>
            CheckComponentHtmlMatchesExpectedHtml(
                data,
                (generator, options) =>
                {
                    var titleContent = TextOrHtmlHelper.GetHtmlContent(options.TitleText, options.TitleHtml);

                    var descriptionContent = TextOrHtmlHelper.GetHtmlContent(options.DescriptionText, options.DescriptionHtml);

                    var items = options.ErrorList?.Select(i =>
                    {
                        var content = TextOrHtmlHelper.GetHtmlContent(i.Text, i.Html);

                        var attributes = i.Attributes.ToAttributesDictionary();

                        return new ErrorSummaryItem()
                        {
                            Content = content,
                            Href = i.Href,
                            LinkAttributes = attributes
                        };
                    }) ?? Enumerable.Empty<ErrorSummaryItem>();

                    var attributes = options.Attributes.ToAttributesDictionary()
                        .MergeAttribute("class", options.Classes);

                    return generator.GenerateErrorSummary(
                            options.DisableAutoFocus ?? ComponentGenerator.ErrorSummaryDefaultDisableAutoFocus,
                            titleContent,
                            titleAttributes: null,
                            descriptionContent,
                            descriptionAttributes: null,
                            attributes,
                            items)
                        .ToHtmlString();
                });
    }
}
