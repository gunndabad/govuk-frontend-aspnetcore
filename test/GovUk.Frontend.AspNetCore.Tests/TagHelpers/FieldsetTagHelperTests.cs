using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class FieldsetTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-fieldset",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-fieldset",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var fieldsetContext = context.GetContextItem<FieldsetContext>();

                    fieldsetContext.SetLegend(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("Legend text"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Main content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new FieldsetTagHelper()
            {
                DescribedBy = "describedby",
                Role = "therole"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<fieldset aria-describedby=""describedby"" class=""govuk-fieldset"" role=""therole"">
    <legend class=""govuk-fieldset__legend"">
        Legend text
    </legend>
    Main content
</fieldset>";

            AssertEx.HtmlEqual(expectedHtml, output.RenderToString());
        }

        [Fact]
        public async Task ProcessAsync_LegendHasIsPageHeading_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-fieldset",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-fieldset",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var fieldsetContext = context.GetContextItem<FieldsetContext>();

                    fieldsetContext.SetLegend(
                        isPageHeading: true,
                        attributes: null,
                        content: new HtmlString("Legend text"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Main content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new FieldsetTagHelper()
            {
                DescribedBy = "describedby",
                Role = "therole"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<fieldset aria-describedby=""describedby"" class=""govuk-fieldset"" role=""therole"">
    <legend class=""govuk-fieldset__legend"">
        <h1 class=""govuk-fieldset__heading"">Legend text</h1>
    </legend>
    Main content
</fieldset>";

            AssertEx.HtmlEqual(expectedHtml, output.RenderToString());
        }
    }
}
