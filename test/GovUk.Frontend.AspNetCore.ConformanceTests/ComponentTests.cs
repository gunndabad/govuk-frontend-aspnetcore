#if NETCOREAPP3_1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Diffing.Core;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public class ComponentTests : IClassFixture<ConformanceTestFixture>
    {
        private readonly ConformanceTestFixture _fixture;

        public ComponentTests(ConformanceTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [ComponentFixtureData(
            "accordion",
            typeof(OptionsJson.Accordion),
            exclude: "with falsey values")]
        public Task Accordion(ComponentTestCaseData<OptionsJson.Accordion> data) =>
            CheckTagHelperOutputMatchesExpectedHtml(
                data,
                (generator, options) => generator.GenerateAccordion(options),
                excludeDiff: diff => diff is AttrDiff attrDiff &&
                    ((attrDiff.Test.Attribute.Name == "id" && attrDiff.Test.Attribute.Value.StartsWith("GFA_test")) ||
                    (attrDiff.Test.Attribute.Name == "aria-labelledby" && attrDiff.Test.Attribute.Value.StartsWith("GFA_test"))));

        [Theory]
        [ComponentFixtureData("back-link", typeof(OptionsJson.BackLink))]
        public Task BackLink(ComponentTestCaseData<OptionsJson.BackLink> data) =>
            CheckTagHelperOutputMatchesExpectedHtml(
                data,
                (generator, options) => generator.GenerateBackLink(options));

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
                "input type"
            })]
        public Task Button(ComponentTestCaseData<OptionsJson.Button> data) =>
            CheckTagHelperOutputMatchesExpectedHtml(
                data,
                (generator, options) => generator.GenerateButton(options));

        [Theory]
        [ComponentFixtureData("details", typeof(OptionsJson.Details))]
        public Task Details(ComponentTestCaseData<OptionsJson.Details> data) =>
            CheckTagHelperOutputMatchesExpectedHtml(
                data,
                (generator, options) => generator.GenerateDetails(options));

        [Theory]
        [ComponentFixtureData("error-message", typeof(OptionsJson.ErrorMessage))]
        public Task ErrorMessage(ComponentTestCaseData<OptionsJson.ErrorMessage> data) =>
            CheckTagHelperOutputMatchesExpectedHtml(
                data,
                (generator, options) => generator.GenerateErrorMessage(options));

        [Theory]
        [ComponentFixtureData("hint", typeof(OptionsJson.Hint))]
        public Task Hint(ComponentTestCaseData<OptionsJson.Hint> data) =>
            CheckTagHelperOutputMatchesExpectedHtml(
                data,
                (_, options) =>
                {
                    // No tag helper for this element - go direct to ComponentGenerator

                    var attributes = (options.Attributes ?? new Dictionary<string, object>())
                        .ToDictionary(a => a.Key, a => a.Value.ToString());

                    if (options.Classes != null)
                    {
                        attributes.Add("class", options.Classes);
                    }

                    var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html) ??
                        new HtmlString(string.Empty);

                    var generator = new ComponentGenerator();

                    return generator.GenerateHint(options.Id, content, attributes).RenderToString();
                });

        protected Task<string> RenderRazorTemplate(string template) => _fixture.RenderRazorTemplate(template);

        private async Task CheckTagHelperOutputMatchesExpectedHtml<TOptions>(
            ComponentTestCaseData<TOptions> testCaseData,
            Func<RazorGenerator, TOptions, string> generateRazor,
            Predicate<IDiff> excludeDiff = null)
        {
            var razorGenerator = new RazorGenerator();
            var template = generateRazor(razorGenerator, testCaseData.Options);

            var html = await RenderRazorTemplate(template);

            AssertEx.HtmlEqual(testCaseData.ExpectedHtml, html, excludeDiff);
        }
    }
}
#endif
