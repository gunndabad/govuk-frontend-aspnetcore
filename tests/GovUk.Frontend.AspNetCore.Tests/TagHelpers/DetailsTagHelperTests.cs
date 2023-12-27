using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class DetailsTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var id = "id";
        var open = true;
        var summaryHtml = "summary";
        var content = "content";
        var classes = "custom-class";
        var dataFooAttrValue = "bar";

        var context = new TagHelperContext(
            tagName: "govuk-details",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-details",
            attributes: new TagHelperAttributeList()
            {
                { "class", classes },
                { "data-foo", dataFooAttrValue },
            },
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var detailsContext = context.GetContextItem<DetailsContext>();

                detailsContext.SetSummary(ImmutableDictionary<string, string?>.Empty, summaryHtml);

                detailsContext.SetText(ImmutableDictionary<string, string?>.Empty, content);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        DetailsOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateDetails(It.IsAny<DetailsOptions>())).Callback<DetailsOptions>(o => actualOptions = o);

        var tagHelper = new DetailsTagHelper(componentGeneratorMock.Object)
        {
            Id = id,
            Open = open
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(id, actualOptions!.Id);
        Assert.Equal(open, actualOptions.Open);
        Assert.Equal(summaryHtml, actualOptions.SummaryHtml);
        Assert.Null(actualOptions.SummaryText);
        Assert.Equal(content, actualOptions.Html);
        Assert.Null(actualOptions.Text);
        Assert.Equal(classes, actualOptions.Classes);
        Assert.Collection(actualOptions.Attributes, kvp =>
        {
            Assert.Equal("data-foo", kvp.Key);
            Assert.Equal(dataFooAttrValue, kvp.Value);
        });
    }
}
