using System;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class TabsContextTests
    {
        [Fact]
        public void AddItem_ItemHasNotIdAndParentDoesNotHaveIdPrefix_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TabsContext(haveIdPrefix: false);

            var item = new TabsItem()
            {
                Id = null,
                Label = "Label",
                PanelContent = new HtmlString("Panel")
            };

            // Act
            var ex = Record.Exception(() => context.AddItem(item));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Item must have the 'id' attribute specified.", ex.Message);
        }

        [Fact]
        public void AddItem_ValidItemWithId_AddsToItems()
        {
            // Arrange
            var context = new TabsContext(haveIdPrefix: false);

            var item = new TabsItem()
            {
                Id = "item1",
                Label = "Label",
                PanelContent = new HtmlString("Panel")
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
                    Assert.Equal("Panel", item.PanelContent?.ToHtmlString());
                });
        }

        [Fact]
        public void AddItem_ValidItemWithoutId_AddsToItems()
        {
            // Arrange
            var context = new TabsContext(haveIdPrefix: true);

            var item = new TabsItem()
            {
                Id = null,
                Label = "Label",
                PanelContent = new HtmlString("Panel")
            };

            // Act
            context.AddItem(item);

            // Assert
            Assert.Collection(
                context.Items,
                item =>
                {
                    Assert.Equal("Label", item.Label);
                    Assert.Equal("Panel", item.PanelContent?.ToHtmlString());
                });
        }
    }
}
