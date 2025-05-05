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
/// <see cref="ITagHelper"/> implementation targeting &lt;form&gt; elements that prepends a GDS error summary component to the form.
/// </summary>
[HtmlTargetElement("form")]
public class FormErrorSummaryTagHelper : TagHelper
{
    private const string PrependErrorSummaryAttributeName = "prepend-error-summary";

    private readonly IComponentGenerator _componentGenerator;
    private readonly IOptions<GovUkFrontendAspNetCoreOptions> _optionsAccessor;

    /// <summary>
    /// Creates a <see cref="FormErrorSummaryTagHelper"/>.
    /// </summary>
    public FormErrorSummaryTagHelper(IComponentGenerator componentGenerator, IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
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

        var prependErrorSummary = PrependErrorSummary ?? _optionsAccessor.Value.PrependErrorSummaryToForms;
        if (!prependErrorSummary)
        {
            return;
        }

        var containerErrorContext = ViewContext!.HttpContext.GetContainerErrorContext();
        if (containerErrorContext.Errors.Count == 0)
        {
            return;
        }

        ViewContext!.ViewData.SetPageHasErrors(true);

        var errorSummary = await _componentGenerator.GenerateErrorSummary(new ErrorSummaryOptions
        {
            TitleText = null,
            TitleHtml = DefaultComponentGenerator.DefaultErrorSummaryTitleHtml,
            DescriptionText = null,
            DescriptionHtml = null,
            ErrorList = containerErrorContext.Errors
                .Select(i => new ErrorSummaryOptionsErrorItem
                {
                    Href = i.Href,
                    Text = null,
                    Html = i.Html,
                    Attributes = null,
                    ItemAttributes = null
                })
                .ToList(),
            Classes = null,
            Attributes = null,
            DisableAutoFocus = null,
            TitleAttributes = null,
            DescriptionAttributes = null
        });

        output.PreContent.AppendHtml(errorSummary);
    }
}
