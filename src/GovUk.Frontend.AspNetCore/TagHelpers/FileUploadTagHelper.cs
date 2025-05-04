using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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

    private readonly IComponentGenerator2 _componentGenerator;
    private readonly IModelHelper _modelHelper;

    /// <summary>
    /// Creates an <see cref="TextInputTagHelper"/>.
    /// </summary>
    public FileUploadTagHelper()
        : this(new FluidComponentGenerator(), modelHelper: new DefaultModelHelper())
    {
    }

    internal FileUploadTagHelper(IComponentGenerator2 componentGenerator)
        : this(componentGenerator, modelHelper: new DefaultModelHelper())
    {
    }

    internal FileUploadTagHelper(IComponentGenerator2 componentGenerator, IModelHelper modelHelper)
    {
        _componentGenerator = componentGenerator;
        _modelHelper = modelHelper;
    }

    /// <summary>
    /// An expression to be evaluated against the current model.
    /// </summary>
    [HtmlAttributeName(AspForAttributeName)]
    //[Obsolete("Use the 'for' attribute instead.")]
    public ModelExpression? AspFor { get; set; }
    /*{
        get => For;
        set => For = value;
    }*/

    /// <summary>
    /// One or more element IDs to add to the <c>aria-describedby</c> attribute of the generated <c>input</c> element.
    /// </summary>
    [HtmlAttributeName(DescribedByAttributeName)]
    public string? DescribedBy { get; set; }

    /// <summary>
    /// Whether the <c>disabled</c> attribute should be added to the generated <c>input</c> element.
    /// </summary>
    [HtmlAttributeName(DisabledAttributeName)]
    public bool Disabled { get; set; } = ComponentGenerator.FileUploadDefaultDisabled;

    ///// <summary>
    ///// An expression to be evaluated against the current model.
    ///// </summary>
    //[HtmlAttributeName(ForAttributeName)]
    //public ModelExpression? For { get; set; }

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
        var labelOptions = fileUploadContext.GetLabelOptions(AspFor, ViewContext!, _modelHelper, id, AspForAttributeName);
        var hintOptions = fileUploadContext.GetHintOptions(AspFor, _modelHelper);
        var errorMessageOptions = fileUploadContext.GetErrorMessageOptions(AspFor, ViewContext!, _modelHelper, IgnoreModelStateErrors);

        if (LabelClass is not null)
        {
            labelOptions.Classes = (labelOptions.Classes + " " + LabelClass).TrimStart();
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

        var component = _componentGenerator.GenerateFileUpload(new FileUploadOptions
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

        output.ApplyComponentHtml(component);

        if (errorMessageOptions is not null && context.TryGetContextItem<ContainerErrorContext>(out var containerErrorContext))
        {
            Debug.Assert(errorMessageOptions.Html is not null);
            containerErrorContext.AddError(new HtmlString(errorMessageOptions.Html!), href: new HtmlString("#" + id));
        }
    }

    private string ResolveId(string name)
    {
        if (Id is not null)
        {
            return Id;
        }

        return TagBuilder.CreateSanitizedId(name, Constants.IdAttributeDotReplacement);
    }

    private string ResolveName()
    {
        if (Name is null && AspFor is null)
        {
            throw ExceptionHelper.AtLeastOneOfAttributesMustBeProvided(
                NameAttributeName,
                AspForAttributeName);
        }

        return Name ?? _modelHelper.GetFullHtmlFieldName(ViewContext!, AspFor!.Name);
    }
}
