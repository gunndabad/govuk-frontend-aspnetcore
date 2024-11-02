using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class DateInputItemTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddItemToContext()
    {
        // Arrange
        var autocomplete = "off";
        var id = "my-day";
        var inputMode = "im";
        var labelHtml = "Label";
        var name = "my_day";
        var pattern = "*";
        var value = 2;

        var dateInputContext = new DateInputContext(haveExplicitValue: false, @for: null);

        var context = new TagHelperContext(
            tagName: "govuk-date-input-day",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(DateInputContext), dateInputContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-date-input-day",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var itemContext = context.GetContextItem<DateInputItemContext>();
                itemContext.SetLabel(attributes: ImmutableDictionary<string, string?>.Empty, html: labelHtml);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputItemTagHelper()
        {
            Autocomplete = autocomplete,
            Id = id,
            InputMode = inputMode,
            Name = name,
            Pattern = pattern,
            Value = value
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            dateInputContext.Items.Values,
            item =>
            {
                Assert.Equal(autocomplete, item.Autocomplete);
                Assert.Equal(id, item.Id);
                Assert.Equal(inputMode, item.InputMode);
                Assert.Equal(labelHtml, item.LabelHtml);
                Assert.Equal(name, item.Name);
                Assert.Equal(pattern, item.Pattern);
                Assert.Equal(value, item.Value);
                Assert.True(item.ValueSpecified);
            });
    }
}
