using System.Collections.Generic;
using System.Threading.Tasks;
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
    public class SelectItemTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddItemsToContext()
        {
            // Arrange
            var selectContext = new SelectContext(aspFor: null);

            var context = new TagHelperContext(
                tagName: "govuk-select-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(SelectContext), selectContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-select-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Item text"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SelectItemTagHelper()
            {
                Disabled = true,
                Selected = true,
                Value = "value"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Collection(
                selectContext.Items,
                item =>
                {
                    Assert.Equal("Item text", item.Content?.RenderToString());
                    Assert.True(item.Disabled);
                    Assert.True(item.Selected);
                    Assert.Equal("value", item.Value);
                });
        }

        [Theory]
        [InlineData("bar", true)]
        [InlineData("baz", false)]
        public async Task ProcessAsync_DeducesSelectedFromModelExpression(string modelValue, bool expectedSelected)
        {
            // Arrange
            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model() { SimpleProperty = modelValue })
                .GetExplorerForProperty(nameof(Model.SimpleProperty));
            var viewContext = new ViewContext();
            var modelExpression = nameof(Model.SimpleProperty);

            var modelHelper = new Mock<IModelHelper>();
            modelHelper.Setup(mock => mock.GetModelValue(viewContext, modelExplorer, modelExpression)).Returns(modelValue);

            var selectContext = new SelectContext(aspFor: new ModelExpression(modelExpression, modelExplorer));

            var context = new TagHelperContext(
                tagName: "govuk-select-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(SelectContext), selectContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-select-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Item text"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SelectItemTagHelper(modelHelper.Object)
            {
                Value = "bar",
                ViewContext = viewContext
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Collection(
                selectContext.Items,
                item =>
                {
                    Assert.Equal(expectedSelected, item.Selected);
                });
        }

        [Fact]
        public async Task ProcessAsync_SelectedAttributeExplicitlySet_IgnoresModelExpression()
        {
            // Arrange
            var modelValue = "bar";
            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model() { SimpleProperty = modelValue })
                .GetExplorerForProperty(nameof(Model.SimpleProperty));
            var viewContext = new ViewContext();
            var modelExpression = nameof(Model.SimpleProperty);

            var modelHelper = new Mock<IModelHelper>();
            modelHelper.Setup(mock => mock.GetModelValue(viewContext, modelExplorer, modelExpression)).Returns(modelValue);

            var selectContext = new SelectContext(aspFor: new ModelExpression(modelExpression, modelExplorer));

            var context = new TagHelperContext(
                tagName: "govuk-select-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(SelectContext), selectContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-select-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Item text"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SelectItemTagHelper(modelHelper.Object)
            {
                Selected = false,
                Value = modelValue,
                ViewContext = viewContext
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Collection(
                selectContext.Items,
                item =>
                {
                    Assert.False(item.Selected);
                });
        }

        private class Model
        {
            public string? SimpleProperty { get; set; }
        }
    }
}
