using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelperComponents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using HtmlAgilityPack;
using GovUk.Frontend.AspNetCore.Tests.Infrastructure;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.FunctionalTests
{
    public class StartupFilterTests : IClassFixture<StartupFilterTestFixture>
    {
        private readonly StartupFilterTestFixture _fixture;

        public StartupFilterTests(StartupFilterTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void AddImportsToHtmlIsFalse_DoesNotAddGdsImportsTagHelperComponentToServices()
        {
            // Arrange & Act
            IServiceProvider services = new ServiceCollection()
                .AddGovUkFrontend(new GovUkFrontendAspNetCoreOptions() { AddImportsToHtml = false })
                .BuildServiceProvider();

            var tagHelperComponents = services.GetServices<ITagHelperComponent>();

            // Assert
            Assert.DoesNotContain(tagHelperComponents, components => components is GdsImportsTagHelperComponent);
        }

        [Theory]
        [InlineData("govuk-frontend-{version}.min.css")]
        [InlineData("govuk-frontend-{version}.min.js")]
        public async Task HostsStaticAssets(string path)
        {
            // Arrange
            var resolvedPath = path.Replace("{version}", HtmlSnippets.GdsLibraryVersion);

            var request = new HttpRequestMessage(HttpMethod.Get, resolvedPath);

            // Act
            var response = await _fixture.HttpClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AddsStyleAndScriptImportsToRazorViews()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/Empty");

            // Act
            var response = await _fixture.HttpClient.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            var html = HtmlNode.CreateNode(await response.Content.ReadAsStringAsync());

            var head = html.SelectSingleNode("head");

            var linkTags = head.SelectNodes("link");

            Assert.Contains(
                $"/govuk-frontend-{HtmlSnippets.GdsLibraryVersion}.min.css",
                linkTags
                    .Where(t => t.GetAttributeValue("rel", "") == "stylesheet")
                    .Select(t => t.GetAttributeValue("href", "")));

            var body = html.SelectSingleNode("body");

            Assert.Contains("govuk-template__body", body.GetClasses());

            var bodyScriptTags = body.SelectNodes("script");

            Assert.Contains(
                $"/govuk-frontend-{HtmlSnippets.GdsLibraryVersion}.min.js",
                bodyScriptTags.Select(t => t.GetAttributeValue("src", "")));

            Assert.Contains(
                $"window.GOVUKFrontend.initAll()",
                bodyScriptTags.Select(t => t.InnerText));
        }
    }

    public class StartupFilterTestFixture : TestServerFixtureBase
    {
        public StartupFilterTestFixture()
            : base(ConfigureServices, Configure)
        {
        }

        private static void Configure(IApplicationBuilder app)
        {
#if NETCOREAPP2_1
            app.UseMvc();
#else
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
#endif
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddGovUkFrontend();

#if NETCOREAPP2_1
            services.AddMvc();
#else
            services.AddRazorPages();
#endif
        }
    }
}
