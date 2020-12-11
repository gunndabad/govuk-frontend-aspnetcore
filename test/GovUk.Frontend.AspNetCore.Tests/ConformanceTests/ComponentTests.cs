#if NETCOREAPP3_1
using System.Threading.Tasks;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.ConformanceTests
{
    [Trait("Category", "ConformanceTests")]
    public class ComponentTests : IClassFixture<ConformanceTestFixture>
    {
        private readonly ConformanceTestFixture _fixture;

        public ComponentTests(ConformanceTestFixture fixture)
        {
            _fixture = fixture;
        }

        protected Task<string> RenderRazorTemplate(string template) => _fixture.RenderRazorTemplate(template);
    }
}
#endif
