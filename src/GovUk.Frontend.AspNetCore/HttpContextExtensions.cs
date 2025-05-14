using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Http;

namespace GovUk.Frontend.AspNetCore;

internal static class HttpContextExtensions
{
    internal static PageErrorContext GetContainerErrorContext(this HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        if (httpContext.Items.TryGetValue(typeof(PageErrorContext), out var containerErrorContextObj) &&
            containerErrorContextObj is PageErrorContext containerErrorContext)
        {
            return containerErrorContext;
        }

        containerErrorContext = new PageErrorContext();
        SetContainerErrorContext(httpContext, containerErrorContext);
        return containerErrorContext;
    }

    internal static void SetContainerErrorContext(this HttpContext httpContext, PageErrorContext pageErrorContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(pageErrorContext);

        httpContext.Items[typeof(PageErrorContext)] = pageErrorContext;
    }
}
