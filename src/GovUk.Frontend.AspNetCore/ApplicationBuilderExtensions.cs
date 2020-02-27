using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace GovUk.Frontend.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGdsFrontEnd(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new ManifestEmbeddedFileProvider(
                    typeof(ApplicationBuilderExtensions).Assembly,
                    root: "Content"),
                RequestPath = ""  // important - CSS assumes things are available at the root
            });
        }
    }
}
