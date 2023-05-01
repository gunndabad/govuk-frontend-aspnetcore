using System;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.ModelBinding;
using GovUk.Frontend.AspNetCore.TagHelperComponents;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore
{
    /// <summary>
    /// Extension methods for setting up GovUk.Frontend.AspNetCore services in an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds GovUk.Frontend.AspNetCore services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddGovUkFrontend(this IServiceCollection services)
        {
            return AddGovUkFrontend(services, _ => { });
        }

        /// <summary>
        /// Adds GovUk.Frontend.AspNetCore services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An <see cref="Action{GovUkFrontendAspNetCoreOptions}"/> to configure the provided <see cref="GovUkFrontendAspNetCoreOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddGovUkFrontend(
            this IServiceCollection services,
            Action<GovUkFrontendAspNetCoreOptions> setupAction)
        {
            Guard.ArgumentNotNull(nameof(services), services);
            Guard.ArgumentNotNull(nameof(setupAction), setupAction);

            services.AddMvcCore();

            services.TryAddSingleton<IGovUkHtmlGenerator, ComponentGenerator>();
            services.TryAddSingleton<IModelHelper, DefaultModelHelper>();
            services.AddSingleton<IStartupFilter, GovUkFrontendAspNetCoreStartupFilter>();
            services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureMvcOptions>();
            services.AddScoped<DateInputParseErrorsProvider>();
            services.AddTransient<ITagHelperComponent, GdsImportsTagHelperComponent>();
            services.AddTransient<PageTemplateHelper>();

            services.Configure(setupAction);

            return services;
        }

        private class ConfigureMvcOptions : IConfigureOptions<MvcOptions>
        {
            private readonly GovUkFrontendAspNetCoreOptions _gfaOptions;

            public ConfigureMvcOptions(IOptions<GovUkFrontendAspNetCoreOptions> gfaOptionsAccessor)
            {
                _gfaOptions = gfaOptionsAccessor.Value;
            }

            public void Configure(MvcOptions options)
            {
                options.ModelBinderProviders.Insert(2, new DateInputModelBinderProvider(_gfaOptions));
            }
        }
    }
}
