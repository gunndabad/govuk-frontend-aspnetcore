using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class DateInputFieldsetLegendTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddsLegendToContext()
    {
        // Arrange
        var legendHtml = "Legend content";
        var isPageHeading = true;

        var fieldsetContext = new DateInputFieldsetContext(attributes: ImmutableDictionary<string, string?>.Empty, @for: null, describedBy: null);

        var context = new TagHelperContext(
            tagName: "govuk-date-input-fieldset-legend",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(DateInputFieldsetContext), fieldsetContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-date-input-fieldset-legend",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(legendHtml);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputFieldsetLegendTagHelper()
        {
            IsPageHeading = isPageHeading
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(legendHtml, fieldsetContext.Legend?.Html);
        Assert.Equal(isPageHeading, fieldsetContext.Legend?.IsPageHeading);
    }

    [Fact]
    public async Task ProcessAsync_ParentAlreadyHasLegend_ThrowsInvalidOperationException()
    {
        // Arrange
        var fieldsetContext = new DateInputFieldsetContext(attributes: ImmutableDictionary<string, string?>.Empty, @for: null, describedBy: null);

        fieldsetContext.SetLegend(
            isPageHeading: false,
            attributes: ImmutableDictionary<string, string?>.Empty,
            html: "Existing legend");

        var context = new TagHelperContext(
            tagName: "govuk-date-input-fieldset-legend",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(DateInputFieldsetContext), fieldsetContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-date-input-fieldset-legend",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Legend content");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputFieldsetLegendTagHelper();

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-date-input-fieldset-legend> element is permitted within each <govuk-date-input-fieldset>.", ex.Message);
    }
}
