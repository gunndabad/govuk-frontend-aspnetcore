using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// <see cref="ITagHelper"/> implementation targeting &lt;form&gt; elements that prepends a GDS error summary component to the form.
/// </summary>
[HtmlTargetElement("form")]
[HtmlTargetElement("*", Attributes = "prepend-error-summary")]
public class ContainerErrorSummaryTagHelper : TagHelper
{
    private const string DisableAutoFocusAttributeName = "disable-error-summary-auto-focus";
    private const string PrependErrorSummaryAttributeName = "prepend-error-summary";

    private readonly IComponentGenerator _componentGenerator;

    /// <summary>
    /// Creates a <see cref="ContainerErrorSummaryTagHelper"/>.
    /// </summary>
    public ContainerErrorSummaryTagHelper(IComponentGenerator componentGenerator)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        _componentGenerator = componentGenerator;
    }

    /// <summary>
    /// Whether to disable the behavior that focuses the error summary when the page loads.
    /// </summary>
    [HtmlAttributeName(DisableAutoFocusAttributeName)]
    public bool? DisableAutoFocus { get; set; }

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
    public override void Init(TagHelperContext context)
    {
        // N.B. We deliberately do not use SetScopedContextItem here; nested forms are not supported.
        // If we weren't able to add the FormErrorContext then there's a parent element that's already set it.
        context.Items.TryAdd(typeof(ContainerErrorContext), new ContainerErrorContext());
    }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await output.GetChildContentAsync();

        if (PrependErrorSummary != true)
        {
            return;
        }

        var containerErrorContext = (ContainerErrorContext)context.Items[typeof(ContainerErrorContext)];
        if (containerErrorContext.Errors.Count == 0)
        {
            return;
        }

        ViewContext!.ViewData.SetPageHasErrors(true);

        var errorItems = containerErrorContext
            .Errors.Select(i => new ErrorSummaryOptionsErrorItem() { Html = i.Html, Href = i.Href })
            .ToArray();

        var component = _componentGenerator.GenerateErrorSummary(
            new ErrorSummaryOptions()
            {
                TitleText = null,
                TitleHtml = null,
                DescriptionText = null,
                DescriptionHtml = null,
                ErrorList = errorItems,
                Classes = null,
                Attributes = null,
                DisableAutoFocus = DisableAutoFocus,
                TitleAttributes = null,
                DescriptionAttributes = null,
            }
        );

        output.PreContent.AppendHtml(component);
    }
}
