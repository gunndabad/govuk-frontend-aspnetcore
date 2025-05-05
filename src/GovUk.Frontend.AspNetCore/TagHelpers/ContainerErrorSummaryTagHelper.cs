using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// <see cref="ITagHelper"/> implementation targeting &lt;form&gt; elements and elements with a 'prepend-error-summary' attribute.
/// </summary>
[HtmlTargetElement("form")]
[HtmlTargetElement("*", Attributes = PrependErrorSummaryAttributeName)]
public class ContainerErrorSummaryTagHelper : TagHelper
{
    private const string PrependErrorSummaryAttributeName = "prepend-error-summary";

    private readonly IComponentGenerator _componentGenerator;
    private readonly IOptions<GovUkFrontendAspNetCoreOptions> _optionsAccessor;

    /// <summary>
    /// Creates a <see cref="ContainerErrorSummaryTagHelper"/>.
    /// </summary>
    public ContainerErrorSummaryTagHelper(IComponentGenerator componentGenerator, IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        ArgumentNullException.ThrowIfNull(optionsAccessor);
        _componentGenerator = componentGenerator;
        _optionsAccessor = optionsAccessor;
    }

    /// <summary>
    /// Whether to prepend an error summary component to this form.
    /// </summary>
    /// <remarks>
    /// The default is set for the application in <see cref="GovUkFrontendAspNetCoreOptions.PrependErrorSummaryToForms"/>.
    /// </remarks>
    [HtmlAttributeName(PrependErrorSummaryAttributeName)]
    public bool? PrependErrorSummary { get; set; }

    /// <summary>
    /// Gets the <see cref="ViewContext"/> of the executing view.
    /// </summary>
    [HtmlAttributeNotBound]
    [ViewContext]
    [DisallowNull]
    public ViewContext? ViewContext { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await output.GetChildContentAsync();

        var prependErrorSummary = PrependErrorSummary ??
            (output.TagName.Equals("form", StringComparison.OrdinalIgnoreCase) && _optionsAccessor.Value.PrependErrorSummaryToForms);

        if (!prependErrorSummary)
        {
            return;
        }

        var containerErrorContext = ViewContext!.HttpContext.GetContainerErrorContext();
        if (containerErrorContext.Errors.Count == 0)
        {
            return;
        }

        containerErrorContext.ErrorSummaryHasBeenRendered = true;

        var errorSummary = await _componentGenerator.GenerateErrorSummary(new ErrorSummaryOptions
        {
            TitleText = null,
            TitleHtml = DefaultComponentGenerator.DefaultErrorSummaryTitleHtml,
            DescriptionText = null,
            DescriptionHtml = null,
            ErrorList = containerErrorContext.GetErrorList(),
            Classes = null,
            Attributes = null,
            DisableAutoFocus = null,
            TitleAttributes = null,
            DescriptionAttributes = null
        });

        output.PreContent.AppendHtml(errorSummary);
    }
}
