using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class RadiosFieldsetTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsFieldsetToContext()
        {
            // Arrange
            var radiosContext = new RadiosContext(name: null, aspFor: null);

            var context = new TagHelperContext(
                tagName: "govuk-radios-fieldset",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(RadiosContext), radiosContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios-fieldset",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var fieldsetContext = context.GetContextItem<RadiosFieldsetContext>();
                    fieldsetContext.SetLegend(isPageHeading: true, attributes: null, content: new HtmlString("Legend"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosFieldsetTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.True(radiosContext.Fieldset?.Legend?.IsPageHeading);
            Assert.Equal("Legend", radiosContext.Fieldset?.Legend?.Content?.ToHtmlString());
        }

        [Fact]
        public async Task ProcessAsync_ParentAlreadyHasFieldset_ThrowsInvalidOperationException()
        {
            // Arrange
            var radiosContext = new RadiosContext(name: null, aspFor: null);

            radiosContext.OpenFieldset();
            var radiosFieldsetContext = new RadiosFieldsetContext(attributes: null, aspFor: null);
            radiosFieldsetContext.SetLegend(isPageHeading: false, attributes: null, content: new HtmlString("Existing legend"));
            radiosContext.CloseFieldset(radiosFieldsetContext);

            var context = new TagHelperContext(
                tagName: "govuk-radios-fieldset",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(RadiosContext), radiosContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios-fieldset",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var fieldsetContext = context.GetContextItem<RadiosFieldsetContext>();
                    fieldsetContext.SetLegend(isPageHeading: true, attributes: null, content: new HtmlString("Legend"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosFieldsetTagHelper();

            // Act
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Only one <govuk-radios-fieldset> element is permitted within each <govuk-radios>.", ex.Message);
        }
    }
}
