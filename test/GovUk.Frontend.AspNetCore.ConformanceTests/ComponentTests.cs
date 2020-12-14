#if NETCOREAPP3_1
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
        public async Task Accordion(ComponentTestCaseData<OptionsJson.Accordion> data)
        {
            var razorGenerator = new RazorGenerator();
            var template = razorGenerator.GenerateAccordion(data.Options);

            var html = await RenderRazorTemplate(template);

            AssertEx.HtmlEqual(data.ExpectedHtml, html);
        }

        protected Task<string> RenderRazorTemplate(string template) => _fixture.RenderRazorTemplate(template);
    }
}
#endif
