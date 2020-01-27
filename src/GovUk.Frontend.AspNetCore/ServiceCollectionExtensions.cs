using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            Action<GovUkFrontendAspNetCoreOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.TryAddSingleton<IGovUkHtmlGenerator, DefaultGovUkHtmlGenerator>();

            services.Configure(setupAction);

            return services;
        }
    }
}
