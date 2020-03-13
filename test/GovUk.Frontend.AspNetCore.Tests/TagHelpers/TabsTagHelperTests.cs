using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using HtmlAgilityPack;
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
                    var tabsContext = (TabsContext)context.Items[TabsContext.ContextName];

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

            var tagHelper = new TabsTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "my-tabs",
                Title = "Title"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-tabs\" data-module=\"govuk-tabs\" id=\"my-tabs\">" +
                "<h2 class=\"govuk-tabs__title\">Title</h2>" +
                "<ul class=\"govuk-tabs__list\">" +
                "<li class=\"govuk-tabs__list-item--selected govuk-tabs__list-item\"><a class=\"govuk-tabs__tab\" href=\"#first\">First</a></li>" +
                "<li class=\"govuk-tabs__list-item\"><a class=\"govuk-tabs__tab\" href=\"#second\">Second</a></li>" +
                "</ul>" +
                "<section class=\"govuk-tabs__panel\" id=\"first\">First panel content</section>" +
                "<section class=\"govuk-tabs__panel--hidden govuk-tabs__panel\" id=\"second\">Second panel content</section>" +
                "</div>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_NoTitleUsesDefaultTitle()
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
                    var tabsContext = (TabsContext)context.Items[TabsContext.ContextName];

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

            var tagHelper = new TabsTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "my-tabs"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            var node = HtmlNode.CreateNode(html);
            Assert.Equal("Contents", node.SelectSingleNode("h2").InnerText);
        }
    }

    public class TabsItemTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_NoIdSpecifiedUsesIdDerivedFromPrefixAndIndex()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-tabs-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var tabsContext = new TabsContext(idPrefix: "myprefix");

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

            context.Items.Add(TabsContext.ContextName, tabsContext);

            var output = new TagHelperOutput(
                "govuk-tabs-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TabsItemTagHelper()
            {
                Label = "Third"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("myprefix-2", tabsContext.Items.Last().Id);
        }

        [Fact]
        public async Task ProcessAsync_IdNotSpecifiedAndNoIdPrefixThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-tabs-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var tabsContext = new TabsContext(idPrefix: null);
            context.Items.Add(TabsContext.ContextName, tabsContext);

            var output = new TagHelperOutput(
                "govuk-tabs-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TabsItemTagHelper()
            {
                Label = "Third"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Item must have the 'id' attribute specified when its parent doesn't specify the 'id-prefix' attribute.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_LabelNotSpecifiedThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-tabs-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var tabsContext = new TabsContext(idPrefix: null);
            context.Items.Add(TabsContext.ContextName, tabsContext);

            var output = new TagHelperOutput(
                "govuk-tabs-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TabsItemTagHelper()
            {
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("The 'label' attribute must be specified.", ex.Message);
        }
    }
}
