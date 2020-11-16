using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Razor.TagHelpers;
using GovUk.Frontend.AspNetCore.TagHelperComponents;
using Microsoft.Extensions.DependencyInjection;
#if NETCOREAPP3_1
using HtmlAgilityPack;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Linq;
#endif
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests
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

#if NETCOREAPP3_1
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
#endif
    }

    public class StartupFilterTestFixture : IDisposable
    {
        private readonly IDisposable _host;

        public StartupFilterTestFixture()
        {
#if NETCOREAPP2_1
            var server = new TestServer(new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddGovUkFrontend();
                })
                .Configure(app =>
                {
                    // .Configure() has to be called, even if it's not used
                }));

            _host = server;
            HttpClient = server.CreateClient();
#else
            var host = new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            services.AddGovUkFrontend();

                            services.AddRazorPages();
                        })
                        .Configure(app =>
                        {
                            app.UseRouting();

                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapRazorPages();
                            });
                        });
                })
                .Start();

            _host = host;
            HttpClient = host.GetTestClient();
#endif
        }

        public HttpClient HttpClient { get; }

        public void Dispose() => _host.Dispose();
    }
}