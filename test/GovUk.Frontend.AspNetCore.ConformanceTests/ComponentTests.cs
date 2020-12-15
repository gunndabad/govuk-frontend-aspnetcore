#if NETCOREAPP3_1
using System;
using System.Threading.Tasks;
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
                (generator, options) => generator.GenerateAccordion(options));

        [Theory]
        [ComponentFixtureData("back-link", typeof(OptionsJson.BackLink))]
        public Task BackLink(ComponentTestCaseData<OptionsJson.BackLink> data) =>
            CheckTagHelperOutputMatchesExpectedHtml(
                data,
                (generator, options) => generator.GenerateBackLink(options));

        [Theory]
        [ComponentFixtureData("breadcrumbs", typeof(OptionsJson.Breadcrumbs))]
        public Task Breadcrumbs(ComponentTestCaseData<OptionsJson.Breadcrumbs> data) =>
            CheckTagHelperOutputMatchesExpectedHtml(
                data,
                (generator, options) => generator.GenerateBreadcrumbs(options));

        protected Task<string> RenderRazorTemplate(string template) => _fixture.RenderRazorTemplate(template);

        private async Task CheckTagHelperOutputMatchesExpectedHtml<TOptions>(
            ComponentTestCaseData<TOptions> testCaseData,
            Func<RazorGenerator, TOptions, string> generateRazor)
        {
            var razorGenerator = new RazorGenerator();
            var template = generateRazor(razorGenerator, testCaseData.Options);

            var html = await RenderRazorTemplate(template);

            AssertEx.HtmlEqual(testCaseData.ExpectedHtml, html);
        }
    }
}
#endif
