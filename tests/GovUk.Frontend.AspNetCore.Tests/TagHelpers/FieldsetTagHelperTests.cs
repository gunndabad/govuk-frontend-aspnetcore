using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class FieldsetTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var describedBy = "describedby";
        var role = "role";
        var html = "Main content";
        var legendHtml = "Legend text";
        var legendIsPageHeading = true;

        var context = new TagHelperContext(
            tagName: "govuk-fieldset",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-fieldset",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var fieldsetContext = context.GetContextItem<FieldsetContext>();

                fieldsetContext.SetLegend(
                    isPageHeading: legendIsPageHeading,
                    attributes: ImmutableDictionary<string, string?>.Empty,
                    html: legendHtml);

                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(html);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        FieldsetOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateFieldset(It.IsAny<FieldsetOptions>())).Callback<FieldsetOptions>(o => actualOptions = o);

        var tagHelper = new FieldsetTagHelper(componentGeneratorMock.Object)
        {
            DescribedBy = describedBy,
            Role = role
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(describedBy, actualOptions!.DescribedBy);
        Assert.NotNull(actualOptions.Legend);
        Assert.Equal(legendIsPageHeading, actualOptions.Legend!.IsPageHeading);
        Assert.Null(actualOptions.Legend!.Text);
        Assert.Equal(legendHtml, actualOptions.Legend!.Html);
        Assert.Equal(role, actualOptions.Role);
        Assert.Null(actualOptions.Text);
        Assert.Equal(html, actualOptions.Html);
    }

    [Fact]
    public async Task ProcessAsync_MissingLegend_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-fieldset",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-fieldset",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var fieldsetContext = context.GetContextItem<FieldsetContext>();

                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Main content");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };

        var tagHelper = new FieldsetTagHelper(componentGeneratorMock.Object)
        {
            DescribedBy = "describedby",
            Role = "therole"
        };

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("A <govuk-fieldset-legend> element must be provided.", ex.Message);
    }
}
