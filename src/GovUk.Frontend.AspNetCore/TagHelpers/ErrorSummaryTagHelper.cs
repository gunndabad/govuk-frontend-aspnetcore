using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS error summary component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(ErrorSummaryTitleTagHelper.TagName, ErrorSummaryDescriptionTagHelper.TagName, "govuk-error-summary-item")]
[OutputElementHint(DefaultComponentGenerator.ComponentElementTypes.ErrorSummary)]
public class ErrorSummaryTagHelper : TagHelper
{
    internal const string TagName = "govuk-error-summary";

    private const string DisableAutoFocusAttributeName = "disable-auto-focus";

    private readonly IComponentGenerator _componentGenerator;

    /// <summary>
    /// Creates a new <see cref="ErrorSummaryTagHelper"/>.
    /// </summary>
    public ErrorSummaryTagHelper(IComponentGenerator componentGenerator)
    {
        _componentGenerator = componentGenerator;
        ArgumentNullException.ThrowIfNull(componentGenerator);
    }

    /// <summary>
    /// Whether to disable the behavior that focuses the error summary when the page loads.
    /// </summary>
    [HtmlAttributeName(DisableAutoFocusAttributeName)]
    public bool? DisableAutoFocus { get; set; }

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
        var errorSummaryContext = new ErrorSummaryContext();

        using (context.SetScopedContextItem(typeof(ErrorSummaryContext), errorSummaryContext))
        {
            await output.GetChildContentAsync();
        }

        if (errorSummaryContext.Title == null &&
            errorSummaryContext.Description == null &&
            errorSummaryContext.Items.Count == 0)
        {
            output.SuppressOutput();
            return;
        }

        ViewContext!.ViewData.SetPageHasErrors(true);

        var attributes = new AttributeCollection(output.Attributes);
        attributes.Remove("class", out var classes);

        var component = await _componentGenerator.GenerateErrorSummary(new ErrorSummaryOptions
        {
            TitleText = null,
            TitleHtml = errorSummaryContext.Title?.Html ?? DefaultComponentGenerator.DefaultErrorSummaryTitleHtml,
            DescriptionText = null,
            DescriptionHtml = errorSummaryContext.Description?.Html,
            ErrorList = errorSummaryContext.Items
                .Select(i => new ErrorSummaryOptionsErrorItem
                {
                    Href = i.Href,
                    Text = null,
                    Html = i.Html,
                    Attributes = i.Attributes,
                    ItemAttributes = i.ItemAttributes
                })
                .ToList(),
            Classes = classes,
            Attributes = attributes,
            DisableAutoFocus = DisableAutoFocus,
            TitleAttributes = errorSummaryContext?.Title?.Attributes,
            DescriptionAttributes = errorSummaryContext?.Description?.Attributes
        });

        output.ApplyComponentHtml(component);
    }
}
