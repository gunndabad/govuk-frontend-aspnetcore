using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class ButtonTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ButtonTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<button class=""govuk-button"" data-module=""govuk-button"">
    Button text
</button>";

            AssertEx.HtmlEqual(expectedHtml, output.RenderToString());
        }

        [Fact]
        public async Task ProcessAsync_IsStartButton_AddsIconToOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ButtonTagHelper()
            {
                IsStartButton = true
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var element = output.RenderToElement();

            Assert.Contains("govuk-button--start", element.ClassList);

            Assert.Collection(
                element.QuerySelectorAll("svg"),
                svg => Assert.Contains("govuk-button__start-icon", svg.ClassList));
        }

        [Fact]
        public async Task ProcessAsync_Disabled_AddsDisabledAttributesToOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ButtonTagHelper()
            {
                Disabled = true
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var element = output.RenderToElement();

            Assert.Contains("govuk-button--disabled", element.ClassList);
            Assert.Equal("disabled", element.Attributes["disabled"].Value);
            Assert.Equal("true", element.Attributes["aria-disabled"].Value);
        }

        [Fact]
        public async Task ProcessAsync_PreventDoubleClickSpecified_AddsAttributesToOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ButtonTagHelper()
            {
                PreventDoubleClick = true
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var element = output.RenderToElement();

            Assert.Equal("true", element.Attributes["data-prevent-double-click"].Value);
        }
    }
}
