using System;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class ContainerErrorSummaryTagHelperInitializer : ITagHelperInitializer<ContainerErrorSummaryTagHelper>
{
    private readonly IOptions<GovUkFrontendAspNetCoreOptions> _optionsAccessor;

    public ContainerErrorSummaryTagHelperInitializer(IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);
        _optionsAccessor = optionsAccessor;
    }

    public void Initialize(ContainerErrorSummaryTagHelper helper, ViewContext context)
    {
        if (_optionsAccessor.Value.PrependErrorSummaryToForms)
        {
            helper.PrependErrorSummary = true;
        }
    }
}
