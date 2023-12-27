using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class FieldsetLegendTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddsLegendToContext()
    {
        // Arrange
        var legendHtml = "Legend content";
        var legendIsPageHeading = true;
        var fieldsetContext = new FieldsetContext();

        var context = new TagHelperContext(
            tagName: "govuk-fieldset-legend",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(FieldsetContext), fieldsetContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-fieldset-legend",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(legendHtml);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new FieldsetLegendTagHelper()
        {
            IsPageHeading = legendIsPageHeading
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(legendHtml, fieldsetContext.Legend?.Html);
        Assert.Equal(legendIsPageHeading, fieldsetContext.Legend?.IsPageHeading);
    }

    [Fact]
    public async Task ProcessAsync_ParentAlreadyHasLegend_ThrowsInvalidOperationException()
    {
        // Arrange
        var fieldsetContext = new FieldsetContext();

        fieldsetContext.SetLegend(
            isPageHeading: false,
            attributes: ImmutableDictionary<string, string?>.Empty,
            html: "Existing legend");

        var context = new TagHelperContext(
            tagName: "govuk-fieldset-legend",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(FieldsetContext), fieldsetContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-fieldset-legend",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Legend content");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new FieldsetLegendTagHelper()
        {
            IsPageHeading = true
        };

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-fieldset-legend> element is permitted within each <govuk-fieldset>.", ex.Message);
    }
}
