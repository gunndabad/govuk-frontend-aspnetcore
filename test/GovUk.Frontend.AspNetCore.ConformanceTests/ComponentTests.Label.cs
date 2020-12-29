using AngleSharp.Diffing.Core;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class ComponentTests
    {
        [Theory]
        [ComponentFixtureData("label", typeof(OptionsJson.Label), exclude: "empty")]
        public void Label(ComponentTestCaseData<OptionsJson.Label> data) =>
            CheckComponentHtmlMatchesExpectedHtml(
                data,
                (generator, options) =>
                {
                    var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html) ?? _emptyContent;

                    var attributes = options.Attributes.ToAttributesDictionary()
                        .MergeAttribute("class", options.Classes);

                    return generator.GenerateLabel(
                            options.For ?? "GFA_test",
                            options.IsPageHeading ?? ComponentGenerator.LabelDefaultIsPageHeading,
                            content,
                            attributes)
                        .RenderToString();
                },
                excludeDiff: diff => diff is UnexpectedAttrDiff unexpectedAttrDiff &&
                    unexpectedAttrDiff.Test.Attribute.Name == "for");
    }
}
