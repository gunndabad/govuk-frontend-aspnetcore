using AngleSharp.Diffing.Core;
using GovUk.Frontend.AspNetCore.Components;

namespace GovUk.Frontend.AspNetCore.Tests.Components;

public class ComponentTests
{
    private readonly LegacyComponentGenerator _componentGenerator;

    public ComponentTests()
    {
        _componentGenerator = new LegacyComponentGenerator();
    }

    [Theory]
    [ComponentFixtureData("input", typeof(TextInputOptions), exclude: ["with extra letter spacing", "disabled"])]
    public void TextInput(ComponentTestCaseData<TextInputOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateTextInput(options).ToHtmlString());

    private void CheckComponentHtmlMatchesExpectedHtml<TOptions>(
        ComponentTestCaseData<TOptions> testCaseData,
        Func<ILegacyComponentGenerator, TOptions, string> generateComponent,
        Predicate<IDiff>? excludeDiff = null)
    {
        var html = generateComponent(_componentGenerator, testCaseData.Options);

        AssertEx.HtmlEqual(testCaseData.ExpectedHtml, html, excludeDiff);
    }
}
