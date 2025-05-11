using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.Components;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class PaginationTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var landmarkLabel = "Landmark";

        var previousHref = "/place?page=4";
        var previousLabelText = "4 of 9";
        var previousText = "Previous page";

        var currentHref = "place?page=5";
        var currentNumber = "5 of 9";
        var currentVisuallyHiddenText = "vht";

        var nextHref = "/place?page=6";
        var nextLabelText = "6 of 9";
        var nextText = "Next page";

        var context = new TagHelperContext(
            tagName: "govuk-pagination",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-pagination",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var paginationContext = context.GetContextItem<PaginationContext>();

                paginationContext.SetPrevious(new()
                {
                    Href = previousHref,
                    LabelText = previousLabelText,
                    Text = previousText
                });

                paginationContext.AddItem(new PaginationOptionsItem()
                {
                    Number = currentNumber,
                    VisuallyHiddenText = currentVisuallyHiddenText,
                    Href = currentHref,
                    Current = true,
                    Ellipsis = null,
                    Attributes = null
                });

                paginationContext.AddItem(new PaginationOptionsItem()
                {
                    Ellipsis = true
                });

                paginationContext.SetNext(new()
                {
                    Href = nextHref,
                    LabelText = nextLabelText,
                    Text = nextText
                });

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        PaginationOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GeneratePaginationAsync(It.IsAny<PaginationOptions>())).Callback<PaginationOptions>(o => actualOptions = o);

        var tagHelper = new PaginationTagHelper(componentGeneratorMock.Object)
        {
            LandmarkLabel = landmarkLabel
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(landmarkLabel, actualOptions.LandmarkLabel?.ToHtmlString(HtmlEncoder.Default));
        Assert.NotNull(actualOptions.Items);
        Assert.NotNull(actualOptions.Previous);
        Assert.Equal(actualOptions.Previous.Href, previousHref);
        Assert.Equal(actualOptions.Previous.LabelText, previousLabelText);
        Assert.Equal(actualOptions.Previous.Text, previousText);
        Assert.NotNull(actualOptions.Next);
        Assert.Equal(actualOptions.Next.Href, nextHref);
        Assert.Equal(actualOptions.Next.LabelText, nextLabelText);
        Assert.Equal(actualOptions.Next.Text, nextText);
        Assert.Collection(
            actualOptions.Items,
            i =>
            {
                var item = Assert.IsType<PaginationOptionsItem>(i);
                Assert.Equal(currentNumber, item.Number);
                Assert.Equal(currentHref, item.Href);
                Assert.Equal(currentVisuallyHiddenText, item.VisuallyHiddenText);
            },
            i =>
            {
                var item = Assert.IsType<PaginationOptionsItem>(i);
                Assert.True(item.Ellipsis);
            });
    }
}
