using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class FileUploadTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_GeneratesExpectedOutput()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-file-upload",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-file-upload",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var fileUploadContext = context.GetContextItem<FileUploadContext>();

                fileUploadContext.SetLabel(
                    isPageHeading: false,
                    attributes: null,
                    content: new HtmlString("The label"));

                fileUploadContext.SetHint(
                    attributes: null,
                    content: new HtmlString("The hint"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new FileUploadTagHelper(new ComponentGenerator(), new DefaultModelHelper())
        {
            Id = "my-id",
            DescribedBy = "describedby",
            Name = "my-id",
            LabelClass = "additional-label-class"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-form-group"">
    <label for=""my-id"" class=""govuk-label additional-label-class"">The label</label>
    <div id=""my-id-hint"" class=""govuk-hint"">The hint</div>
    <input aria-describedby=""describedby my-id-hint"" class=""govuk-file-upload"" id=""my-id"" name=""my-id"" type=""file"">
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithError_RendersExpectedOutput()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-file-upload",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-file-upload",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var fileUploadContext = context.GetContextItem<FileUploadContext>();

                fileUploadContext.SetLabel(
                    isPageHeading: false,
                    attributes: null,
                    content: new HtmlString("The label"));

                fileUploadContext.SetErrorMessage(
                    visuallyHiddenText: null,
                    attributes: null,
                    content: new HtmlString("The error"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new FileUploadTagHelper(new ComponentGenerator(), new DefaultModelHelper())
        {
            Id = "my-id",
            DescribedBy = "describedby",
            Name = "my-id"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-form-group govuk-form-group--error"">
    <label for=""my-id"" class=""govuk-label"">The label</label>
    <p id=""my-id-error"" class=""govuk-error-message"">
        <span class=""govuk-visually-hidden"">Error:</span>
        The error
    </p>
    <input aria-describedby=""describedby my-id-error"" class=""govuk-file-upload govuk-file-upload--error"" id=""my-id"" name=""my-id"" type=""file"">
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }
}
