using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class DetailsSummaryTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_SetsSummaryOnContext()
    {
        // Arrange
        var detailsContext = new DetailsContext();
        var summaryContent = "The summary";

        var context = new TagHelperContext(
            tagName: "govuk-details-summary",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(DetailsContext), detailsContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-details-summary",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(summaryContent);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DetailsSummaryTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(summaryContent, detailsContext.Summary?.Html);
    }

    [Fact]
    public async Task ProcessAsync_ParentAlreadyHasSummary_ThrowsInvalidOperationException()
    {
        // Arrange
        var detailsContext = new DetailsContext();
        detailsContext.SetSummary(ImmutableDictionary<string, string?>.Empty, "Existing summary");

        var context = new TagHelperContext(
            tagName: "govuk-details-summary",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(DetailsContext), detailsContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-details-summary",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("The summary");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DetailsSummaryTagHelper();

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-details-summary> element is permitted within each <govuk-details>.", ex.Message);
    }

    [Fact]
    public async Task ProcessAsync_ParentAlreadyHasText_ThrowsInvalidOperationException()
    {
        // Arrange
        var detailsContext = new DetailsContext();
        detailsContext.SetText(ImmutableDictionary<string, string?>.Empty, "Existing text");

        var context = new TagHelperContext(
            tagName: "govuk-details-summary",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(DetailsContext), detailsContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-details-summary",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("The summary");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DetailsSummaryTagHelper();

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-details-summary> must be specified before <govuk-details-text>.", ex.Message);
    }
}
