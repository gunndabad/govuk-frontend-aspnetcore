using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using static GovUk.Frontend.AspNetCore.GenerateErrorSummariesOptions;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// <see cref="ITagHelper"/> implementation targeting &lt;form&gt; elements and elements with a 'prepend-error-summary' attribute.
/// </summary>
[HtmlTargetElement("form")]
[HtmlTargetElement("main")]
public class GeneratedErrorSummaryTagHelper : TagHelper
{
    private const string PrependErrorSummaryAttributeName = "prepend-error-summary";

    private readonly IComponentGenerator _componentGenerator;
    private readonly IOptions<GovUkFrontendAspNetCoreOptions> _optionsAccessor;

    /// <summary>
    /// Creates a <see cref="GeneratedErrorSummaryTagHelper"/>.
    /// </summary>
    public GeneratedErrorSummaryTagHelper(IComponentGenerator componentGenerator, IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
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
    /// Whether to prepend an error summary component to this form.
    /// </summary>
    /// <remarks>
    /// The default is set for the application in <see cref="GovUkFrontendAspNetCoreOptions.PrependErrorSummaryToForms"/>.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use the 'prepend-error-summary' attribute instead.", DiagnosticId = DiagnosticIds.UsePrependErrorSummaryAttributeInstead)]
    [HtmlAttributeName("gfa-" + PrependErrorSummaryAttributeName)]
    public bool? GfaPrependErrorSummary
    {
        get => PrependErrorSummary;
        set => PrependErrorSummary = value;
    }

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

        var generateErrorSummariesOptions = _optionsAccessor.Value.GenerateErrorSummaries;

        var prependErrorSummary = PrependErrorSummary ??
            (output.TagName.Equals("form", StringComparison.OrdinalIgnoreCase) && generateErrorSummariesOptions.HasFlag(PrependToFormElements)) ||
            (output.TagName.Equals("main", StringComparison.OrdinalIgnoreCase) && generateErrorSummariesOptions.HasFlag(PrependToMainElement));

        if (!prependErrorSummary)
        {
            return;
        }

        var containerErrorContext = ViewContext!.HttpContext.GetContainerErrorContext();
        if (containerErrorContext.Errors.Count == 0)
        {
            return;
        }

        var disableAutoFocus = generateErrorSummariesOptions.HasFlag(DisableAutoFocus);

        var errorSummary = await _componentGenerator.GenerateErrorSummaryAsync(new ErrorSummaryOptions()
        {
            TitleText = null,
            TitleHtml = DefaultComponentGenerator.DefaultErrorSummaryTitleHtml,
            DescriptionText = null,
            DescriptionHtml = null,
            ErrorList = containerErrorContext.GetErrorList(),
            Classes = null,
            Attributes = null,
            DisableAutoFocus = disableAutoFocus,
            TitleAttributes = null,
            DescriptionAttributes = null
        });

        output.PreContent.AppendHtml(errorSummary);

        containerErrorContext.ErrorSummaryHasBeenRendered = true;
    }
}
