using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class SelectTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_GeneratesExpectedOutput()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-select",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-select",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var selectContext = context.GetContextItem<SelectContext>();

                selectContext.SetLabel(
                    isPageHeading: false,
                    attributes: null,
                    content: new HtmlString("The label"));

                selectContext.SetHint(
                    attributes: null,
                    content: new HtmlString("The hint"));

                selectContext.AddItem(new SelectItem()
                {
                    Content = new HtmlString("First")
                });

                selectContext.AddItem(new SelectItem()
                {
                    Content = new HtmlString("Second"),
                    Value = "second"
                });

                selectContext.AddItem(new SelectItem()
                {
                    Content = new HtmlString("Third"),
                    Disabled = true,
                    Value = "third"
                });

                selectContext.AddItem(new SelectItem()
                {
                    Content = new HtmlString("Fourth"),
                    Selected = true,
                    Value = "fourth"
                });

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new SelectTagHelper()
        {
            Id = "my-id",
            DescribedBy = "describedby",
            Name = "my-name",
            LabelClass = "additional-label-class"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-form-group"">
    <label for=""my-id"" class=""govuk-label additional-label-class"">The label</label>
    <div id=""my-id-hint"" class=""govuk-hint"">The hint</div>
    <select aria-describedby=""describedby my-id-hint"" class=""govuk-select"" id=""my-id"" name=""my-name"">
        <option>First</option>
        <option value=""second"">Second</option>
        <option disabled=""disabled"" value=""third"">Third</option>
        <option selected=""selected"" value=""fourth"">Fourth</option>
    </select>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithError_RendersExpectedOutput()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-select",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-select",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var selectContext = context.GetContextItem<SelectContext>();

                selectContext.SetLabel(
                    isPageHeading: false,
                    attributes: null,
                    content: new HtmlString("The label"));

                selectContext.SetHint(
                    attributes: null,
                    content: new HtmlString("The hint"));

                selectContext.SetErrorMessage(
                    visuallyHiddenText: null,
                    attributes: null,
                    content: new HtmlString("The error"));

                selectContext.AddItem(new SelectItem()
                {
                    Content = new HtmlString("First")
                });

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new SelectTagHelper()
        {
            Id = "my-id",
            DescribedBy = "describedby",
            Name = "my-name"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-form-group govuk-form-group--error"">
    <label for=""my-id"" class=""govuk-label"">The label</label>
    <div id=""my-id-hint"" class=""govuk-hint"">The hint</div>
    <p id=""my-id-error"" class=""govuk-error-message"">
        <span class=""govuk-visually-hidden"">Error:</span>
        The error
    </p>
    <select aria-describedby=""describedby my-id-hint my-id-error"" class=""govuk-select govuk-select--error"" id=""my-id"" name=""my-name"">
        <option>First</option>
    </select>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }
}
