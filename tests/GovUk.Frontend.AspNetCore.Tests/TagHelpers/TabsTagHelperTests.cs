using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.Components;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class TabsTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var id = "id";
        var title = "title";
        var firstItemId = "id1";
        var firstItemLabel = "First";
        var firstItemContent = "First content";
        var secondItemId = "id2";
        var secondItemLabel = "Second";
        var secondItemContent = "second content";

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

                tabsContext.AddItem(new TabsOptionsItem()
                {
                    Id = firstItemId,
                    Label = firstItemLabel,
                    Panel = new TabsOptionsItemPanel()
                    {
                        Html = firstItemContent
                    }
                });

                tabsContext.AddItem(new TabsOptionsItem()
                {
                    Id = secondItemId,
                    Label = secondItemLabel,
                    Panel = new TabsOptionsItemPanel()
                    {
                        Html = secondItemContent
                    }
                });

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        TabsOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateTabsAsync(It.IsAny<TabsOptions>())).Callback<TabsOptions>(o => actualOptions = o);

        var tagHelper = new TabsTagHelper(componentGeneratorMock.Object, HtmlEncoder.Default)
        {
            Id = id,
            Title = title
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(id, actualOptions.Id);
        Assert.Equal(title, actualOptions.Title);
        Assert.NotNull(actualOptions.Items);
        Assert.Collection(
            actualOptions.Items,
            item =>
            {
                Assert.Equal(firstItemId, item.Id);
                Assert.Equal(firstItemLabel, item.Label);
                Assert.Equal(firstItemContent, item.Panel?.Html);
            },
            item =>
            {
                Assert.Equal(secondItemId, item.Id);
                Assert.Equal(secondItemLabel, item.Label);
                Assert.Equal(secondItemContent, item.Panel?.Html);
            });
    }
}
