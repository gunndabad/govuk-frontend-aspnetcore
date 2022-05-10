using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class FormErrorSummaryTagHelperTests
    {
        [Theory]
        [InlineData(false, null, false)]
        [InlineData(false, false, false)]
        [InlineData(false, true, true)]
        [InlineData(true, null, true)]
        [InlineData(true, false, false)]
        [InlineData(true, true, true)]
        public async Task ProcessAsync_GeneratesExpectedOutput(
            bool prepentErrorSummaryToFormsOption,
            bool? prependErrorSummary,
            bool expectErrorSummary)
        {
            // Arrange
            var options = Options.Create(new GovUkFrontendAspNetCoreOptions()
            {
                PrependErrorSummaryToForms = prepentErrorSummaryToFormsOption
            });

            var context = new TagHelperContext(
                tagName: "form",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "form",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formErrorContext = (FormErrorContext)context.Items[typeof(FormErrorContext)];
                    formErrorContext.AddError(new HtmlString("Content"), "href");

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new FormErrorSummaryTagHelper(options)
            {
                PrependErrorSummary = prependErrorSummary
            };

            tagHelper.Init(context);

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToElement();
            Assert.Equal(expectErrorSummary ? 1 : 0, html.ChildElementCount);
        }
    }
}
