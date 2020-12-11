using System;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.ModelBinding;
using GovUk.Frontend.AspNetCore.TagHelperComponents;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
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
            Action<GovUkFrontendAspNetCoreOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.TryAddSingleton<IGovUkHtmlGenerator, ComponentGenerator>();
            services.TryAddSingleton<IModelHelper, DefaultModelHelper>();
            services.AddSingleton<IStartupFilter, GovUkFrontendAspNetCoreStartupFilter>();
            services.AddTransient<ITagHelperComponent, GdsImportsTagHelperComponent>();

            services.Configure<MvcOptions>(options =>
            {
                options.ModelBinderProviders.Insert(0, new DateInputModelBinderProvider());
            });

            services.Configure(configureOptions);

            return services;
        }
    }
}
