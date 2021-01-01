using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class InputTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-input",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-input",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var inputContext = context.GetContextItem<InputContext>();

                    inputContext.SetLabel(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("The label"));

                    inputContext.SetHint(
                        attributes: null,
                        content: new HtmlString("The hint"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new InputTagHelper()
            {
                Id = "my-id",
                DescribedBy = "describedby",
                Name = "my-name",
                Autocomplete = "none",
                InputMode = "numeric",
                Pattern = "[0-9]*",
                Type = "number",
                Value = "42"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div class=""govuk-form-group"">
    <label for=""my-id"" class=""govuk-label"">The label</label>
    <div id=""my-id-hint"" class=""govuk-hint"">The hint</div>
    <input
        aria-describedby=""describedby my-id-hint""
        autocomplete=""none""
        class=""govuk-input""
        id=""my-id""
        inputmode=""numeric""
        name=""my-name""
        pattern=""[0-9]*""
        type=""number""
        value=""42"">
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.RenderToString());
        }

        [Fact]
        public async Task ProcessAsync_WithError_RendersExpectedOutput()
        {
            var context = new TagHelperContext(
                tagName: "govuk-input",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-input",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var inputContext = context.GetContextItem<InputContext>();

                    inputContext.SetLabel(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("The label"));

                    inputContext.SetHint(
                        attributes: null,
                        content: new HtmlString("The hint"));

                    inputContext.SetErrorMessage(
                        visuallyHiddenText: null,
                        attributes: null,
                        content: new HtmlString("The error"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new InputTagHelper()
            {
                Id = "my-id",
                DescribedBy = "describedby",
                Name = "my-name",
                Type = "text"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div class=""govuk-form-group govuk-form-group--error"">
    <label for=""my-id"" class=""govuk-label"">The label</label>
    <div id=""my-id-hint"" class=""govuk-hint"">The hint</div>
    <span id=""my-id-error"" class=""govuk-error-message"">
        <span class=""govuk-visually-hidden"">Error:</span>
        The error
    </span>
    <input
        aria-describedby=""describedby my-id-hint my-id-error""
        class=""govuk-input govuk-input--error""
        id=""my-id""
        name=""my-name""
        type=""text"">
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.RenderToString());
        }

        [Fact]
        public async Task ProcessAsync_NoTypeSpecified_UsesDefaultType()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-input",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-input",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var inputContext = context.GetContextItem<InputContext>();

                    inputContext.SetLabel(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("The label"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new InputTagHelper()
            {
                Id = "my-id",
                Name = "my-name"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var element = output.RenderToElement();
            var input = element.QuerySelector("input");
            Assert.Equal("text", input.Attributes["type"].Value);
        }

        [Fact]
        public async Task ProcessAsync_WithPrefix_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-input",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-input",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var inputContext = context.GetContextItem<InputContext>();

                    inputContext.SetLabel(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("The label"));

                    inputContext.SetPrefix(attributes: null, content: new HtmlString("Prefix"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new InputTagHelper()
            {
                Id = "my-id",
                Name = "my-name"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div class=""govuk-form-group"">
    <label for=""my-id"" class=""govuk-label"">The label</label>
    <div class=""govuk-input__wrapper"">
        <div aria-hidden=""true"" class=""govuk-input__prefix"">Prefix</div>
        <input
            class=""govuk-input""
            id=""my-id""
            name=""my-name""
            type=""text"">
    </div>
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.RenderToString());
        }

        [Fact]
        public async Task ProcessAsync_WithSuffix_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-input",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-input",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var inputContext = context.GetContextItem<InputContext>();

                    inputContext.SetLabel(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("The label"));

                    inputContext.SetSuffix(attributes: null, content: new HtmlString("Suffix"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new InputTagHelper()
            {
                Id = "my-id",
                Name = "my-name"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div class=""govuk-form-group"">
    <label for=""my-id"" class=""govuk-label"">The label</label>
    <div class=""govuk-input__wrapper"">
        <input
            class=""govuk-input""
            id=""my-id""
            name=""my-name""
            type=""text"">
        <div aria-hidden=""true"" class=""govuk-input__suffix"">Suffix</div>
    </div>
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.RenderToString());
        }

        [Fact]
        public async Task ProcessAsync_WithPrefixAndSuffix_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-input",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-input",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var inputContext = context.GetContextItem<InputContext>();

                    inputContext.SetLabel(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("The label"));

                    inputContext.SetPrefix(attributes: null, content: new HtmlString("Prefix"));

                    inputContext.SetSuffix(attributes: null, content: new HtmlString("Suffix"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new InputTagHelper()
            {
                Id = "my-id",
                Name = "my-name"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div class=""govuk-form-group"">
    <label for=""my-id"" class=""govuk-label"">The label</label>
    <div class=""govuk-input__wrapper"">
        <div aria-hidden=""true"" class=""govuk-input__prefix"">Prefix</div>
        <input
            class=""govuk-input""
            id=""my-id""
            name=""my-name""
            type=""text"">
        <div aria-hidden=""true"" class=""govuk-input__suffix"">Suffix</div>
    </div>
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.RenderToString());
        }
    }
}
