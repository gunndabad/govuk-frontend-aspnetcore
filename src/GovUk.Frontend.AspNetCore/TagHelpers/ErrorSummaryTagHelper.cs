using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS error summary component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(ErrorSummaryTitleTagHelper.TagName, ErrorSummaryDescriptionTagHelper.TagName, "govuk-error-summary-item")]
[OutputElementHint(DefaultComponentGenerator.ErrorSummaryElement)]
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

        if (!errorSummaryContext.HasContent)
        {
            output.SuppressOutput();
            return;
        }

        ViewContext!.ViewData.SetPageHasErrors(true);

        var title = errorSummaryContext.GetTitle();
        var description = errorSummaryContext.GetDescription();

        var attributes = output.Attributes.ToEncodedAttributeDictionary()
            .Remove("class", out var classes);

        var component = _componentGenerator.GenerateErrorSummary(new ErrorSummaryOptions()
        {
            TitleText = null,
            TitleHtml = title?.Html,
            DescriptionText = null,
            DescriptionHtml = description?.Html,
            ErrorList = errorSummaryContext.Items,
            Classes = classes,
            Attributes = attributes,
            DisableAutoFocus = DisableAutoFocus,
            TitleAttributes = title?.Attributes,
            DescriptionAttributes = description?.Attributes
        });

        output.WriteComponent(component);
    }
}
