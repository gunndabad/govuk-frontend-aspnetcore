using System;
using AngleSharp.Diffing.Core;
using GovUk.Frontend.AspNetCore.Components;
using GovUk.Frontend.AspNetCore.TestCommon;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.Components;

public class ComponentTests
{
    private readonly LegacyComponentGenerator _componentGenerator;

    public ComponentTests()
    {
        _componentGenerator = new LegacyComponentGenerator();
    }

    [Theory]
    [ComponentFixtureData("character-count", typeof(CharacterCountOptions))]
    public void CharacterCount(ComponentTestCaseData<CharacterCountOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateCharacterCount(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("tag", typeof(TagOptions))]
    public void Tag(ComponentTestCaseData<TagOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateTag(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("textarea", typeof(TextareaOptions))]
    public void Textarea(ComponentTestCaseData<TextareaOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateTextarea(options).ToHtmlString());

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
