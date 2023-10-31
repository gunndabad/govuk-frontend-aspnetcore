using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    [Theory]
    [ComponentFixtureData(
        "button",
        typeof(OptionsJson.Button),
        exclude: new[]
        {
            "input",
            "input disabled",
            "input attributes",
            "input classes",
            "input type",
            "id"  // uses input type
        })]
    public void Button(ComponentTestCaseData<OptionsJson.Button> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) =>
            {
                var isButtonLink = options.Element == "a" || options.Href != null;

                var isStartButton = options.IsStartButton ?? ComponentGenerator.ButtonDefaultIsStartButton;
                var disabled = options.Disabled ?? ComponentGenerator.ButtonDefaultDisabled;
                var preventDoubleClick = options.PreventDoubleClick;
                var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html) ?? _emptyContent;

                var attributes = options.Attributes.ToAttributesDictionary()
                    .MergeAttribute("class", options.Classes);

                if (isButtonLink)
                {
                    attributes.MergeAttribute("href", options.Href ?? "#");
                }
                else
                {
                    attributes.MergeAttribute("name", options.Name);
                    attributes.MergeAttribute("type", options.Type ?? "submit");
                    attributes.MergeAttribute("value", options.Value);
                }

                return (
                    isButtonLink ?
                        generator.GenerateButtonLink(isStartButton, disabled, options.Id, content, attributes) :
                        generator.GenerateButton(isStartButton, disabled, preventDoubleClick, options.Id, content, attributes)
                    ).ToHtmlString();
            });
}
