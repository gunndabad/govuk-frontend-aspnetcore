using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using AttributeCollection = GovUk.Frontend.AspNetCore.Components.AttributeCollection;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS File upload component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(LabelTagName, HintTagName, ErrorMessageTagName)]
public class FileUploadTagHelper : TagHelper
{
    internal const string ErrorMessageTagName = "govuk-file-upload-error-message";
    internal const string HintTagName = "govuk-file-upload-hint";
    internal const string LabelTagName = "govuk-file-upload-label";
    internal const string TagName = "govuk-file-upload";

    private const string AspForAttributeName = "asp-for";
    private const string AttributesPrefix = "input-";
    private const string DescribedByAttributeName = "described-by";
    private const string DisabledAttributeName = "disabled";
    private const string ForAttributeName = "for";
    private const string IdAttributeName = "id";
    private const string IgnoreModelStateErrorsAttributeName = "ignore-modelstate-errors";
    private const string JavaScriptEnhancementsAttributeName = "javascript-enhancements";
    private const string LabelClassAttributeName = "label-class";
    private const string MultipleAttributeName = "multiple";
    private const string NameAttributeName = "name";

    private readonly IComponentGenerator _componentGenerator;
    private readonly IModelHelper _modelHelper;
    private readonly HtmlEncoder _encoder;

