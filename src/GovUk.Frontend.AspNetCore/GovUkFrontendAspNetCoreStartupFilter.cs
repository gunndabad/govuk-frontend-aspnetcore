using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore;

internal class GovUkFrontendAspNetCoreStartupFilter : IStartupFilter
{
    private readonly IOptions<GovUkFrontendAspNetCoreOptions> _optionsAccessor;

    public GovUkFrontendAspNetCoreStartupFilter(IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);
        _optionsAccessor = optionsAccessor;
    }

    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        Guard.ArgumentNotNull(nameof(next), next);

        return app =>
        {
            if (_optionsAccessor.Value.CompiledContentPath is string compiledContentPath)
            {
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new ManifestEmbeddedFileProvider(
                      typeof(GovUkFrontendAspNetCoreStartupFilter).Assembly,
                      root: "Content/Compiled"),
                    RequestPath = compiledContentPath
                });
            }

            if (_optionsAccessor.Value.StaticAssetsContentPath is string assetsContentPath)
            {
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new ManifestEmbeddedFileProvider(
                      typeof(GovUkFrontendAspNetCoreStartupFilter).Assembly,
                      root: "Content/Assets"),
                    RequestPath = assetsContentPath
                });
            }

            next(app);
        };
    }
}
