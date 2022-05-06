using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TestCommon;
using GovUk.Frontend.AspNetCore.Tests.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.FunctionalTests
{
    public class StartupFilterTests
    {
        [Theory]
        [InlineData("govuk-frontend-{version}.min.css")]
        [InlineData("govuk-frontend-{version}.min.js")]
        public async Task HostsStaticAssets(string path)
        {
            // Arrange
            using var fixture = new StartupFilterTestFixture(services => { });
            await fixture.InitializeAsync();

            var resolvedPath = path.Replace("{version}", HtmlSnippets.GdsLibraryVersion);

            var request = new HttpRequestMessage(HttpMethod.Get, resolvedPath);

            // Act
            var response = await fixture.HttpClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AddImportsToHtmlIsFalse_DoesNotAddStyleAndScriptImportsToRazorViews()
        {
            // Arrange
            using var fixture = new StartupFilterTestFixture(services =>
            {
                services.Configure<GovUkFrontendAspNetCoreOptions>(options => options.AddImportsToHtml = false);
            });
            await fixture.InitializeAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, "/Empty");

            // Act
            var response = await fixture.HttpClient.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            var html = await response.GetHtmlDocument();

            var head = html.QuerySelector("head");

            var linkTags = head.QuerySelectorAll("link");

            Assert.DoesNotContain(
                $"/govuk-frontend-{HtmlSnippets.GdsLibraryVersion}.min.css",
                linkTags
                    .Where(t => t.GetAttribute("rel") == "stylesheet")
                    .Select(t => t.GetAttribute("href")));

            var body = html.QuerySelector("body");

            Assert.DoesNotContain("govuk-template__body", body.ClassList);

            var bodyScriptTags = body.QuerySelectorAll("script");

            Assert.DoesNotContain(
                $"/govuk-frontend-{HtmlSnippets.GdsLibraryVersion}.min.js",
                bodyScriptTags.Select(t => t.GetAttribute("src")));

            Assert.DoesNotContain(
                $"window.GOVUKFrontend.initAll()",
                bodyScriptTags.Select(t => t.InnerHtml));
        }

        [Fact]
        public async Task AddImportsToHtmlIsTrue_AddsStyleAndScriptImportsToRazorViews()
        {
            // Arrange
            using var fixture = new StartupFilterTestFixture(services =>
            {
                services.Configure<GovUkFrontendAspNetCoreOptions>(options => options.AddImportsToHtml = true);
            });
            await fixture.InitializeAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, "/Empty");

            // Act
            var response = await fixture.HttpClient.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            var html = await response.GetHtmlDocument();

            var head = html.QuerySelector("head");

            var linkTags = head.QuerySelectorAll("link");

            Assert.Contains(
                $"/govuk-frontend-{HtmlSnippets.GdsLibraryVersion}.min.css",
                linkTags
                    .Where(t => t.GetAttribute("rel") == "stylesheet")
                    .Select(t => t.GetAttribute("href")));

            var body = html.QuerySelector("body");

            Assert.Contains("govuk-template__body", body.ClassList);

            var bodyScriptTags = body.QuerySelectorAll("script");

            Assert.Contains(
                $"/govuk-frontend-{HtmlSnippets.GdsLibraryVersion}.min.js",
                bodyScriptTags.Select(t => t.GetAttribute("src")));

            Assert.Contains(
                $"window.GOVUKFrontend.initAll()",
                bodyScriptTags.Select(t => t.InnerHtml));
        }
    }

    public class StartupFilterTestFixture : TestServerFixtureBase
    {
        private readonly Action<IServiceCollection> _configureServices;

        public StartupFilterTestFixture(Action<IServiceCollection> configureServices)
        {
            _configureServices = configureServices;
        }

        protected override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddRazorPages();

            _configureServices?.Invoke(services);
        }
    }
}