    /// <summary>
    /// Creates an <see cref="TextInputTagHelper"/>.
    /// </summary>
    public FileUploadTagHelper(IComponentGenerator componentGenerator, HtmlEncoder encoder)
        : this(componentGenerator, modelHelper: new DefaultModelHelper(), encoder)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        ArgumentNullException.ThrowIfNull(encoder);
    }

    internal FileUploadTagHelper(IComponentGenerator componentGenerator, IModelHelper modelHelper, HtmlEncoder encoder)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        ArgumentNullException.ThrowIfNull(modelHelper);
        ArgumentNullException.ThrowIfNull(encoder);
        _componentGenerator = componentGenerator;
        _modelHelper = modelHelper;
        _encoder = encoder;
    }

    /// <summary>
    /// An expression to be evaluated against the current model.
    /// </summary>
    [HtmlAttributeName(AspForAttributeName)]
    [Obsolete("Use the 'for' attribute instead.", DiagnosticId = DiagnosticIds.UseForAttributeInstead)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ModelExpression? AspFor
    {
        get => For;
        set => For = value;
    }

    /// <summary>
    /// One or more element IDs to add to the <c>aria-describedby</c> attribute of the generated <c>input</c> element.
    /// </summary>
    [HtmlAttributeName(DescribedByAttributeName)]
    public string? DescribedBy { get; set; }

    /// <summary>
    /// Whether the <c>disabled</c> attribute should be added to the generated <c>input</c> element.
    /// </summary>
    [HtmlAttributeName(DisabledAttributeName)]
    public bool? Disabled { get; set; }

    /// <summary>
    /// An expression to be evaluated against the current model.
    /// </summary>
    [HtmlAttributeName(ForAttributeName)]
    public ModelExpression? For { get; set; }

    /// <summary>
    /// The <c>id</c> attribute for the generated <c>input</c> element.
    /// </summary>
    /// <remarks>
    /// If not specified then a value is generated from the <c>name</c> attribute.
    /// </remarks>
    [HtmlAttributeName(IdAttributeName)]
    public string? Id { get; set; }

    /// <summary>
    /// Whether the <see cref="ModelStateEntry.Errors"/> for the <see cref="AspFor"/> expression should be used
    /// to deduce an error message.
    /// </summary>
    /// <remarks>
    /// <para>When there are multiple errors in the <see cref="ModelErrorCollection"/> the first is used.</para>
    /// </remarks>
    [HtmlAttributeName(IgnoreModelStateErrorsAttributeName)]
    public bool? IgnoreModelStateErrors { get; set; }

    /// <summary>
    /// Additional attributes to add to the generated <c>input</c> element.
    /// </summary>
    [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
    public IDictionary<string, string?> InputAttributes { get; set; } = new Dictionary<string, string?>();

    /// <summary>
    /// Whether to enable JavaScript enhancements for the component.
    /// </summary>
    /// <remarks>
    /// The default is set for the application in <see cref="GovUkFrontendAspNetCoreOptions.DefaultFileUploadJavaScriptEnhancements"/>.
    /// </remarks>
    [HtmlAttributeName(JavaScriptEnhancementsAttributeName)]
    public bool? JavaScriptEnhancements { get; set; }

    /// <summary>
    /// Additional classes for the generated <c>label</c> element.
    /// </summary>
    [HtmlAttributeName(LabelClassAttributeName)]
    public string? LabelClass { get; set; }

    /// <summary>
    /// The <c>multiple</c> attribute for the generated <c>input</c> element.
    /// </summary>
    [HtmlAttributeName(MultipleAttributeName)]
    public bool? Multiple { get; set; }

    /// <summary>
    /// The <c>name</c> attribute for the generated <c>input</c> element.
    /// </summary>
    /// <remarks>
    /// Required unless <see cref="FormGroupTagHelperBase.AspFor"/> is specified.
    /// </remarks>
    [HtmlAttributeName(NameAttributeName)]
    public string? Name { get; set; }

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
        var fileUploadContext = new FileUploadContext();

        using (context.SetScopedContextItem(fileUploadContext))
        using (context.SetScopedContextItem(typeof(FormGroupContext3), fileUploadContext))
        {
            await output.GetChildContentAsync();
        }

        var name = ResolveName();
        var id = ResolveId(name);
        var labelOptions = fileUploadContext.GetLabelOptions(For, ViewContext!, _modelHelper, id, AspForAttributeName);
        var hintOptions = fileUploadContext.GetHintOptions(For, _modelHelper);
        var errorMessageOptions = fileUploadContext.GetErrorMessageOptions(For, ViewContext!, _modelHelper, IgnoreModelStateErrors);

        if (LabelClass is not null)
        {
            labelOptions.Classes = labelOptions.Classes?.Concatenate(_encoder, " ", LabelClass);
        }

        var formGroupAttributes = new AttributeCollection(output.Attributes);
        formGroupAttributes.Remove("class", out var formGroupClasses);
        var formGroupOptions = new FileUploadOptionsFormGroup()
        {
            Attributes = formGroupAttributes,
            Classes = formGroupClasses
        };

        var attributes = new AttributeCollection(InputAttributes);
        attributes.Remove("class", out var classes);

        var component = await _componentGenerator.GenerateFileUploadAsync(new FileUploadOptions
        {
            Id = id,
            Name = name,
            Value = null,
            Disabled = Disabled,
            Multiple = Multiple,
            DescribedBy = DescribedBy,
            Label = labelOptions,
            Hint = hintOptions,
            ErrorMessage = errorMessageOptions,
            FormGroup = formGroupOptions,
            JavaScript = JavaScriptEnhancements,
            ChooseFilesButtonText = null,
            DropInstructionText = null,
            MultipleFilesChosenText = null,
            NoFileChosenText = null,
            EnteredDropZoneText = null,
            LeftDropZoneText = null,
            Classes = classes,
            Attributes = attributes
        });

        output.ApplyComponentHtml(component, HtmlEncoder.Default);

        if (errorMessageOptions is not null)
        {
            Debug.Assert(errorMessageOptions.Html is not null);
            var containerErrorContext = ViewContext!.HttpContext.GetContainerErrorContext();
            containerErrorContext.AddError(errorMessageOptions.Html!, href: "#" + id);
        }
    }

    private string ResolveId(string name) =>
        Id ?? TagBuilder.CreateSanitizedId(name, Constants.IdAttributeDotReplacement);

    private string ResolveName()
    {
        if (Name is null && For is null)
        {
            throw ExceptionHelper.AtLeastOneOfAttributesMustBeProvided(
                NameAttributeName,
                AspForAttributeName);
        }

        return Name ?? _modelHelper.GetFullHtmlFieldName(ViewContext!, For!.Name);
    }
}
