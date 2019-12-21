using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GovUk.Frontend.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGovUkFrontend(this IServiceCollection services)
        {
            services.TryAddSingleton<IGovUkHtmlGenerator, DefaultGovUkHtmlGenerator>();

            return services;
        }
    }
}
