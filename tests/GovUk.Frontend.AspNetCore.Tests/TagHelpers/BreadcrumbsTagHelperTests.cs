using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class BreadcrumbsTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        BreadcrumbsOptionsItem[] items =
        [
            new BreadcrumbsOptionsItem()
            {
                Href = new HtmlString("first"),
                Html = new HtmlString("First")
            },
            new BreadcrumbsOptionsItem()
            {
                Href = new HtmlString("second"),
                Html = new HtmlString("Second")
            },
            new BreadcrumbsOptionsItem()
            {
                Html = new HtmlString("Last")
            }
        ];

        var collapseOnMobile = true;
        var labelText = "Label text";

        var context = new TagHelperContext(
            tagName: "govuk-breadcrumbs",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-breadcrumbs",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var breadcrumbsContext = context.GetContextItem<BreadcrumbsContext>();

                foreach (var item in items)
                {
                    breadcrumbsContext.AddItem(item);
                }

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        BreadcrumbsOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateBreadcrumbs(It.IsAny<BreadcrumbsOptions>())).Callback<BreadcrumbsOptions>(o => actualOptions = o);

        var tagHelper = new BreadcrumbsTagHelper(componentGeneratorMock.Object)
        {
            CollapseOnMobile = collapseOnMobile,
            LabelText = labelText,
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(collapseOnMobile, actualOptions.CollapseOnMobile);
        Assert.NotNull(actualOptions.Items);
        Assert.Collection(
            actualOptions.Items,
            item => Assert.Same(items[0], item),
            item => Assert.Same(items[1], item),
            item => Assert.Same(items[2], item));
        Assert.Equal(labelText, actualOptions.LabelText?.ToHtmlString());
    }
}
