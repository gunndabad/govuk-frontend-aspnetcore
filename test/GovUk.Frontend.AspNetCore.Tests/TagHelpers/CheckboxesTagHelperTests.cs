using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class CheckboxesTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-checkboxes",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var checkboxesContext = (CheckboxesContext)context.Items[typeof(CheckboxesContext)];

                    checkboxesContext.AddItem(new CheckboxesItem()
                    {
                        IsChecked = false,
                        Content = new HtmlString("First"),
                        IsDisabled = false,
                        Id = "first",
                        Value = "first"
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                IdPrefix = "my-id",
                Name = "testcheckboxes"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            Assert.Equal(
                "<div class=\"govuk-form-group\">" +
                "<div class=\"govuk-checkboxes\">" +
                "<div class=\"govuk-checkboxes__item\">" +
                "<input class=\"govuk-checkboxes__input\" id=\"first\" name=\"testcheckboxes\" type=\"checkbox\" value=\"first\" />" +
                "<label class=\"govuk-checkboxes__label govuk-label\" for=\"first\">First</label>" +
                "</div>" +
                "</div>" +
                "</div>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_CheckedItemGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-checkboxes",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var checkboxesContext = (CheckboxesContext)context.Items[typeof(CheckboxesContext)];

                    checkboxesContext.AddItem(new CheckboxesItem()
                    {
                        IsChecked = true,
                        Content = new HtmlString("First"),
                        IsDisabled = false,
                        Id = "first",
                        Value = "first"
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                IdPrefix = "my-id",
                DescribedBy = "describedby",
                Name = "testcheckboxes"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            var node = HtmlNode.CreateNode(html);
            var input = node.ChildNodes.FindFirst("input");
            Assert.Equal("checked", input.Attributes["checked"].Value);
        }

        [Fact]
        public async Task ProcessAsync_DisabledItemGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-checkboxes",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var checkboxesContext = (CheckboxesContext)context.Items[typeof(CheckboxesContext)];

                    checkboxesContext.AddItem(new CheckboxesItem()
                    {
                        IsChecked = false,
                        Content = new HtmlString("First"),
                        IsDisabled = true,
                        Id = "first",
                        Value = "first"
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                IdPrefix = "my-id",
                DescribedBy = "describedby",
                Name = "testcheckboxes"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            var node = HtmlNode.CreateNode(html);
            var input = node.ChildNodes.FindFirst("input");
            Assert.Equal("disabled", input.Attributes["disabled"].Value);
        }

        [Fact]
        public async Task ProcessAsync_WithHintGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-checkboxes",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[typeof(FormGroupBuilder)];
                    formGroupContext.TrySetHint(attributes: null, content: new HtmlString("The hint"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                IdPrefix = "my-id",
                Name = "testcheckboxes"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            Assert.Equal(
                "<div class=\"govuk-form-group\">" +
                "<div class=\"govuk-hint\" id=\"my-id-hint\">The hint</div>" +
                "<div class=\"govuk-checkboxes\">" +
                "</div>" +
                "</div>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_WithErrorMessageGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-checkboxes",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[typeof(FormGroupBuilder)];
                    formGroupContext.TrySetErrorMessage(
                        visuallyHiddenText: null,
                        attributes: null,
                        content: new HtmlString("A error"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                IdPrefix = "my-id",
                Name = "testcheckboxes"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            Assert.Equal(
                "<div class=\"govuk-form-group--error govuk-form-group\">" +
                "<span class=\"govuk-error-message\" id=\"my-id-error\"><span class=\"govuk-visually-hidden\">Error:</span>A error</span>" +
                "<div class=\"govuk-checkboxes\">" +
                "</div>" +
                "</div>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_WithItemHintGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-checkboxes",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var checkboxesContext = (CheckboxesContext)context.Items[typeof(CheckboxesContext)];

                    checkboxesContext.AddItem(new CheckboxesItem()
                    {
                        IsChecked = false,
                        Content = new HtmlString("First"),
                        IsDisabled = false,
                        HintContent = new HtmlString("Item hint"),
                        HintId = "first-hint",
                        Id = "first",
                        Value = "first"
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                IdPrefix = "my-id",
                Name = "testcheckboxes"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            Assert.Equal(
                "<div class=\"govuk-form-group\">" +
                "<div class=\"govuk-checkboxes\">" +
                "<div class=\"govuk-checkboxes__item\">" +
                "<input aria-describedby=\"first-hint\" class=\"govuk-checkboxes__input\" id=\"first\" name=\"testcheckboxes\" type=\"checkbox\" value=\"first\" />" +
                "<label class=\"govuk-checkboxes__label govuk-label\" for=\"first\">First</label>" +
                "<div class=\"govuk-checkboxes__hint govuk-hint\" id=\"first-hint\">Item hint</div>" +
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
                tagName: "govuk-checkboxes",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var checkboxesContext = (CheckboxesContext)context.Items[typeof(CheckboxesContext)];

                    checkboxesContext.AddItem(new CheckboxesItem()
                    {
                        IsChecked = false,
                        ConditionalContent = new HtmlString("Conditional"),
                        ConditionalId = "conditional-first",
                        Content = new HtmlString("First"),
                        IsDisabled = false,
                        Id = "first",
                        Value = "first"
                    });

                    checkboxesContext.SetIsConditional();

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                IdPrefix = "my-id",
                Name = "testcheckboxes"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            Assert.Equal(
                "<div class=\"govuk-form-group\">" +
                "<div class=\"govuk-checkboxes--conditional govuk-checkboxes\" data-module=\"govuk-checkboxes\">" +
                "<div class=\"govuk-checkboxes__item\">" +
                "<input class=\"govuk-checkboxes__input\" data-aria-controls=\"conditional-first\" id=\"first\" name=\"testcheckboxes\" type=\"checkbox\" value=\"first\" />" +
                "<label class=\"govuk-checkboxes__label govuk-label\" for=\"first\">First</label>" +
                "</div>" +
                "<div class=\"govuk-checkboxes__conditional--hidden govuk-checkboxes__conditional\" id=\"conditional-first\">Conditional</div>" +
                "</div>" +
                "</div>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_WithCheckedItemConditionalGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-checkboxes",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var checkboxesContext = (CheckboxesContext)context.Items[typeof(CheckboxesContext)];

                    checkboxesContext.AddItem(new CheckboxesItem()
                    {
                        IsChecked = true,
                        ConditionalContent = new HtmlString("Conditional"),
                        ConditionalId = "conditional-first",
                        Content = new HtmlString("First"),
                        IsDisabled = false,
                        Id = "first",
                        Value = "first"
                    });

                    checkboxesContext.SetIsConditional();

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                IdPrefix = "my-id",
                Name = "testcheckboxes"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            Assert.Equal(
                "<div class=\"govuk-form-group\">" +
                "<div class=\"govuk-checkboxes--conditional govuk-checkboxes\" data-module=\"govuk-checkboxes\">" +
                "<div class=\"govuk-checkboxes__item\">" +
                "<input checked=\"checked\" class=\"govuk-checkboxes__input\" data-aria-controls=\"conditional-first\" id=\"first\" name=\"testcheckboxes\" type=\"checkbox\" value=\"first\" />" +
                "<label class=\"govuk-checkboxes__label govuk-label\" for=\"first\">First</label>" +
                "</div>" +
                "<div class=\"govuk-checkboxes__conditional\" id=\"conditional-first\">Conditional</div>" +
                "</div>" +
                "</div>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_NoNameOrAspForThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-checkboxes",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new ComponentGenerator(), new DefaultModelHelper())
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
                tagName: "govuk-checkboxes",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var checkboxesContext = (CheckboxesContext)context.Items[typeof(CheckboxesContext)];

                    checkboxesContext.SetFieldset(new CheckboxesFieldset()
                    {
                        LegendIsPageHeading = false,
                        LegendContent = new HtmlString("Legend")
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                DescribedBy = "describedby",
                IdPrefix = "my-id",
                Name = "testcheckboxes"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            Assert.Equal(
                "<div class=\"govuk-form-group\">" +
                "<fieldset aria-describedby=\"describedby\" class=\"govuk-fieldset\">" +
                "<legend class=\"govuk-fieldset__legend\">Legend</legend>" +
                "<div class=\"govuk-checkboxes\">" +
                "</div>" +
                "</fieldset>" +
                "</div>",
                html);
        }
    }

    public class CheckboxesFieldsetTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_SetsFieldsetOnContext()
        {
            // Arrange
            var checkboxesContext = new CheckboxesContext(
                idPrefix: "prefix",
                resolvedName: "r",
                aspFor: null,
                viewContext: null);

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-fieldset",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(CheckboxesContext), checkboxesContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes-fieldset",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var fieldsetContext = (CheckboxesFieldsetContext)context.Items[typeof(CheckboxesFieldsetContext)];
                    fieldsetContext.TrySetLegend(
                        isPageHeading: true,
                        attributes: null,
                        content: new HtmlString("Legend"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesFieldsetTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.True(checkboxesContext.Fieldset.LegendIsPageHeading);
            Assert.Equal("Legend", checkboxesContext.Fieldset.LegendContent.RenderToString());
        }
    }

    public class CheckboxesFieldsetLegendTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_SetsLegendOnContext()
        {
            // Arrange
            var fieldsetContext = new CheckboxesFieldsetContext();

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-fieldset-legend",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(CheckboxesFieldsetContext), fieldsetContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes-fieldset-legend",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetHtmlContent(new HtmlString("Legend"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesFieldsetLegendTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Legend", fieldsetContext.Legend?.content?.RenderToString());
        }
    }

    public class CheckboxesItemTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_ValueNotSpecifiedThrowsNotSupportedException()
        {
            // Arrange
            var checkboxesContext = new CheckboxesContext(
                idPrefix: "prefix",
                resolvedName: "r",
                aspFor: null,
                viewContext: null);

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(CheckboxesContext), checkboxesContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Legend"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper(new DefaultModelHelper());

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("The 'value' attribute must be specified.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_AddsItemToContext()
        {
            // Arrange
            var checkboxesContext = new CheckboxesContext(
                idPrefix: "prefix",
                resolvedName: "mycheckboxes",
                aspFor: null,
                viewContext: null);

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(CheckboxesContext), checkboxesContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var itemContext = (CheckboxesItemContext)context.Items[typeof(CheckboxesItemContext)];
                    itemContext.SetHint(attributes: null, content: new HtmlString("Hint"));
                    itemContext.SetConditional(attributes: null, new HtmlString("Conditional"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Label"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper(new DefaultModelHelper())
            {
                IsChecked = true,
                Id = "id",
                Value = "V"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Contains(
                checkboxesContext.Items,
                item => item is CheckboxesItem i &&
                    i.IsChecked &&
                    !i.IsDisabled &&
                    i.Content.RenderToString() == "Label" &&
                    !i.IsDisabled &&
                    i.Id == "id" &&
                    i.Value == "V" &&
                    i.ConditionalContent.RenderToString() == "Conditional" &&
                    i.HintContent.RenderToString() == "Hint");
        }

        [Fact]
        public async Task ProcessAsync_ComputesCorrectIdForFirstItemWhenNotSpecified()
        {
            // Arrange
            var checkboxesContext = new CheckboxesContext(
                idPrefix: "prefix",
                resolvedName: "mycheckboxes",
                aspFor: null,
                viewContext: null);

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(CheckboxesContext), checkboxesContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Label"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper(new DefaultModelHelper())
            {
                IsChecked = true,
                Value = "V"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("prefix", checkboxesContext.Items.Single().Id);
            Assert.Equal("prefix-item-hint", checkboxesContext.Items.Single().HintId);
            Assert.Equal("conditional-prefix", checkboxesContext.Items.Single().ConditionalId);
        }

        [Fact]
        public async Task ProcessAsync_ComputesCorrectIdForSubsequentItemsWhenNotSpecified()
        {
            // Arrange
            var checkboxesContext = new CheckboxesContext(
                idPrefix: "prefix",
                resolvedName: "mycheckboxes",
                aspFor: null,
                viewContext: null);

            checkboxesContext.AddItem(new CheckboxesItem()
            {
                Content = new HtmlString("First")
            });

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(CheckboxesContext), checkboxesContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Label"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper(new DefaultModelHelper())
            {
                IsChecked = true,
                Value = "V"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("prefix-1", checkboxesContext.Items.Last().Id);
            Assert.Equal("prefix-1-item-hint", checkboxesContext.Items.Last().HintId);
            Assert.Equal("conditional-prefix-1", checkboxesContext.Items.Last().ConditionalId);
        }

        [Fact]
        public async Task ProcessAsync_ConditionalContentSpecifiedSetsIsConditional()
        {
            // Arrange
            var checkboxesContext = new CheckboxesContext(
                idPrefix: "prefix",
                resolvedName: "mycheckboxes",
                aspFor: null,
                viewContext: null);

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(CheckboxesContext), checkboxesContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var itemContext = (CheckboxesItemContext)context.Items[typeof(CheckboxesItemContext)];
                    itemContext.SetConditional(attributes: null, new HtmlString("Conditional"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Label"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper(new DefaultModelHelper())
            {
                IsChecked = true,
                Value = "V"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.True(checkboxesContext.IsConditional);
        }

        [Fact]
        public async Task ProcessAsync_ConditionalContentNotSpecifiedDoesNotSetIsConditional()
        {
            // Arrange
            var checkboxesContext = new CheckboxesContext(
                idPrefix: "prefix",
                resolvedName: "mycheckboxes",
                aspFor: null,
                viewContext: null);

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(CheckboxesContext), checkboxesContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Label"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper(new DefaultModelHelper())
            {
                IsChecked = true,
                Value = "V"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.False(checkboxesContext.IsConditional);
        }

        [Fact]
        public async Task ProcessAsync_CheckedNullButModelValueEqualsValueSetsCheckedAttribute()
        {
            // Arrange
            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");
            var viewContext = new ViewContext();

            var radiosContext = new CheckboxesContext(
                idPrefix: "prefix",
                resolvedName: "mycheckboxes",
                viewContext: viewContext,
                aspFor: new ModelExpression("Foo", modelExplorer));

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(CheckboxesContext), radiosContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Label"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var modelHelperMock = new Mock<IModelHelper>();

            modelHelperMock
                .Setup(mock => mock.GetModelValue(viewContext, modelExplorer, "Foo"))
                .Returns("bar");

            var tagHelper = new CheckboxesItemTagHelper(modelHelperMock.Object)
            {
                Value = "bar"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.True(radiosContext.Items.Single().IsChecked);
        }

        [Fact]
        public async Task ProcessAsync_CheckedNullAndModelValueDoesEqualsValueDoesNotSetCheckedAttribute()
        {
            // Arrange
            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");
            var viewContext = new ViewContext();

            var radiosContext = new CheckboxesContext(
                idPrefix: "prefix",
                resolvedName: "mycheckboxes",
                viewContext: viewContext,
                aspFor: new ModelExpression("Foo", modelExplorer));

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(CheckboxesContext), radiosContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Label"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<ComponentGenerator>()
            {
                CallBase = true
            };

            var modelHelperMock = new Mock<IModelHelper>();

            modelHelperMock
                .Setup(mock => mock.GetModelValue(viewContext, modelExplorer, "Foo"))
                .Returns("bar");

            var tagHelper = new CheckboxesItemTagHelper(modelHelperMock.Object)
            {
                Value = "baz"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.False(radiosContext.Items.Single().IsChecked);
        }
    }

    public class CheckboxesItemConditionalTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddItemsToContext()
        {
            // Arrange
            var itemContext = new CheckboxesItemContext();

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item-conditional",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(CheckboxesItemContext), itemContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes-item-conditional",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Conditional"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemConditionalTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Conditional", itemContext.Conditional?.content.RenderToString());
        }
    }

    public class CheckboxesItemHintTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddItemsToContext()
        {
            // Arrange
            var itemContext = new CheckboxesItemContext();

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item-hint",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(CheckboxesItemContext), itemContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes-item-hint",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Hint"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemHintTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Hint", itemContext.Hint?.content.RenderToString());
        }
    }
}
