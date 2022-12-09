#nullable disable
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
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGovUkFrontend(this IServiceCollection services)
        {
            return AddGovUkFrontend(services, _ => { });
        }

        public static IServiceCollection AddGovUkFrontend(
            this IServiceCollection services,
            Action<GovUkFrontendAspNetCoreOptions> configureOptions)
        {
            Guard.ArgumentNotNull(nameof(services), services);
            Guard.ArgumentNotNull(nameof(configureOptions), configureOptions);

            services.TryAddSingleton<IGovUkHtmlGenerator, ComponentGenerator>();
            services.TryAddSingleton<IModelHelper, DefaultModelHelper>();
            services.AddSingleton<IStartupFilter, GovUkFrontendAspNetCoreStartupFilter>();
            services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureMvcOptions>();
            services.AddScoped<DateInputParseErrorsProvider>();
            services.AddTransient<ITagHelperComponent, GdsImportsTagHelperComponent>();
            services.AddTransient<PageTemplateHelper>();

            services.Configure(configureOptions);

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
                options.ModelBinderProviders.Insert(0, new DateInputModelBinderProvider(_gfaOptions));
            }
        }
    }
}
