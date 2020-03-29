using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class RadiosTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-radios",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var radiosContext = (RadiosContext)context.Items[RadiosContext.ContextName];

                    radiosContext.AddItem(new RadiosItem()
                    {
                        Checked = false,
                        Content = new HtmlString("First"),
                        Disabled = false,
                        Id = "first",
                        Value = "first"
                    });

                    radiosContext.AddItem(new RadiosItemDivider()
                    {
                        Content = new HtmlString("Divider")
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                Name = "testradios"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-form-group\">" +
                "<div class=\"govuk-radios\">" +
                "<div class=\"govuk-radios__item\">" +
                "<input class=\"govuk-radios__input\" id=\"first\" name=\"testradios\" type=\"radio\" value=\"first\" />" +
                "<label class=\"govuk-radios__label govuk-label\" for=\"first\">First</label>" +
                "</div>" +
                "<div class=\"govuk-radios__divider\">Divider</div>" +
                "</div>" +
                "</div>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_CheckedItemGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-radios",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var radiosContext = (RadiosContext)context.Items[RadiosContext.ContextName];

                    radiosContext.AddItem(new RadiosItem()
                    {
                        Checked = true,
                        Content = new HtmlString("First"),
                        Disabled = false,
                        Id = "first",
                        Value = "first"
                    });

                    radiosContext.AddItem(new RadiosItemDivider()
                    {
                        Content = new HtmlString("Divider")
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                DescribedBy = "describedby",
                Name = "testradios"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            var node = HtmlNode.CreateNode(html);
            var input = node.ChildNodes.FindFirst("input");
            Assert.Equal("checked", input.Attributes["checked"].Value);
        }

        [Fact]
        public async Task ProcessAsync_DisabledItemGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-radios",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var radiosContext = (RadiosContext)context.Items[RadiosContext.ContextName];

                    radiosContext.AddItem(new RadiosItem()
                    {
                        Checked = false,
                        Content = new HtmlString("First"),
                        Disabled = true,
                        Id = "first",
                        Value = "first"
                    });

                    radiosContext.AddItem(new RadiosItemDivider()
                    {
                        Content = new HtmlString("Divider")
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                DescribedBy = "describedby",
                Name = "testradios"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            var node = HtmlNode.CreateNode(html);
            var input = node.ChildNodes.FindFirst("input");
            Assert.Equal("disabled", input.Attributes["disabled"].Value);
        }

        [Fact]
        public async Task ProcessAsync_WithHintGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-radios",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetHint(attributes: null, content: new HtmlString("The hint"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                Name = "testradios"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-form-group\">" +
                "<span class=\"govuk-hint\" id=\"my-id-hint\">The hint</span>" +
                "<div class=\"govuk-radios\">" +
                "</div>" +
                "</div>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_WithErrorMessageGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-radios",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetErrorMessage(
                        visuallyHiddenText: null,
                        attributes: null,
                        new HtmlString("A error"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                Name = "testradios"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-form-group--error govuk-form-group\">" +
                "<span class=\"govuk-error-message\" id=\"my-id-error\"><span class=\"govuk-visually-hidden\">Error</span>A error</span>" +
                "<div class=\"govuk-radios\">" +
                "</div>" +
                "</div>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_WithItemHintGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-radios",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var radiosContext = (RadiosContext)context.Items[RadiosContext.ContextName];

                    radiosContext.AddItem(new RadiosItem()
                    {
                        Checked = false,
                        Content = new HtmlString("First"),
                        Disabled = false,
                        HintContent = new HtmlString("Item hint"),
                        HintId = "first-hint",
                        Id = "first",
                        Value = "first"
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                Name = "testradios"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-form-group\">" +
                "<div class=\"govuk-radios\">" +
                "<div class=\"govuk-radios__item\">" +
                "<input aria-describedby=\"first-hint\" class=\"govuk-radios__input\" id=\"first\" name=\"testradios\" type=\"radio\" value=\"first\" />" +
                "<label class=\"govuk-radios__label govuk-label\" for=\"first\">First</label>" +
                "<span class=\"govuk-radios__hint govuk-hint\" id=\"first-hint\">Item hint</span>" +
                "</div>" +
                "</div>" +
                "</div>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_WithItemConditionalGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-radios",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var radiosContext = (RadiosContext)context.Items[RadiosContext.ContextName];

                    radiosContext.AddItem(new RadiosItem()
                    {
                        Checked = false,
                        ConditionalContent = new HtmlString("Conditional"),
                        ConditionalId = "conditional-first",
                        Content = new HtmlString("First"),
                        Disabled = false,
                        Id = "first",
                        Value = "first"
                    });

                    radiosContext.SetIsConditional();

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                Name = "testradios"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-form-group\">" +
                "<div class=\"govuk-radios--conditional govuk-radios\" data-module=\"govuk-radios\">" +
                "<div class=\"govuk-radios__item\">" +
                "<input class=\"govuk-radios__input\" data-aria-controls=\"conditional-first\" id=\"first\" name=\"testradios\" type=\"radio\" value=\"first\" />" +
                "<label class=\"govuk-radios__label govuk-label\" for=\"first\">First</label>" +
                "<div class=\"govuk-radios__conditional--hidden govuk-radios__conditional\" id=\"conditional-first\">Conditional</div>" +
                "</div>" +
                "</div>" +
                "</div>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_WithCheckedItemConditionalGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-radios",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var radiosContext = (RadiosContext)context.Items[RadiosContext.ContextName];

                    radiosContext.AddItem(new RadiosItem()
                    {
                        Checked = true,
                        ConditionalContent = new HtmlString("Conditional"),
                        ConditionalId = "conditional-first",
                        Content = new HtmlString("First"),
                        Disabled = false,
                        Id = "first",
                        Value = "first"
                    });

                    radiosContext.SetIsConditional();

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                Name = "testradios"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-form-group\">" +
                "<div class=\"govuk-radios--conditional govuk-radios\" data-module=\"govuk-radios\">" +
                "<div class=\"govuk-radios__item\">" +
                "<input checked=\"checked\" class=\"govuk-radios__input\" data-aria-controls=\"conditional-first\" id=\"first\" name=\"testradios\" type=\"radio\" value=\"first\" />" +
                "<label class=\"govuk-radios__label govuk-label\" for=\"first\">First</label>" +
                "<div class=\"govuk-radios__conditional\" id=\"conditional-first\">Conditional</div>" +
                "</div>" +
                "</div>" +
                "</div>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_NoNameOrAspForThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-radios",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosTagHelper(Mock.Of<IGovUkHtmlGenerator>())
            {
                IdPrefix = "prefix"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("At least one of the 'name' and 'asp-for' attributes must be specified.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_WithFieldsetGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-radios",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var radiosContext = (RadiosContext)context.Items[RadiosContext.ContextName];

                    radiosContext.SetFieldset(new RadiosFieldset()
                    {
                        IsPageHeading = false,
                        LegendContent = new HtmlString("Legend")
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosTagHelper(new DefaultGovUkHtmlGenerator())
            {
                DescribedBy = "describedby",
                IdPrefix = "my-id",
                Name = "testradios"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-form-group\">" +
                "<fieldset aria-describedby=\"describedby\" class=\"govuk-fieldset\">" +
                "<legend class=\"govuk-fieldset__legend\">Legend</legend>" +
                "<div class=\"govuk-radios\">" +
                "</div>" +
                "</fieldset>" +
                "</div>",
                html);
        }
    }

    public class RadiosDividerTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsItemToContext()
        {
            // Arrange
            var radiosContext = new RadiosContext(
                idPrefix: "prefix",
                resolvedName: "r",
                viewContext: null,
                @for: null);
            
            var context = new TagHelperContext(
                tagName: "govuk-radios-divider",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { RadiosContext.ContextName, radiosContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios-divider",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Divider"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosDividerTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Contains(radiosContext.Items, item => item is RadiosItemDivider d && d.Content.AsString() == "Divider");
        }
    }

    public class RadiosFieldsetTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_SetsFieldsetOnContext()
        {
            // Arrange
            var radiosContext = new RadiosContext(
                idPrefix: "prefix",
                resolvedName: "r",
                viewContext: null,
                @for: null);

            var context = new TagHelperContext(
                tagName: "govuk-radios-fieldset",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { RadiosContext.ContextName, radiosContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios-fieldset",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var fieldsetContext = (RadiosFieldsetContext)context.Items[RadiosFieldsetContext.ContextName];
                    fieldsetContext.TrySetLegend(attributes: null, content: new HtmlString("Legend"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosFieldsetTagHelper()
            {
                IsPageHeading = true
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.True(radiosContext.Fieldset.IsPageHeading);
            Assert.Equal("Legend", radiosContext.Fieldset.LegendContent.AsString());
        }
    }

    public class RadiosFieldsetLegendTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_SetsLegendOnContext()
        {
            // Arrange
            var fieldsetContext = new RadiosFieldsetContext();

            var context = new TagHelperContext(
                tagName: "govuk-radios-fieldset-legend",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { RadiosFieldsetContext.ContextName, fieldsetContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios-fieldset-legend",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetHtmlContent(new HtmlString("Legend"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosFieldsetLegendTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Legend", fieldsetContext.Legend?.content?.AsString());
        }
    }

    public class RadiosItemTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_ValueNotSpecifiedThrowsNotSupportedException()
        {
            // Arrange
            var radiosContext = new RadiosContext(
                idPrefix: "prefix",
                resolvedName: "r",
                viewContext: null,
                @for: null);

            var context = new TagHelperContext(
                tagName: "govuk-radios-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { RadiosContext.ContextName, radiosContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Legend"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosItemTagHelper(new DefaultGovUkHtmlGenerator());

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("The 'value' attribute must be specified.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_AddsItemToContext()
        {
            // Arrange
            var radiosContext = new RadiosContext(
                idPrefix: "prefix",
                resolvedName: "r",
                viewContext: null,
                @for: null);

            var context = new TagHelperContext(
                tagName: "govuk-radios-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { RadiosContext.ContextName, radiosContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var itemContext = (RadiosItemContext)context.Items[RadiosItemContext.ContextName];
                    itemContext.SetHint(attributes: null, content: new HtmlString("Hint"));
                    itemContext.SetConditional(attributes: null, new HtmlString("Conditional"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Label"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosItemTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Checked = true,
                Id = "id",
                Value = "V"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Contains(
                radiosContext.Items,
                item => item is RadiosItem i &&
                    i.Checked &&
                    !i.Disabled &&
                    i.Content.AsString() == "Label" &&
                    !i.Disabled &&
                    i.Id == "id" &&
                    i.Value == "V" &&
                    i.ConditionalContent.AsString() == "Conditional" &&
                    i.HintContent.AsString() == "Hint");
        }

        [Fact]
        public async Task ProcessAsync_ComputesCorrectIdForFirstItemWhenNotSpecified()
        {
            // Arrange
            var radiosContext = new RadiosContext(
                idPrefix: "prefix",
                resolvedName: "myradios",
                viewContext: null,
                @for: null);

            var context = new TagHelperContext(
                tagName: "govuk-radios-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { RadiosContext.ContextName, radiosContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Label"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosItemTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Checked = true,
                Value = "V"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("prefix", radiosContext.Items.OfType<RadiosItem>().Single().Id);
            Assert.Equal("prefix-item-hint", radiosContext.Items.OfType<RadiosItem>().Single().HintId);
            Assert.Equal("conditional-prefix", radiosContext.Items.OfType<RadiosItem>().Single().ConditionalId);
        }

        [Fact]
        public async Task ProcessAsync_ComputesCorrectIdForSubsequentItemsWhenNotSpecified()
        {
            // Arrange
            var radiosContext = new RadiosContext(
                idPrefix: "prefix",
                resolvedName: "myradios",
                viewContext: null,
                @for: null);
            radiosContext.AddItem(new RadiosItemDivider() { Content = new HtmlString("Divider") });

            var context = new TagHelperContext(
                tagName: "govuk-radios-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { RadiosContext.ContextName, radiosContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Label"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosItemTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Checked = true,
                Value = "V"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("prefix-1", radiosContext.Items.OfType<RadiosItem>().Single().Id);
            Assert.Equal("prefix-1-item-hint", radiosContext.Items.OfType<RadiosItem>().Single().HintId);
            Assert.Equal("conditional-prefix-1", radiosContext.Items.OfType<RadiosItem>().Single().ConditionalId);
        }

        [Fact]
        public async Task ProcessAsync_ConditionalContentSpecifiedSetsIsConditional()
        {
            // Arrange
            var radiosContext = new RadiosContext(
                idPrefix: "prefix",
                resolvedName: "myradios",
                viewContext: null,
                @for: null);

            var context = new TagHelperContext(
                tagName: "govuk-radios-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { RadiosContext.ContextName, radiosContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var itemContext = (RadiosItemContext)context.Items[RadiosItemContext.ContextName];
                    itemContext.SetConditional(attributes: null, new HtmlString("Conditional"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Label"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosItemTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Checked = true,
                Value = "V"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.True(radiosContext.IsConditional);
        }

        [Fact]
        public async Task ProcessAsync_ConditionalContentNotSpecifiedDoesNotSetIsConditional()
        {
            // Arrange
            var radiosContext = new RadiosContext(
                idPrefix: "prefix",
                resolvedName: "myradios",
                viewContext: null,
                @for: null);

            var context = new TagHelperContext(
                tagName: "govuk-radios-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { RadiosContext.ContextName, radiosContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Label"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosItemTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Checked = true,
                Value = "V"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.False(radiosContext.IsConditional);
        }

        [Fact]
        public async Task ProcessAsync_CheckedNullButModelValueEqualsValueSetsCheckedAttribute()
        {
            // Arrange
            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");
            var viewContext = new ViewContext();

            var radiosContext = new RadiosContext(
                idPrefix: "prefix",
                resolvedName: "myradios",
                viewContext: viewContext,
                @for: new ModelExpression("Foo", modelExplorer));

            var context = new TagHelperContext(
                tagName: "govuk-radios-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { RadiosContext.ContextName, radiosContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Label"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<DefaultGovUkHtmlGenerator>()
            {
                CallBase = true
            };

            htmlGenerator
                .Setup(mock => mock.GetModelValue(viewContext, modelExplorer, "Foo"))
                .Returns("bar");

            var tagHelper = new RadiosItemTagHelper(htmlGenerator.Object)
            {
                Value = "bar"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.True(radiosContext.Items.OfType<RadiosItem>().Single().Checked);
        }

        [Fact]
        public async Task ProcessAsync_CheckedNullAndModelValueDoesEqualsValueDoesNotSetCheckedAttribute()
        {
            // Arrange
            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");
            var viewContext = new ViewContext();

            var radiosContext = new RadiosContext(
                idPrefix: "prefix",
                resolvedName: "myradios",
                viewContext: viewContext,
                @for: new ModelExpression("Foo", modelExplorer));

            var context = new TagHelperContext(
                tagName: "govuk-radios-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { RadiosContext.ContextName, radiosContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Label"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<DefaultGovUkHtmlGenerator>()
            {
                CallBase = true
            };

            htmlGenerator
                .Setup(mock => mock.GetModelValue(viewContext, modelExplorer, "Foo"))
                .Returns("bar");

            var tagHelper = new RadiosItemTagHelper(htmlGenerator.Object)
            {
                Value = "baz"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.False(radiosContext.Items.OfType<RadiosItem>().Single().Checked);
        }
    }

    public class RadiosItemConditionalTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddItemsToContext()
        {
            // Arrange
            var itemContext = new RadiosItemContext();

            var context = new TagHelperContext(
                tagName: "govuk-radios-item-conditional",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { RadiosItemContext.ContextName, itemContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios-item-conditional",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Conditional"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosItemConditionalTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Conditional", itemContext.Conditional?.content.AsString());
        }
    }

    public class RadiosItemHintTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddItemsToContext()
        {
            // Arrange
            var itemContext = new RadiosItemContext();

            var context = new TagHelperContext(
                tagName: "govuk-radios-item-hint",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { RadiosItemContext.ContextName, itemContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios-item-hint",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Hint"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosItemHintTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Hint", itemContext.Hint?.content.AsString());
        }
    }
}
