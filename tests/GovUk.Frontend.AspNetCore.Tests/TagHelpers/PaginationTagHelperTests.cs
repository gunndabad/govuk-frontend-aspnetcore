using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class PaginationTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_WithPreviousAndNextOnly_GeneratesExpectedOutput()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-pagination",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-pagination",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var paginationContext = context.GetContextItem<PaginationContext>();

                paginationContext.SetPrevious(new()
                {
                    Href = "/previous",
                    LabelText = "1 of 3",
                    Text = new HtmlString("Previous page")
                });

                paginationContext.SetNext(new()
                {
                    Href = "/next",
                    LabelText = "3 of 3",
                    Text = new HtmlString("Next page")
                });

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            })
        {

        };
        var tagHelper = new PaginationTagHelper(new ComponentGenerator());

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<nav aria-label=""Pagination"" class=""govuk-pagination--block govuk-pagination"">
    <div class=""govuk-pagination__prev"">
        <a class=""govuk-link govuk-pagination__link"" href=""/previous"" rel=""prev"">
            <svg aria-hidden=""true"" class=""govuk-pagination__icon govuk-pagination__icon--prev"" focusable=""false"" height=""13"" viewBox=""0 0 15 13"" width=""15"" xmlns=""http://www.w3.org/2000/svg"">
                <path d=""m6.5938-0.0078125-6.7266 6.7266 6.7441 6.4062 1.377-1.449-4.1856-3.9768h12.896v-2h-12.984l4.2931-4.293-1.414-1.414z""></path>
            </svg>
            <span class=""govuk-pagination__link-title"">Previous page</span>
            <span class=""govuk-visually-hidden"">:</span>
            <span class=""govuk-pagination__link-label"">1 of 3</span>
        </a>
    </div>
    <div class=""govuk-pagination__next"">
        <a class=""govuk-link govuk-pagination__link"" href=""/next"" rel=""next"">
            <svg aria-hidden=""true"" class=""govuk-pagination__icon govuk-pagination__icon--next"" focusable=""false"" height=""13"" viewBox=""0 0 15 13"" width=""15"" xmlns=""http://www.w3.org/2000/svg"">
                <path d=""m8.107-0.0078125-1.4136 1.414 4.2926 4.293h-12.986v2h12.896l-4.1855 3.9766 1.377 1.4492 6.7441-6.4062-6.7246-6.7266z""></path>
            </svg>
            <span class=""govuk-pagination__link-title"">Next page</span>
            <span class=""govuk-visually-hidden"">:</span>
            <span class=""govuk-pagination__link-label"">3 of 3</span>
        </a>
    </div>
</nav>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithItems_GeneratesExpectedOutput()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-pagination",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-pagination",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var paginationContext = context.GetContextItem<PaginationContext>();

                paginationContext.AddItem(new PaginationItem()
                {
                    Href = "/page1",
                    Number = new HtmlString("1"),
                    VisuallyHiddenText = "1st page"
                });

                paginationContext.AddItem(new PaginationItem()
                {
                    Href = "/page2",
                    Number = new HtmlString("2"),
                    IsCurrent = true
                });

                paginationContext.AddItem(new PaginationItemEllipsis());

                paginationContext.AddItem(new PaginationItem()
                {
                    Href = "/page5",
                    Number = new HtmlString("5"),
                    VisuallyHiddenText = "5th page"
                });

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new PaginationTagHelper(new ComponentGenerator())
        {
            LandmarkLabel = "search"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var actual = output.ToHtmlString();
        var expectedHtml = @"
<nav aria-label=""search"" class=""govuk-pagination"">
    <ul class=""govuk-pagination__list"">
        <li class=""govuk-pagination__item"">
            <a aria-label=""1st page"" class=""govuk-link govuk-pagination__link"" href=""/page1"">1</a>
        </li>
        <li class=""govuk-pagination__item govuk-pagination__item--current"">
            <a aria-current=""page"" aria-label=""Page 2"" class=""govuk-link govuk-pagination__link"" href=""/page2"">2</a>
        </li>
        <li class=""govuk-pagination__item govuk-pagination__item--ellipses"">&ctdot;</li>
        <li class=""govuk-pagination__item"">
            <a aria-label=""5th page"" class=""govuk-link govuk-pagination__link"" href=""/page5"">5</a>
        </li>
    </ul>
</nav>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }
}
