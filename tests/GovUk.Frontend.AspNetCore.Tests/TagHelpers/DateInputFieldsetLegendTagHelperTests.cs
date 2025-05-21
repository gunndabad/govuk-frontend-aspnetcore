using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class DateInputFieldsetLegendTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddsLegendToContext()
    {
        // Arrange
        var fieldsetContext = new DateInputFieldsetContext(attributes: null, aspFor: null);

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

        var tagHelper = new DateInputFieldsetLegendTagHelper()
        {
            IsPageHeading = true
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal("Legend content", fieldsetContext.Legend?.Content?.ToHtmlString());
        Assert.True(fieldsetContext.Legend?.IsPageHeading);
    }

    [Fact]
    public async Task ProcessAsync_ParentAlreadyHasLegend_ThrowsInvalidOperationException()
    {
        // Arrange
        var fieldsetContext = new DateInputFieldsetContext(attributes: null, aspFor: null);

        fieldsetContext.SetLegend(
            isPageHeading: false,
            attributes: null,
            content: new HtmlString("Existing legend"));

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

        var tagHelper = new DateInputFieldsetLegendTagHelper()
        {
            IsPageHeading = true
        };

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-date-input-fieldset-legend> element is permitted within each <govuk-date-input-fieldset>.", ex.Message);
    }
}
