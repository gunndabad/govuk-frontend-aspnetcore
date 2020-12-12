using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public class ConformanceTestFixture : IDisposable
    {
        private const string ViewImports = @"@addTagHelper *, GovUk.Frontend.AspNetCore";

        private const string ViewName = "ConformanceTest";

        private readonly IHost _host;

        public ConformanceTestFixture()
        {
#if TEST_SWITCHES
            Config.RunningConformanceTests = true;
#endif

            var host = new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            var stringFileProvider = new StringFileProvider($"/Views/Shared/{ViewName}.cshtml");

                            services.AddSingleton(stringFileProvider);

                            services.AddGovUkFrontend();

                            services
                                .AddMvc()
                                .ConfigureApplicationPartManager(partManager =>
                                {
                                    partManager.FeatureProviders.Add(
                                        new SpecifiedControllerFeatureProvider(typeof(ComponentTestController).GetTypeInfo()));
                                })
                                .AddRazorRuntimeCompilation(options =>
                                {
                                    options.FileProviders.Add(new StringFileProvider("/Views/Shared/_ViewImports.cshtml", ViewImports));

                                    options.FileProviders.Add(stringFileProvider);
                                });
                        })
                        .Configure(app =>
                        {
                            app.UseRouting();

                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapControllers();
                            });
                        });
                })
                .Start();

            _host = host;
            HttpClient = host.GetTestClient();
            Services = host.Services;
        }

        public HttpClient HttpClient { get; private set; }

        public IServiceProvider Services { get; private set; }

        public virtual void Dispose()
        {
            HttpClient.Dispose();
            _host.Dispose();
        }

        private StringFileProvider StringFileProvider => Services.GetRequiredService<StringFileProvider>();

        public async Task<string> RenderRazorTemplate(string template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(template);
            }

            StringFileProvider.Value = template;

            var rendered = await HttpClient.GetAsync("");
            rendered.EnsureSuccessStatusCode();

            return await rendered.Content.ReadAsStringAsync();
        }

        private class ComponentTestController : Controller
        {
            [HttpGet("")]
            public IActionResult RenderView() => View(ViewName);
        }
    }
}
