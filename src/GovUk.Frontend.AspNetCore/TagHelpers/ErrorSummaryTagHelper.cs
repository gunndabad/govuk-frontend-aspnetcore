using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS error summary component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(ErrorSummaryTitleTagHelper.TagName, ErrorSummaryDescriptionTagHelper.TagName, "govuk-error-summary-item")]
[OutputElementHint(ComponentGenerator.ErrorSummaryElement)]
public class ErrorSummaryTagHelper : TagHelper
{
    internal const string TagName = "govuk-error-summary";

    private const string DisableAutoFocusAttributeName = "disable-auto-focus";

    private readonly IGovUkHtmlGenerator _htmlGenerator;

    /// <summary>
    /// Creates a new <see cref="ErrorSummaryTagHelper"/>.
    /// </summary>
    public ErrorSummaryTagHelper()
        : this(null)
    {
    }

    internal ErrorSummaryTagHelper(IGovUkHtmlGenerator? htmlGenerator = null)
    {
        _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
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

        var tagBuilder = _htmlGenerator.GenerateErrorSummary(
            DisableAutoFocus,
            errorSummaryContext.Title?.Content ?? new HtmlString(HtmlEncoder.Default.Encode(ComponentGenerator.ErrorSummaryDefaultTitle)),
            errorSummaryContext.Title?.Attributes,
            errorSummaryContext.Description?.Content,
            errorSummaryContext.Description?.Attributes,
            output.Attributes.ToAttributeDictionary(),
            errorSummaryContext.Items);

        output.TagName = tagBuilder.TagName;
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.Clear();
        output.MergeAttributes(tagBuilder);
        output.Content.SetHtmlContent(tagBuilder.InnerHtml);
    }
}
