using System;
using Microsoft.AspNetCore.Builder;

namespace GovUk.Frontend.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
        [Obsolete("Static assets are now added automatically.")]
        public static IApplicationBuilder UseGdsFrontEnd(this IApplicationBuilder builder) => builder;
    }
}
