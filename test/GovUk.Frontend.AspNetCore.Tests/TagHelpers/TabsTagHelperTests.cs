using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class TabsTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-tabs",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-tabs",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tabsContext = context.GetContextItem<TabsContext>(); ;

                    tabsContext.AddItem(new TabsItem()
                    {
                        Id = "first",
                        Label = "First",
                        PanelContent = new HtmlString("First panel content")
                    });

                    tabsContext.AddItem(new TabsItem()
                    {
                        Id = "second",
                        Label = "Second",
                        PanelContent = new HtmlString("Second panel content")
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TabsTagHelper(new ComponentGenerator())
            {
                Id = "my-tabs",
                Title = "Title"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div class=""govuk-tabs"" data-module=""govuk-tabs"" id=""my-tabs"">
    <h2 class=""govuk-tabs__title"">Title</h2>
    <ul class=""govuk-tabs__list"">
        <li class=""govuk-tabs__list-item--selected govuk-tabs__list-item""><a class=""govuk-tabs__tab"" href=""#first"">First</a></li>
        <li class=""govuk-tabs__list-item""><a class=""govuk-tabs__tab"" href=""#second"">Second</a></li>
    </ul>
    <div class=""govuk-tabs__panel"" id=""first"">First panel content</div>
    <div class=""govuk-tabs__panel--hidden govuk-tabs__panel"" id=""second"">Second panel content</div>
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
        }

        [Fact]
        public async Task ProcessAsync_WithoutTitle_UsesDefaultTitle()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-tabs",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-tabs",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tabsContext = context.GetContextItem<TabsContext>();

                    tabsContext.AddItem(new TabsItem()
                    {
                        Id = "first",
                        Label = "First",
                        PanelContent = new HtmlString("First panel content")
                    });

                    tabsContext.AddItem(new TabsItem()
                    {
                        Id = "second",
                        Label = "Second",
                        PanelContent = new HtmlString("Second panel content")
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TabsTagHelper(new ComponentGenerator())
            {
                Id = "my-tabs"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var element = output.RenderToElement();
            Assert.Equal("Contents", element.QuerySelector("h2").TextContent);
        }
    }
}
