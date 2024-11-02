using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class DateInputFieldsetTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddsFieldsetToContext()
    {
        // Arrange
        var legendHtml = "Legend";
        var isPageHeading = true;
        var describedBy = "describedby";

        var dateInputContext = new DateInputContext(haveExplicitValue: false, @for: null);

        var context = new TagHelperContext(
            tagName: "govuk-date-input-fieldset",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(DateInputContext), dateInputContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-date-input-fieldset",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var fieldsetContext = context.GetContextItem<DateInputFieldsetContext>();
                fieldsetContext.SetLegend(isPageHeading, attributes: ImmutableDictionary<string, string?>.Empty, html: legendHtml);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputFieldsetTagHelper()
        {
            DescribedBy = describedBy
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(isPageHeading, dateInputContext._fieldset?.Legend?.IsPageHeading);
        Assert.Equal(legendHtml, dateInputContext._fieldset?.Legend?.Html);
        Assert.Equal(describedBy, dateInputContext._fieldset?.DescribedBy);
    }

    [Fact]
    public async Task ProcessAsync_ParentAlreadyHasFieldset_ThrowsInvalidOperationException()
    {
        // Arrange
        var dateInputContext = new DateInputContext(haveExplicitValue: false, @for: null);

        dateInputContext.OpenFieldset();
        var dateInputFieldsetContext = new DateInputFieldsetContext(attributes: ImmutableDictionary<string, string?>.Empty, @for: null, describedBy: null);
        dateInputFieldsetContext.SetLegend(isPageHeading: false, attributes: ImmutableDictionary<string, string?>.Empty, html: "Existing legend");
        dateInputContext.CloseFieldset(dateInputFieldsetContext);

        var context = new TagHelperContext(
            tagName: "govuk-date-input-fieldset",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(DateInputContext), dateInputContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-date-input-fieldset",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var fieldsetContext = context.GetContextItem<DateInputFieldsetContext>();
                fieldsetContext.SetLegend(isPageHeading: true, attributes: ImmutableDictionary<string, string?>.Empty, html: "Legend");

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputFieldsetTagHelper();

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-date-input-fieldset> element is permitted within each <govuk-date-input>.", ex.Message);
    }
}
