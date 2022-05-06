using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class DateInputFieldsetTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsFieldsetToContext()
        {
            // Arrange
            var dateInputContext = new DateInputContext(haveExplicitValue: false);

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
                    fieldsetContext.SetLegend(isPageHeading: true, attributes: null, content: new HtmlString("Legend"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new DateInputFieldsetTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.True(dateInputContext.Fieldset?.Legend?.IsPageHeading);
            Assert.Equal("Legend", dateInputContext.Fieldset?.Legend?.Content?.RenderToString());
        }

        [Fact]
        public async Task ProcessAsync_ParentAlreadyHasFieldset_ThrowsInvalidOperationException()
        {
            // Arrange
            var dateInputContext = new DateInputContext(haveExplicitValue: false);

            dateInputContext.OpenFieldset();
            var checkboxesFieldsetContext = new DateInputFieldsetContext(attributes: null);
            checkboxesFieldsetContext.SetLegend(isPageHeading: false, attributes: null, content: new HtmlString("Existing legend"));
            dateInputContext.CloseFieldset(checkboxesFieldsetContext);

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
                    fieldsetContext.SetLegend(isPageHeading: true, attributes: null, content: new HtmlString("Legend"));

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
}
