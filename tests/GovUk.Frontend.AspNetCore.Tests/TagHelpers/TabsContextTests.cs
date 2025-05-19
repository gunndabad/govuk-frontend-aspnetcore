using GovUk.Frontend.AspNetCore.Components;
using GovUk.Frontend.AspNetCore.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class TabsContextTests
{
    [Fact]
    public void AddItem_ValidItemWithId_AddsToItems()
    {
        // Arrange
        var context = new TabsContext(haveIdPrefix: false);

        var item = new TabsOptionsItem()
        {
            Id = "item1",
            Label = "Label",
            Panel = new TabsOptionsItemPanel()
            {
                Html = "Panel"
            }
        };

        // Act
        context.AddItem(item);

        // Assert
        Assert.Collection(
            context.Items,
            item =>
            {
                Assert.Equal("item1", item.Id);
                Assert.Equal("Label", item.Label);
                Assert.Equal("Panel", item.Panel?.Html);
            });
    }

    [Fact]
    public void AddItem_ValidItemWithoutId_AddsToItems()
    {
        // Arrange
        var context = new TabsContext(haveIdPrefix: true);

        var item = new TabsOptionsItem()
        {
            Id = null,
            Label = "Label",
            Panel = new TabsOptionsItemPanel()
            {
                Html = "Panel"
            }
        };

        // Act
        context.AddItem(item);

        // Assert
        Assert.Collection(
            context.Items,
            item =>
            {
                Assert.Equal("Label", item.Label);
                Assert.Equal("Panel", item.Panel?.Html);
            });
    }
}
