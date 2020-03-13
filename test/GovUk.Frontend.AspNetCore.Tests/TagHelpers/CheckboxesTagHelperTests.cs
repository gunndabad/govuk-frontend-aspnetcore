using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
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
                    var checkboxesContext = (CheckboxesContext)context.Items[CheckboxesContext.ContextName];

                    checkboxesContext.AddItem(new CheckboxesItem()
                    {
                        Checked = false,
                        Content = new HtmlString("First"),
                        Disabled = false,
                        Id = "first",
                        Value = "first"
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                Name = "testcheckboxes"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
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
                    var checkboxesContext = (CheckboxesContext)context.Items[CheckboxesContext.ContextName];

                    checkboxesContext.AddItem(new CheckboxesItem()
                    {
                        Checked = true,
                        Content = new HtmlString("First"),
                        Disabled = false,
                        Id = "first",
                        Value = "first"
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                DescribedBy = "describedby",
                Name = "testcheckboxes"
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
                tagName: "govuk-checkboxes",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var checkboxesContext = (CheckboxesContext)context.Items[CheckboxesContext.ContextName];

                    checkboxesContext.AddItem(new CheckboxesItem()
                    {
                        Checked = false,
                        Content = new HtmlString("First"),
                        Disabled = true,
                        Id = "first",
                        Value = "first"
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                DescribedBy = "describedby",
                Name = "testcheckboxes"
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
                tagName: "govuk-checkboxes",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetHint(attributes: null, content: new HtmlString("The hint"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                Name = "testcheckboxes"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-form-group\">" +
                "<span class=\"govuk-hint\" id=\"my-id-hint\">The hint</span>" +
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
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetErrorMessage(
                        visuallyHiddenText: null,
                        attributes: null,
                        content: new HtmlString("A error"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                Name = "testcheckboxes"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-form-group--error govuk-form-group\">" +
                "<span class=\"govuk-error-message\" id=\"my-id-error\"><span class=\"govuk-visually-hidden\">Error</span>A error</span>" +
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
                    var checkboxesContext = (CheckboxesContext)context.Items[CheckboxesContext.ContextName];

                    checkboxesContext.AddItem(new CheckboxesItem()
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

            var tagHelper = new CheckboxesTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                Name = "testcheckboxes"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-form-group\">" +
                "<div class=\"govuk-checkboxes\">" +
                "<div class=\"govuk-checkboxes__item\">" +
                "<input aria-describedby=\"first-hint\" class=\"govuk-checkboxes__input\" id=\"first\" name=\"testcheckboxes\" type=\"checkbox\" value=\"first\" />" +
                "<label class=\"govuk-checkboxes__label govuk-label\" for=\"first\">First</label>" +
                "<span class=\"govuk-checkboxes__hint govuk-hint\" id=\"first-hint\">Item hint</span>" +
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
                    var checkboxesContext = (CheckboxesContext)context.Items[CheckboxesContext.ContextName];

                    checkboxesContext.AddItem(new CheckboxesItem()
                    {
                        Checked = false,
                        ConditionalContent = new HtmlString("Conditional"),
                        ConditionalId = "conditional-first",
                        Content = new HtmlString("First"),
                        Disabled = false,
                        Id = "first",
                        Value = "first"
                    });

                    checkboxesContext.SetIsConditional();

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                Name = "testcheckboxes"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-form-group\">" +
                "<div class=\"govuk-checkboxes--conditional govuk-checkboxes\" data-module=\"govuk-checkboxes\">" +
                "<div class=\"govuk-checkboxes__item\">" +
                "<input class=\"govuk-checkboxes__input\" data-aria-controls=\"conditional-first\" id=\"first\" name=\"testcheckboxes\" type=\"checkbox\" value=\"first\" />" +
                "<label class=\"govuk-checkboxes__label govuk-label\" for=\"first\">First</label>" +
                "<div class=\"govuk-checkboxes__conditional--hidden govuk-checkboxes__conditional\" id=\"conditional-first\">Conditional</div>" +
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
                tagName: "govuk-checkboxes",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var checkboxesContext = (CheckboxesContext)context.Items[CheckboxesContext.ContextName];

                    checkboxesContext.AddItem(new CheckboxesItem()
                    {
                        Checked = true,
                        ConditionalContent = new HtmlString("Conditional"),
                        ConditionalId = "conditional-first",
                        Content = new HtmlString("First"),
                        Disabled = false,
                        Id = "first",
                        Value = "first"
                    });

                    checkboxesContext.SetIsConditional();

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                Name = "testcheckboxes"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-form-group\">" +
                "<div class=\"govuk-checkboxes--conditional govuk-checkboxes\" data-module=\"govuk-checkboxes\">" +
                "<div class=\"govuk-checkboxes__item\">" +
                "<input checked=\"checked\" class=\"govuk-checkboxes__input\" data-aria-controls=\"conditional-first\" id=\"first\" name=\"testcheckboxes\" type=\"checkbox\" value=\"first\" />" +
                "<label class=\"govuk-checkboxes__label govuk-label\" for=\"first\">First</label>" +
                "<div class=\"govuk-checkboxes__conditional\" id=\"conditional-first\">Conditional</div>" +
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

            var tagHelper = new CheckboxesTagHelper(Mock.Of<IGovUkHtmlGenerator>())
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
                    var checkboxesContext = (CheckboxesContext)context.Items[CheckboxesContext.ContextName];

                    checkboxesContext.SetFieldset(new CheckboxesFieldset()
                    {
                        DescribedBy = "fieldsetdescribedby",
                        IsPageHeading = false,
                        LegendContent = new HtmlString("Legend")
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesTagHelper(new DefaultGovUkHtmlGenerator())
            {
                DescribedBy = "describedby",
                IdPrefix = "my-id",
                Name = "testcheckboxes"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
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
            var checkboxesContext = new CheckboxesContext(idPrefix: "prefix", resolvedName: "r");

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-fieldset",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { CheckboxesContext.ContextName, checkboxesContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes-fieldset",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var fieldsetContext = (CheckboxesFieldsetContext)context.Items[CheckboxesFieldsetContext.ContextName];
                    fieldsetContext.TrySetLegend(attributes: null, content: new HtmlString("Legend"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesFieldsetTagHelper()
            {
                DescribedBy = "fieldsetdescribedby",
                IsPageHeading = true
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.True(checkboxesContext.Fieldset.IsPageHeading);
            Assert.Equal("Legend", checkboxesContext.Fieldset.LegendContent.AsString());
            Assert.Equal("fieldsetdescribedby", checkboxesContext.Fieldset.DescribedBy);
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
                    { CheckboxesFieldsetContext.ContextName, fieldsetContext }
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
            Assert.Equal("Legend", fieldsetContext.Legend?.content?.AsString());
        }
    }

    public class CheckboxesItemTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_ValueNotSpecifiedThrowsNotSupportedException()
        {
            // Arrange
            var checkboxesContext = new CheckboxesContext(idPrefix: "prefix", resolvedName: "r");

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { CheckboxesContext.ContextName, checkboxesContext }
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

            var tagHelper = new CheckboxesItemTagHelper();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("The 'value' attribute must be specified.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_AddsItemToContext()
        {
            // Arrange
            var checkboxesContext = new CheckboxesContext(idPrefix: "prefix", resolvedName: "mycheckboxes");

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { CheckboxesContext.ContextName, checkboxesContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var itemContext = (CheckboxesItemContext)context.Items[CheckboxesItemContext.ContextName];
                    itemContext.SetHint(attributes: null, content: new HtmlString("Hint"));
                    itemContext.SetConditionalContent(new HtmlString("Conditional"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Label"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper()
            {
                Checked = true,
                Id = "id",
                Value = "V"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Contains(
                checkboxesContext.Items,
                item => item is CheckboxesItem i &&
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
            var checkboxesContext = new CheckboxesContext(idPrefix: "prefix", resolvedName: "mycheckboxes");

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { CheckboxesContext.ContextName, checkboxesContext }
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

            var tagHelper = new CheckboxesItemTagHelper()
            {
                Checked = true,
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
            var checkboxesContext = new CheckboxesContext(idPrefix: "prefix", resolvedName: "mycheckboxes");
            checkboxesContext.AddItem(new CheckboxesItem()
            {
                Content = new HtmlString("First")
            });

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { CheckboxesContext.ContextName, checkboxesContext }
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

            var tagHelper = new CheckboxesItemTagHelper()
            {
                Checked = true,
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
            var checkboxesContext = new CheckboxesContext(idPrefix: "prefix", resolvedName: "mycheckboxes");

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { CheckboxesContext.ContextName, checkboxesContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var itemContext = (CheckboxesItemContext)context.Items[CheckboxesItemContext.ContextName];
                    itemContext.SetConditionalContent(new HtmlString("Conditional"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Label"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper()
            {
                Checked = true,
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
            var checkboxesContext = new CheckboxesContext(idPrefix: "prefix", resolvedName: "mycheckboxes");

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { CheckboxesContext.ContextName, checkboxesContext }
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

            var tagHelper = new CheckboxesItemTagHelper()
            {
                Checked = true,
                Value = "V"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.False(checkboxesContext.IsConditional);
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
                    { CheckboxesItemContext.ContextName, itemContext }
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
            Assert.Equal("Conditional", itemContext.ConditionalContent.AsString());
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
                    { CheckboxesItemContext.ContextName, itemContext }
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
            Assert.Equal("Hint", itemContext.Hint?.content.AsString());
        }
    }
}
