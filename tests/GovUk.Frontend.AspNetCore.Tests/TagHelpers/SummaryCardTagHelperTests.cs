using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class SummaryCardTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_GeneratesExpectedOutput()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-summary-card",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-summary-card",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var summaryCardContext = (SummaryCardContext)context.Items[typeof(SummaryCardContext)];

                summaryCardContext.SetTitle(new HtmlString("My title"), headingLevel: 3, attributes: new AttributeDictionary());

                summaryCardContext.AddAction(new SummaryListAction()
                {
                    Attributes = new Microsoft.AspNetCore.Mvc.ViewFeatures.AttributeDictionary()
                    {
                        { "href", "#" }
                    },
                    Content = new HtmlString("Action 1"),
                    VisuallyHiddenText = "vht"
                });

                summaryCardContext.AddAction(new SummaryListAction()
                {
                    Attributes = new Microsoft.AspNetCore.Mvc.ViewFeatures.AttributeDictionary()
                    {
                        { "href", "#" }
                    },
                    Content = new HtmlString("Action 2")
                });

                summaryCardContext.SetSummaryList(new HtmlString("<div></div>"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new SummaryCardTagHelper(new ComponentGenerator());

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-summary-card"">
  <div class=""govuk-summary-card__title-wrapper"">
    <h3 class=""govuk-summary-card__title"">My title</h3>
    <ul class=""govuk-summary-card__actions"">
      <li class=""govuk-summary-card__action"">
        <a class=""govuk-link"" href=""#"">
          Action 1<span class=""govuk-visually-hidden""> vht (My title)</span>
        </a>
      </li>
      <li class=""govuk-summary-card__action"">
        <a class=""govuk-link"" href=""#"">
          Action 2<span class=""govuk-visually-hidden""> (My title)
        </a>
      </li>
    </ul>
  </div>
  <div class=""govuk-summary-card__content"">
    <div></div>
  </div>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }
}
