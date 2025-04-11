using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS error message component.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(ComponentGenerator.ErrorMessageElement)]
public class ErrorMessageTagHelper : TagHelper
{
    internal const string TagName = "govuk-error-message";

    private const string AspForAttributeName = "asp-for";
    private const string ForAttributeName = "for";
    private const string VisuallyHiddenTextAttributeName = "visually-hidden-text";

    private readonly IGovUkHtmlGenerator _htmlGenerator;
    private readonly IModelHelper _modelHelper;

    /// <summary>
    /// Creates a <see cref="ErrorMessageTagHelper"/>.
    /// </summary>
    public ErrorMessageTagHelper()
        : this(htmlGenerator: null, modelHelper: null)
    {
    }

    internal ErrorMessageTagHelper(
        IGovUkHtmlGenerator? htmlGenerator,
        IModelHelper? modelHelper)
    {
        _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        _modelHelper = modelHelper ?? new DefaultModelHelper();
    }

    /// <summary>
    /// An expression to be evaluated against the current model.
    /// </summary>
    [HtmlAttributeName(AspForAttributeName)]
    [Obsolete("Use the 'for' attribute instead.")]
    public ModelExpression? AspFor
    {
        get => For;
        set => For = value;
    }

    /// <summary>
    /// An expression to be evaluated against the current model.
    /// </summary>
    [HtmlAttributeName(ForAttributeName)]
    public ModelExpression? For { get; set; }

    /// <summary>
    /// The visually hidden prefix used before the error message.
    /// </summary>
    /// <remarks>
    /// The default is <c>&quot;Error&quot;</c>.
    /// </remarks>
    [HtmlAttributeName(VisuallyHiddenTextAttributeName)]
    public string? VisuallyHiddenText { get; set; } = ComponentGenerator.ErrorMessageDefaultVisuallyHiddenText;

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
        var childContent = output.TagMode == TagMode.StartTagAndEndTag ?
            await output.GetChildContentAsync() :
            null;

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        if (childContent == null && For == null)
        {
            throw new InvalidOperationException(
                $"Cannot determine content. Element must contain content if the '{AspForAttributeName}' attribute is not specified.");
        }

        IHtmlContent? resolvedContent = childContent;
        if (resolvedContent == null && For != null)
        {
            var validationMessage = _modelHelper.GetValidationMessage(
                ViewContext!,
                For.ModelExplorer,
                For.Name);

            if (validationMessage != null)
            {
                resolvedContent = validationMessage.EncodeHtml();
            }
        }

        if (resolvedContent != null)
        {
            var tagBuilder = _htmlGenerator.GenerateErrorMessage(
                VisuallyHiddenText,
                resolvedContent,
                output.Attributes.ToAttributeDictionary());

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
        else
        {
            output.SuppressOutput();
        }
    }
}
