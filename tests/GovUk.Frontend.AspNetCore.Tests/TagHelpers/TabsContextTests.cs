using System;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class TabsContextTests
{
    [Fact]
    public void AddItem_ItemHasNotIdAndParentDoesNotHaveIdPrefix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TabsContext(haveIdPrefix: false);

        var item = new TabsOptionsItem()
        {
            Id = null,
            Label = new HtmlString("Label"),
            Panel = new()
            {
                Html = new HtmlString("Panel")
            }
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
        var id = "item1";
        var label = "Label";
        var panelHtml = "Panel";

        var context = new TabsContext(haveIdPrefix: false);

        var item = new TabsOptionsItem()
        {
            Id = new HtmlString(id),
            Label = new HtmlString(label),
            Panel = new()
            {
                Html = new HtmlString(panelHtml)
            }
        };

        // Act
        context.AddItem(item);

        // Assert
        Assert.Collection(
            context.Items,
            item =>
            {
                Assert.Equal(id, item.Id?.ToHtmlString());
                Assert.Equal(label, item.Label?.ToHtmlString());
                Assert.Equal(panelHtml, item.Panel?.Html?.ToHtmlString());
            });
    }

    [Fact]
    public void AddItem_ValidItemWithoutId_AddsToItems()
    {
        // Arrange
        var label = "Label";
        var panelHtml = "Panel";

        var context = new TabsContext(haveIdPrefix: true);

        var item = new TabsOptionsItem()
        {
            Id = null,
            Label = new HtmlString(label),
            Panel = new()
            {
                Html = new HtmlString(panelHtml)
            }
        };

        // Act
        context.AddItem(item);

        // Assert
        Assert.Collection(
            context.Items,
            item =>
            {
                Assert.Equal(label, item.Label?.ToHtmlString());
                Assert.Equal(panelHtml, item.Panel?.Html?.ToHtmlString());
            });
    }
}
