using System;
using GovUk.Frontend.AspNetCore.TagHelperComponents;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GovUk.Frontend.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGovUkFrontend(this IServiceCollection services)
        {
            return AddGovUkFrontend(services, new GovUkFrontendAspNetCoreOptions());
        }

        public static IServiceCollection AddGovUkFrontend(
            this IServiceCollection services,
            GovUkFrontendAspNetCoreOptions options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            services.AddSingleton(options);

            services.TryAddSingleton<IGovUkHtmlGenerator, DefaultGovUkHtmlGenerator>();

            if (options.AddImportsToHtml)
            {
                services.AddTransient<ITagHelperComponent, GdsImportsTagHelperComponent>();
            }

            return services;
        }
    }
}
