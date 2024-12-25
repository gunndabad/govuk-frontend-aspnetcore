using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class TabsTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var id = "my-id";
        var title = "Title";

        TabsOptionsItem[] items =
        [
            new TabsOptionsItem()
            {
                Id = new HtmlString("first"),
                Label = new HtmlString("First"),
                Panel = new()
                {
                    Html = new HtmlString("First panel content")
                }
            },
            new TabsOptionsItem()
            {
                Id = new HtmlString("second"),
                Label = new HtmlString("Second"),
                Panel = new()
                {
                    Html = new HtmlString("Second panel content")
                }
            }
        ];

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

                foreach (var item in items)
                {
                    tabsContext.AddItem(item);
                }

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        TabsOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateTabs(It.IsAny<TabsOptions>())).Callback<TabsOptions>(o => actualOptions = o);

        var tagHelper = new TabsTagHelper(componentGeneratorMock.Object)
        {
            Id = id,
            Title = title
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(id, actualOptions.Id?.ToHtmlString());
        Assert.Equal(title, actualOptions.Title?.ToHtmlString());
        Assert.NotNull(actualOptions.Items);
        Assert.Collection(
            actualOptions.Items,
            item => Assert.Same(items[0], item),
            item => Assert.Same(items[1], item));
    }
}
