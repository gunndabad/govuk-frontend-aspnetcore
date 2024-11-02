using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class CheckboxesFieldsetTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddsFieldsetToContext()
    {
        // Arrange
        var checkboxesContext = new CheckboxesContext(name: null, aspFor: null);

        var context = new TagHelperContext(
            tagName: "govuk-checkboxes-fieldset",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>() { { typeof(CheckboxesContext), checkboxesContext } },
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-checkboxes-fieldset",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var fieldsetContext = context.GetContextItem<CheckboxesFieldsetContext>();
                fieldsetContext.SetLegend(isPageHeading: true, attributes: null, content: new HtmlString("Legend"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new CheckboxesFieldsetTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.True(checkboxesContext.Fieldset?.Legend?.IsPageHeading);
        Assert.Equal("Legend", checkboxesContext.Fieldset?.Legend?.Content?.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_ParentAlreadyHasFieldset_ThrowsInvalidOperationException()
    {
        // Arrange
        var checkboxesContext = new CheckboxesContext(name: null, aspFor: null);

        checkboxesContext.OpenFieldset();
        var checkboxesFieldsetContext = new CheckboxesFieldsetContext(attributes: null, aspFor: null);
        checkboxesFieldsetContext.SetLegend(
            isPageHeading: false,
            attributes: null,
            content: new HtmlString("Existing legend")
        );
        checkboxesContext.CloseFieldset(checkboxesFieldsetContext);

        var context = new TagHelperContext(
            tagName: "govuk-checkboxes-fieldset",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>() { { typeof(CheckboxesContext), checkboxesContext } },
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-checkboxes-fieldset",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var fieldsetContext = context.GetContextItem<CheckboxesFieldsetContext>();
                fieldsetContext.SetLegend(isPageHeading: true, attributes: null, content: new HtmlString("Legend"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new CheckboxesFieldsetTagHelper();

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal(
            "Only one <govuk-checkboxes-fieldset> element is permitted within each <govuk-checkboxes>.",
            ex.Message
        );
    }
}
