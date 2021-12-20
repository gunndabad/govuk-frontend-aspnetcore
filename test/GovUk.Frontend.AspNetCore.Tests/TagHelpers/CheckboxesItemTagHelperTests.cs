using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class CheckboxesItemTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsItemToContext()
        {
            // Arrange
            var checkboxesContext = new CheckboxesContext(name: "test", aspFor: null);

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
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper()
            {
                Checked = true,
                Disabled = true,
                Id = "id",
                Name = "name",
                Value = "value"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Collection(
                checkboxesContext.Items,
                item =>
                {
                    var checkboxesItem = Assert.IsType<CheckboxesItem>(item);
                    Assert.True(checkboxesItem.Checked);
                    Assert.True(checkboxesItem.Disabled);
                    Assert.Equal("id", checkboxesItem.Id);
                    Assert.Equal("name", checkboxesItem.Name);
                    Assert.Equal("value", checkboxesItem.Value);
                });
        }

        [Fact]
        public async Task ProcessAsync_NoValue_ThrowsInvalidOperationException()
        {
            // Arrange
            var checkboxesContext = new CheckboxesContext(name: "test", aspFor: null);

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
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper();

            // Act
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("The 'value' attribute must be specified.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_NoName_ThrowsInvalidOperationException()
        {
            // Arrange
            var checkboxesContext = new CheckboxesContext(name: null, aspFor: null);

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
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper()
            {
                Value = "value"
            };

            // Act
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("The 'name' attribute must be specified on each item when not specified on the parent <govuk-checkboxes>.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_NoNameButParentHasName_DoesNotThrowInvalidOperationException()
        {
            // Arrange
            var checkboxesContext = new CheckboxesContext(name: "parent", aspFor: null);

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
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper()
            {
                Value = "value"
            };

            // Act
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.Null(ex);
        }

        [Theory]
        [InlineData("bar", true)]
        [InlineData("baz", false)]
        public async Task ProcessAsync_WithSimpleModelExpression_DeducesCheckedFromModelExpression(string modelValue, bool expectedChecked)
        {
            // Arrange
            var model = new Model()
            {
                Foo = modelValue
            };

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), model)
                .GetExplorerForProperty(nameof(Model.Foo));
            var viewContext = new ViewContext();
            var modelExpression = nameof(Model.Foo);

            var modelHelper = new Mock<IModelHelper>();

            var checkboxesContext = new CheckboxesContext(name: "test", aspFor: new ModelExpression(modelExpression, modelExplorer));

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
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper(modelHelper.Object)
            {
                Checked = null,
                Value = "bar",
                ViewContext = viewContext
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Collection(
                checkboxesContext.Items,
                item =>
                {
                    var checkboxesItem = Assert.IsType<CheckboxesItem>(item);
                    Assert.Equal(expectedChecked, checkboxesItem.Checked);
                });
        }

        [Theory]
        [InlineData(new[] { 2, 3 }, "3", true)]
        [InlineData(new[] { 2, 3 }, "4", false)]
        public async Task ProcessAsync_WithCollectionModelExpression_DeducesCheckedFromModelExpression(
            int[] modelValues,
            string itemValue,
            bool expectedChecked)
        {
            // Arrange
            var model = new ModelWithCollectionProperty()
            {
                CollectionProperty = modelValues
            };

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(ModelWithCollectionProperty), model)
                .GetExplorerForProperty(nameof(ModelWithCollectionProperty.CollectionProperty));
            var viewContext = new ViewContext();
            var modelExpression = nameof(ModelWithCollectionProperty.CollectionProperty);

            var modelHelper = new Mock<IModelHelper>();

            var checkboxesContext = new CheckboxesContext(name: "test", aspFor: new ModelExpression(modelExpression, modelExplorer));

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
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper(modelHelper.Object)
            {
                ViewContext = viewContext,
                Value = itemValue
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Collection(
                checkboxesContext.Items,
                item =>
                {
                    var checkboxesItem = Assert.IsType<CheckboxesItem>(item);
                    Assert.Equal(expectedChecked, checkboxesItem.Checked);
                });
        }

        [Fact]
        public async Task ProcessAsync_WithHint_SetsHintOnContext()
        {
            // Arrange
            var checkboxesContext = new CheckboxesContext(name: "test", aspFor: null);

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
                    var itemContext = context.GetContextItem<CheckboxesItemContext>();
                    itemContext.SetHint(attributes: null, content: new HtmlString("Hint"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper()
            {
                Name = "name",
                Value = "value"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Collection(
                checkboxesContext.Items,
                item =>
                {
                    var checkboxesItem = Assert.IsType<CheckboxesItem>(item);
                    Assert.Equal("Hint", checkboxesItem.Hint.Content.RenderToString());
                });
        }

        [Fact]
        public async Task ProcessAsync_WithoutHint_DoesNotSetHintOnContext()
        {
            // Arrange
            var checkboxesContext = new CheckboxesContext(name: "test", aspFor: null);

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
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper()
            {
                Name = "name",
                Value = "value"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Collection(
                checkboxesContext.Items,
                item =>
                {
                    var checkboxesItem = Assert.IsType<CheckboxesItem>(item);
                    Assert.Null(checkboxesItem.Hint);
                });
        }

        [Fact]
        public async Task ProcessAsync_WithConditional_SetsConditionalOnContext()
        {
            // Arrange
            var checkboxesContext = new CheckboxesContext(name: "test", aspFor: null);

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
                    var itemContext = context.GetContextItem<CheckboxesItemContext>();
                    itemContext.SetConditional(attributes: null, content: new HtmlString("Conditional"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper()
            {
                Name = "name",
                Value = "value"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Collection(
                checkboxesContext.Items,
                item =>
                {
                    var checkboxesItem = Assert.IsType<CheckboxesItem>(item);
                    Assert.Equal("Conditional", checkboxesItem.Conditional.Content.RenderToString());
                });
        }

        [Fact]
        public async Task ProcessAsync_WithoutConditional_DoesNotSetConditionalOnContext()
        {
            // Arrange
            var checkboxesContext = new CheckboxesContext(name: "test", aspFor: null);

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
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemTagHelper()
            {
                Name = "name",
                Value = "value"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Collection(
                checkboxesContext.Items,
                item =>
                {
                    var checkboxesItem = Assert.IsType<CheckboxesItem>(item);
                    Assert.Null(checkboxesItem.Conditional);
                });
        }

        private class ModelWithBooleanProperty
        {
            public bool BooleanProperty { get; set; }
        }

        private class ModelWithCollectionProperty
        {
            public IEnumerable<int> CollectionProperty { get; set; }
        }
    }
}
