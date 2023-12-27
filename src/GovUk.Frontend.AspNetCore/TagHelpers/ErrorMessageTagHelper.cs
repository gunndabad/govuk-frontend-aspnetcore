using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS error message component.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(DefaultComponentGenerator.ErrorMessageElement)]
public class ErrorMessageTagHelper : TagHelper
{
    internal const string TagName = "govuk-error-message";

    private const string AspForAttributeName = "asp-for";
    private const string ForAttributeName = "for";
    private const string IdAttributeName = "id";
    private const string VisuallyHiddenTextAttributeName = "visually-hidden-text";

    private readonly IComponentGenerator _componentGenerator;
    private readonly IModelHelper _modelHelper;

    /// <summary>
    /// Creates a <see cref="ErrorMessageTagHelper"/>.
    /// </summary>
    public ErrorMessageTagHelper(IComponentGenerator componentGenerator) :
        this(componentGenerator, new DefaultModelHelper())
    {
    }

    internal ErrorMessageTagHelper(IComponentGenerator componentGenerator, IModelHelper modelHelper)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        ArgumentNullException.ThrowIfNull(modelHelper);
        _componentGenerator = componentGenerator;
        _modelHelper = modelHelper;
    }

    /// <summary>
    /// An expression to be evaluated against the current model.
    /// </summary>
    /// <remarks>
    /// If specified and this element has no child content the error message will be resolved from
    /// the errors on this expression's <see cref="ModelStateEntry"/>.
    /// </remarks>
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
    /// <remarks>
    /// If specified and this element has no child content the error message will be resolved from
    /// the errors on this expression's <see cref="ModelStateEntry"/>.
    /// </remarks>
    [HtmlAttributeName(ForAttributeName)]
    public ModelExpression? For { get; set; }

    /// <summary>
    /// The <c>id</c> attribute for the error message.
    /// </summary>
    [HtmlAttributeName(IdAttributeName)]
    public string? Id { get; set; }

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
                resolvedContent = new HtmlString(HtmlEncoder.Default.Encode(validationMessage));
            }
        }

        if (resolvedContent != null)
        {
            var attributes = output.Attributes.ToEncodedAttributeDictionary()
                .Remove("class", out var classes);

            var component = _componentGenerator.GenerateErrorMessage(new ErrorMessageOptions()
            {
                Text = null,
                Html = resolvedContent.ToHtmlString(),
                Id = Id,
                VisuallyHiddenText = VisuallyHiddenText,
                Classes = classes,
                Attributes = attributes
            });

            output.WriteComponent(component);
        }
        else
        {
            output.SuppressOutput();
        }
    }
}
