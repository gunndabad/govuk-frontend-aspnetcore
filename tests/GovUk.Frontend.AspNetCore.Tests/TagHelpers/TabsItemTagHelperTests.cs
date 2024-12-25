using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class TabsItemTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_WithIdSpecified_AddsItemToContext()
    {
        // Arrange
        var id = "item1";
        var label = "First";
        var panelHtml = "Panel";

        var context = new TagHelperContext(
            tagName: "govuk-tabs-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var tabsContext = new TabsContext(haveIdPrefix: false);
        context.Items.Add(typeof(TabsContext), tabsContext);

        var output = new TagHelperOutput(
            "govuk-tabs-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var panelContent = new DefaultTagHelperContent();
                panelContent.SetContent(panelHtml);
                return Task.FromResult<TagHelperContent>(panelContent);
            });

        var tagHelper = new TabsItemTagHelper()
        {
            Id = id,
            Label = label
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            tabsContext.Items,
            item =>
            {
                Assert.Equal(id, item.Id?.ToHtmlString());
                Assert.Equal(label, item.Label?.ToHtmlString());
                Assert.Equal(panelHtml, item.Panel?.Html?.ToHtmlString());
            });
    }

    [Fact]
    public async Task ProcessAsync_WithoutIdSpecified_AddsItemToContext()
    {
        // Arrange
        var label = "First";
        var panelHtml = "Panel";

        var context = new TagHelperContext(
            tagName: "govuk-tabs-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var tabsContext = new TabsContext(haveIdPrefix: true);
        context.Items.Add(typeof(TabsContext), tabsContext);

        var output = new TagHelperOutput(
            "govuk-tabs-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var panelContent = new DefaultTagHelperContent();
                panelContent.SetContent(panelHtml);
                return Task.FromResult<TagHelperContent>(panelContent);
            });

        var tagHelper = new TabsItemTagHelper()
        {
            Label = label
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            tabsContext.Items,
            item =>
            {
                Assert.Null(item.Id);
                Assert.Equal(label, item.Label?.ToHtmlString());
                Assert.Equal(panelHtml, item.Panel?.Html?.ToHtmlString());
            });
    }

    [Fact]
    public async Task ProcessAsync_LabelNotSpecified_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-tabs-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var tabsContext = new TabsContext(haveIdPrefix: false);
        context.Items.Add(typeof(TabsContext), tabsContext);

        var output = new TagHelperOutput(
            "govuk-tabs-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new TabsItemTagHelper();

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("The 'label' attribute must be specified.", ex.Message);
    }
}
