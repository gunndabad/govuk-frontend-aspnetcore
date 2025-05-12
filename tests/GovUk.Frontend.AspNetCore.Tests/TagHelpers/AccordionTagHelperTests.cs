using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class AccordionTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_GeneratesExpectedOutput()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-accordion",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-accordion",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var accordionContext = (AccordionContext)context.Items[typeof(AccordionContext)];

                accordionContext.AddItem(new AccordionItem()
                {
                    Content = new HtmlString("First content"),
                    Expanded = false,
                    HeadingContent = new HtmlString("First heading"),
                    SummaryContent = new HtmlString("First summary")
                });

                accordionContext.AddItem(new AccordionItem()
                {
                    Content = new HtmlString("First content"),
                    Expanded = true,
                    HeadingContent = new HtmlString("Second heading")
                });

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new AccordionTagHelper()
        {
            Id = "testaccordion",
            HeadingLevel = 1,
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-accordion"" data-module=""govuk-accordion"" id=""testaccordion"">
    <div class=""govuk-accordion__section"">
        <div class=""govuk-accordion__section-header"">
            <h1 class=""govuk-accordion__section-heading"">
                <span class=""govuk-accordion__section-button"" id=""testaccordion-heading-1"">First heading</span>
            </h1>
            <div class=""govuk-body govuk-accordion__section-summary"" id=""testaccordion-summary-1"">First summary</div>
        </div>
        <div class=""govuk-accordion__section-content"" id=""testaccordion-content-1"">
            First content
        </div>
    </div>
    <div class=""govuk-accordion__section--expanded govuk-accordion__section"">
        <div class=""govuk-accordion__section-header"">
            <h1 class=""govuk-accordion__section-heading"">
                <span class=""govuk-accordion__section-button"" id=""testaccordion-heading-2"">Second heading</span>
            </h1>
        </div>
        <div class=""govuk-accordion__section-content"" id=""testaccordion-content-2"">
            First content
        </div>
    </div>
</div>";

        AssertEx.HtmlEqual(@expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithSectionTranslationAttributes_GeneratesExpectedOutput()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-accordion",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-accordion",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var accordionContext = (AccordionContext)context.Items[typeof(AccordionContext)];

                accordionContext.AddItem(new AccordionItem()
                {
                    Content = new HtmlString("First content"),
                    Expanded = false,
                    HeadingContent = new HtmlString("First heading"),
                    SummaryContent = new HtmlString("First summary")
                });

                accordionContext.AddItem(new AccordionItem()
                {
                    Content = new HtmlString("First content"),
                    Expanded = true,
                    HeadingContent = new HtmlString("Second heading")
                });

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new AccordionTagHelper()
        {
            Id = "testaccordion",
            HideAllSectionsText = "Collapse all sections",
            HideSectionText = "Collapse",
            HideSectionAriaLabelText = "Collapse this section",
            ShowAllSectionsText = "Expand all sections",
            ShowSectionText = "Expand",
            ShowSectionAriaLabelText = "Expand this section"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-accordion"" data-module=""govuk-accordion"" id=""testaccordion"" data-i18n.hide-all-sections=""Expand all sections"" data-i18n.hide-section=""Expand"" data-i18n.hide-section-aria-label=""Expand this section"" data-i18n.show-all-sections=""Collapse all sections"" data-i18n.show-section=""Collapse"" data-i18n.show-section-aria-label=""Collapse this section"">
    <div class=""govuk-accordion__section"">
        <div class=""govuk-accordion__section-header"">
            <h2 class=""govuk-accordion__section-heading"">
                <span class=""govuk-accordion__section-button"" id=""testaccordion-heading-1"">First heading</span>
            </h2>
            <div class=""govuk-body govuk-accordion__section-summary"" id=""testaccordion-summary-1"">First summary</div>
        </div>
        <div class=""govuk-accordion__section-content"" id=""testaccordion-content-1"">
            First content
        </div>
    </div>
    <div class=""govuk-accordion__section--expanded govuk-accordion__section"">
        <div class=""govuk-accordion__section-header"">
            <h2 class=""govuk-accordion__section-heading"">
                <span class=""govuk-accordion__section-button"" id=""testaccordion-heading-2"">Second heading</span>
            </h2>
        </div>
        <div class=""govuk-accordion__section-content"" id=""testaccordion-content-2"">
            First content
        </div>
    </div>
</div>";

        AssertEx.HtmlEqual(@expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_NoId_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-accordion",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-accordion",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var accordionContext = (AccordionContext)context.Items[typeof(AccordionContext)];

                accordionContext.AddItem(new AccordionItem()
                {
                    Content = new HtmlString("First content"),
                    Expanded = false,
                    HeadingContent = new HtmlString("First heading"),
                    SummaryContent = new HtmlString("First summary")
                });

                accordionContext.AddItem(new AccordionItem()
                {
                    Content = new HtmlString("First content"),
                    Expanded = true,
                    HeadingContent = new HtmlString("Second heading")
                });

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new AccordionTagHelper(new ComponentGenerator());

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("The 'id' attribute must be specified.", ex.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(7)]
    public void SetHeadingLevel_InvalidLevel_ThrowsArgumentException(int level)
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-accordion",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-accordion",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var accordionContext = (AccordionContext)context.Items[typeof(AccordionContext)];

                accordionContext.AddItem(new AccordionItem()
                {
                    Content = new HtmlString("First content"),
                    Expanded = false,
                    HeadingContent = new HtmlString("First heading"),
                    SummaryContent = new HtmlString("First summary")
                });

                accordionContext.AddItem(new AccordionItem()
                {
                    Content = new HtmlString("First content"),
                    Expanded = true,
                    HeadingContent = new HtmlString("Second heading")
                });

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        // Act
        var ex = Record.Exception(() => new AccordionTagHelper(new ComponentGenerator())
        {
            Id = "testaccordion",
            HeadingLevel = level
        });

        // Assert
        var argumentEx = Assert.IsType<ArgumentOutOfRangeException>(ex);
        Assert.Equal("value", argumentEx.ParamName);
        Assert.StartsWith("HeadingLevel must be between 1 and 6.", argumentEx.Message);
    }
}
