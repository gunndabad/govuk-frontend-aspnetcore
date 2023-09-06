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

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class RadiosItemTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddsItemToContext()
    {
        // Arrange
        var radiosContext = new RadiosContext(name: "test", aspFor: null);

        var context = new TagHelperContext(
            tagName: "govuk-radios-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(RadiosContext), radiosContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-radios-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new RadiosItemTagHelper()
        {
            Checked = true,
            Disabled = true,
            Id = "id",
            Value = "value"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            radiosContext.Items,
            item =>
            {
                var radiosItem = Assert.IsType<RadiosItem>(item);
                Assert.True(radiosItem.Checked);
                Assert.True(radiosItem.Disabled);
                Assert.Equal("id", radiosItem.Id);
                Assert.Equal("value", radiosItem.Value);
            });
    }

    [Fact]
    public async Task ProcessAsync_NoValue_ThrowsInvalidOperationException()
    {
        // Arrange
        var radiosContext = new RadiosContext(name: "test", aspFor: null);

        var context = new TagHelperContext(
            tagName: "govuk-radios-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(RadiosContext), radiosContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-radios-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new RadiosItemTagHelper();

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("The 'value' attribute must be specified.", ex.Message);
    }

    [Fact]
    public async Task ProcessAsync_NoNameButParentHasName_DoesNotThrowInvalidOperationException()
    {
        // Arrange
        var radiosContext = new RadiosContext(name: "parent", aspFor: null);

        var context = new TagHelperContext(
            tagName: "govuk-radios-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(RadiosContext), radiosContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-radios-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new RadiosItemTagHelper()
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
    public async Task ProcessAsync_WithModelExpression_DeducesCheckedFromModelExpression(string modelValue, bool expectedChecked)
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

        var radiosContext = new RadiosContext(name: "test", aspFor: new ModelExpression(modelExpression, modelExplorer));

        var context = new TagHelperContext(
            tagName: "govuk-radios-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(RadiosContext), radiosContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-radios-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new RadiosItemTagHelper()
        {
            Checked = null,
            Value = "bar"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            radiosContext.Items,
            item =>
            {
                var radiosItem = Assert.IsType<RadiosItem>(item);
                Assert.Equal(expectedChecked, radiosItem.Checked);
            });
    }

    [Fact]
    public async Task ProcessAsync_WithNullCollectionModelExpression_ExecutesSuccessfully()
    {
        // Arrange
        var model = new ModelWithCollectionProperty()
        {
            CollectionProperty = null
        };

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(ModelWithCollectionProperty), model)
            .GetExplorerForProperty(nameof(ModelWithCollectionProperty.CollectionProperty));
        var viewContext = new ViewContext();
        var modelExpression = nameof(ModelWithCollectionProperty.CollectionProperty);

        var radiosContext = new RadiosContext(name: "test", aspFor: new ModelExpression(modelExpression, modelExplorer));

        var context = new TagHelperContext(
            tagName: "govuk-radios-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(RadiosContext), radiosContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-radios-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new RadiosItemTagHelper()
        {
            Value = "2"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            radiosContext.Items,
            item =>
            {
                var radiosItem = Assert.IsType<RadiosItem>(item);
                Assert.False(radiosItem.Checked);
            });
    }

    [Fact]
    public async Task ProcessAsync_WithHint_SetsHintOnContext()
    {
        // Arrange
        var radiosContext = new RadiosContext(name: "test", aspFor: null);

        var context = new TagHelperContext(
            tagName: "govuk-radios-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(RadiosContext), radiosContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-radios-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var itemContext = context.GetContextItem<RadiosItemContext>();
                itemContext.SetHint(new AttributeDictionary(), content: new HtmlString("Hint"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new RadiosItemTagHelper()
        {
            Value = "value"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            radiosContext.Items,
            item =>
            {
                var radiosItem = Assert.IsType<RadiosItem>(item);
                Assert.Equal("Hint", radiosItem.Hint?.Content?.ToHtmlString());
            });
    }

    [Fact]
    public async Task ProcessAsync_WithoutHint_DoesNotSetHintOnContext()
    {
        // Arrange
        var radiosContext = new RadiosContext(name: "test", aspFor: null);

        var context = new TagHelperContext(
            tagName: "govuk-radios-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(RadiosContext), radiosContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-radios-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new RadiosItemTagHelper()
        {
            Value = "value"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            radiosContext.Items,
            item =>
            {
                var radiosItem = Assert.IsType<RadiosItem>(item);
                Assert.Null(radiosItem.Hint);
            });
    }

    [Fact]
    public async Task ProcessAsync_WithConditional_SetsConditionalOnContext()
    {
        // Arrange
        var radiosContext = new RadiosContext(name: "test", aspFor: null);

        var context = new TagHelperContext(
            tagName: "govuk-radios-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(RadiosContext), radiosContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-radios-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var itemContext = context.GetContextItem<RadiosItemContext>();
                itemContext.SetConditional(new AttributeDictionary(), content: new HtmlString("Conditional"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new RadiosItemTagHelper()
        {
            Value = "value"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            radiosContext.Items,
            item =>
            {
                var radiosItem = Assert.IsType<RadiosItem>(item);
                Assert.Equal("Conditional", radiosItem.Conditional?.Content?.ToHtmlString());
            });
    }

    [Fact]
    public async Task ProcessAsync_WithoutConditional_DoesNotSetConditionalOnContext()
    {
        // Arrange
        var radiosContext = new RadiosContext(name: "test", aspFor: null);

        var context = new TagHelperContext(
            tagName: "govuk-radios-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(RadiosContext), radiosContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-radios-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new RadiosItemTagHelper()
        {
            Value = "value"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            radiosContext.Items,
            item =>
            {
                var radiosItem = Assert.IsType<RadiosItem>(item);
                Assert.Null(radiosItem.Conditional);
            });
    }

    private class Model
    {
        public string? Foo { get; set; }
    }

    private class ModelWithBooleanProperty
    {
        public bool BooleanProperty { get; set; }
    }

    private class ModelWithCollectionProperty
    {
        public IEnumerable<int>? CollectionProperty { get; set; }
    }
}
