using System;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Http;

namespace GovUk.Frontend.AspNetCore;

internal static class HttpContextExtensions
{
    internal static ContainerErrorContext GetContainerErrorContext(this HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        if (httpContext.Items.TryGetValue(typeof(ContainerErrorContext), out var containerErrorContextObj) &&
            containerErrorContextObj is ContainerErrorContext containerErrorContext)
        {
            return containerErrorContext;
        }

        containerErrorContext = new ContainerErrorContext();
        SetContainerErrorContext(httpContext, containerErrorContext);
        return containerErrorContext;
    }

    internal static void SetContainerErrorContext(this HttpContext httpContext, ContainerErrorContext containerErrorContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(containerErrorContext);

        httpContext.Items[typeof(ContainerErrorContext)] = containerErrorContext;
    }
}
