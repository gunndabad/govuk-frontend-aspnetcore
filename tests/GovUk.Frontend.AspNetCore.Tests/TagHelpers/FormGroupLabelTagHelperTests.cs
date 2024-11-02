using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class FormGroupLabelTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_SetsLabelOnContext()
    {
        // Arrange
        var label = "The label";

        var formGroupContext = new TestFormGroupContext();

        var context = new TagHelperContext(
            tagName: "test-label",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(FormGroupContext), formGroupContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "test-label",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(label);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new FormGroupLabelTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(HtmlEncoder.Default.Encode(label), formGroupContext.Label?.Content?.ToHtmlString());
    }

    private class TestFormGroupContext : FormGroupContext
    {
        protected override string ErrorMessageTagName => "test-error-message";

        protected override string HintTagName => "test-hint";

        protected override string LabelTagName => "test-label";

        protected override string RootTagName => "test";
    }
}
