#if !NETCOREAPP2_1
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.Tests.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace GovUk.Frontend.AspNetCore.Tests.ConformanceTests
{
    public class ConformanceTestFixture : TestServerFixtureBase
    {
        private const string ViewImports = @"@addTagHelper *, GovUk.Frontend.AspNetCore";

        private const string ViewName = "ConformanceTest";

        public ConformanceTestFixture()
            : base(ConfigureServices, Configure)
        {
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

        private static void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var stringFileProvider = new StringFileProvider($"/Views/Shared/{ViewName}.cshtml");

            services.AddSingleton(stringFileProvider);

            services.AddGovUkFrontend();

            services.Configure<GovUkFrontendAspNetCoreOptions>(options => options.RunningConformanceTests = true);

            services
                .AddMvc(options =>
                {
                    options.Conventions.Add(
                        new LimitControllerNamespaceConvention(typeof(ConformanceTestFixture).Namespace));
                })
                .ConfigureApplicationPartManager(partManager =>
                {
                    partManager.FeatureProviders.Add(
                        new SpecifiedControllerFeatureProvider(
                            typeof(ComponentTestController).GetTypeInfo()));
                })
                .AddRazorRuntimeCompilation(options =>
                {
                    options.FileProviders.Add(new StringFileProvider("/Views/Shared/_ViewImports.cshtml", ViewImports));

                    options.FileProviders.Add(stringFileProvider);
                });
        }

        private class ComponentTestController : Controller
        {
            [HttpGet("")]
            public IActionResult RenderView() => View(ViewName);
        }
    }
}
#endif
