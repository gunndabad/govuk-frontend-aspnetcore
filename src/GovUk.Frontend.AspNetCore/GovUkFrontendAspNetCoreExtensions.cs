using System;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.ModelBinding;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore;

/// <summary>
/// Extension methods for setting up GovUk.Frontend.AspNetCore.
/// </summary>
public static class GovUkFrontendAspNetCoreExtensions
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
        services.TryAddSingleton<ILegacyComponentGenerator, DefaultComponentGenerator>();
        services.TryAddSingleton<IComponentGenerator, FluidComponentGenerator>();
        services.TryAddSingleton<IModelHelper, DefaultModelHelper>();
        services.AddSingleton<IStartupFilter, GovUkFrontendAspNetCoreStartupFilter>();
        services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureMvcOptions>();
        services.AddScoped<DateInputParseErrorsProvider>();
        services.AddTransient<PageTemplateHelper>();
        services.AddSingleton<ITagHelperInitializer<ButtonTagHelper>, ButtonTagHelperInitializer>();

        services.Configure(setupAction);

        return services;
    }

    /// <summary>
    /// Replaces the <see cref="IModelBinderProvider"/> of type <typeparamref name="T"/> with <paramref name="modelBinderProvider"/>.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IModelBinderProvider"/> to replace.</typeparam>
    /// <param name="mvcOptions">The <see cref="MvcOptions"/>.</param>
    /// <param name="modelBinderProvider">The <see cref="IModelBinderProvider"/> to replace with.</param>
    /// <returns><see langword="true"/> if a <see cref="IModelBinderProvider"/> was replaced otherwise <see langword="false"/>.</returns>
    public static bool ReplaceModelBinderProvider<T>(this MvcOptions mvcOptions, IModelBinderProvider modelBinderProvider)
        where T : IModelBinderProvider
    {
        Guard.ArgumentNotNull(nameof(mvcOptions), mvcOptions);

        var modelBinderProviders = mvcOptions.ModelBinderProviders;

        for (var i = 0; i < modelBinderProviders.Count; i++)
        {
            if (modelBinderProviders[i] is T)
            {
                modelBinderProviders[i] = modelBinderProvider;
                return true;
            }
        }

        return false;
    }

    private class ConfigureMvcOptions : IConfigureOptions<MvcOptions>
    {
        private readonly IOptions<GovUkFrontendAspNetCoreOptions> _gfaOptionsAccessor;

        public ConfigureMvcOptions(IOptions<GovUkFrontendAspNetCoreOptions> gfaOptionsAccessor)
        {
            _gfaOptionsAccessor = gfaOptionsAccessor;
        }

        public void Configure(MvcOptions options)
        {
            options.ModelBinderProviders.Insert(2, new DateInputModelBinderProvider(_gfaOptionsAccessor));
            options.ModelMetadataDetailsProviders.Add(new GovUkFrontendAspNetCoreMetadataDetailsProvider());
        }
    }
}
