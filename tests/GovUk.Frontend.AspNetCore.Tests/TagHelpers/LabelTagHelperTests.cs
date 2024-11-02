using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class LabelTagHelperTests
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
            items: new Dictionary<object, object>() { { typeof(FormGroupContext2), formGroupContext } },
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "test-label",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(label);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new LabelTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(HtmlEncoder.Default.Encode(label), formGroupContext.Label?.Html);
    }

    private class TestFormGroupContext : FormGroupContext2
    {
        protected override IReadOnlyCollection<string> ErrorMessageTagNames => [ShortTagNames.ErrorMessage];

        protected override IReadOnlyCollection<string> HintTagNames => [ShortTagNames.Hint];

        protected override IReadOnlyCollection<string> LabelTagNames => [ShortTagNames.Label];

        protected override string RootTagName => "test";
    }
}
