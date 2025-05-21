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
/// Generates a GDS input component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(
    LabelTagName,
    //LabelShortTagName,
    HintTagName,
    //HintShortTagName,
    ErrorMessageTagName,
    //ErrorMessageShortTagName,
    TextInputPrefixTagHelper.TagName,
    //TextInputPrefixTagHelper.ShortTagName,
    TextInputSuffixTagHelper.TagName/*,
    TextInputSuffixTagHelper.ShortTagName*/)]
[OutputElementHint(DefaultComponentGenerator.ComponentElementTypes.FormGroup)]
public class TextInputTagHelper : TagHelper
{
    internal const string ErrorMessageTagName = "govuk-input-error-message";
    //internal const string ErrorMessageShortTagName = ShortTagNames.ErrorMessage;
    internal const string HintTagName = "govuk-input-hint";
    //internal const string HintShortTagName = ShortTagNames.Hint;
    internal const string LabelTagName = "govuk-input-label";
    //internal const string LabelShortTagName = ShortTagNames.Label;
    internal const string TagName = "govuk-input";

    private const string AspForAttributeName = "asp-for";
    private const string AttributesPrefix = "input-";
    private const string AutoCompleteAttributeName = "autocomplete";
    private const string AutocapitalizeAttributeName = "autocapitalize";
    private const string DescribedByAttributeName = "described-by";
    private const string DisabledAttributeName = "disabled";
    private const string ForAttributeName = "for";
    private const string IdAttributeName = "id";
    private const string IgnoreModelStateErrorsAttributeName = "ignore-modelstate-errors";
    private const string InputModeAttributeName = "inputmode";
    private const string InputWrapperAttributesPrefix = "input-wrapper-";
    private const string LabelClassAttributeName = "label-class";
    private const string NameAttributeName = "name";
    private const string PatternAttributeName = "pattern";
    private const string SpellcheckAttributeName = "spellcheck";
    private const string TypeAttributeName = "type";
    private const string ValueAttributeName = "value";

    private readonly IComponentGenerator _componentGenerator;
    private readonly IModelHelper _modelHelper;
    private readonly HtmlEncoder _encoder;

    private string? _value;
    private bool _valueSpecified;

    /// <summary>
    /// Creates an <see cref="TextInputTagHelper"/>.
    /// </summary>
    public TextInputTagHelper(IComponentGenerator componentGenerator, HtmlEncoder encoder)
        : this(componentGenerator, new DefaultModelHelper(), encoder)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        ArgumentNullException.ThrowIfNull(encoder);
    }

    internal TextInputTagHelper(IComponentGenerator componentGenerator, IModelHelper modelHelper, HtmlEncoder encoder)
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
    /// An expression to be evaluated against the current model.
    /// </summary>
    [HtmlAttributeName(ForAttributeName)]
    public ModelExpression? For { get; set; }

    /// <summary>
    /// The <c>autocapitalize</c> attribute for the generated <c>input</c> element.
    /// </summary>
    [HtmlAttributeName(AutocapitalizeAttributeName)]
    public string? Autocapitalize { get; set; }

    /// <summary>
    /// The <c>autocomplete</c> attribute for the generated <c>input</c> element.
    /// </summary>
    [HtmlAttributeName(AutoCompleteAttributeName)]
    public string? AutoComplete { get; set; }

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
    /// Additional attributes to add to the element that wraps the <c>input</c> element.
    /// </summary>
    [HtmlAttributeName(DictionaryAttributePrefix = InputWrapperAttributesPrefix)]
    public IDictionary<string, string?> InputWrapperAttributes { get; set; } = new Dictionary<string, string?>();

    /// <summary>
    /// Additional classes for the generated <c>label</c> element.
    /// </summary>
    [HtmlAttributeName(LabelClassAttributeName)]
    public string? LabelClass { get; set; }

    /// <summary>
    /// The <c>name</c> attribute for the generated <c>input</c> element.
    /// </summary>
    /// <remarks>
    /// Required unless <see cref="FormGroupTagHelperBase.AspFor"/> is specified.
    /// </remarks>
    [HtmlAttributeName(NameAttributeName)]
    public string? Name { get; set; }

    /// <summary>
    /// The <c>inputmode</c> attribute for the generated <c>input</c> element.
    /// </summary>
    [HtmlAttributeName(InputModeAttributeName)]
    public string? InputMode { get; set; }

    /// <summary>
    /// The <c>pattern</c> attribute for the generated <c>input</c> element.
    /// </summary>
    [HtmlAttributeName(PatternAttributeName)]
    public string? Pattern { get; set; }

    /// <summary>
    /// The <c>spellcheck</c> attribute for the generated <c>input</c> element.
    /// </summary>
    [HtmlAttributeName(SpellcheckAttributeName)]
    public bool? Spellcheck { get; set; }

    /// <summary>
    /// The <c>type</c> attribute for the generated <c>input</c> element.
    /// </summary>
    /// <remarks>
    /// The default is <c>&quot;text&quot;</c>.
    /// </remarks>
    [HtmlAttributeName(TypeAttributeName)]
    public string? Type { get; set; }

    /// <summary>
    /// The <c>value</c> attribute for the generated <c>input</c> element.
    /// </summary>
    /// <remarks>
    /// If not specified and <see cref="FormGroupTagHelperBase.AspFor"/> is not <c>null</c> then the value
    /// for the specified model expression will be used.
    /// </remarks>
    [HtmlAttributeName(ValueAttributeName)]
    public string? Value
    {
        get => _value;
        set
        {
            _value = value;
            _valueSpecified = true;
        }
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
        var textInputContext = new TextInputContext();

        using (context.SetScopedContextItem(textInputContext))
        using (context.SetScopedContextItem(typeof(FormGroupContext3), textInputContext))
        {
            await output.GetChildContentAsync();
        }

        var name = ResolveName();
        var id = ResolveId(name);
        var value = ResolveValue();
        var labelOptions = textInputContext.GetLabelOptions(For, ViewContext!, _modelHelper, id, AspForAttributeName);
        var hintOptions = textInputContext.GetHintOptions(For, _modelHelper);
        var errorMessageOptions = textInputContext.GetErrorMessageOptions(For, ViewContext!, _modelHelper, IgnoreModelStateErrors);

        if (LabelClass is not null)
        {
            labelOptions.Classes = labelOptions.Classes.AppendCssClasses(_encoder, LabelClass);
        }

        var formGroupAttributes = new AttributeCollection(output.Attributes);
        formGroupAttributes.Remove("class", out var formGroupClasses);
        var formGroupOptions = new InputFormGroupOptions()
        {
            Attributes = formGroupAttributes,
            Classes = formGroupClasses
        };

        var attributes = new AttributeCollection(InputAttributes);
        attributes.Remove("class", out var classes);

        var inputWrapperAttributes = new AttributeCollection(InputWrapperAttributes);
        inputWrapperAttributes.Remove("classes", out var inputWrapperClasses);

        var component = await _componentGenerator.GenerateInputAsync(new InputOptions()
        {
            Id = id,
            Name = name,
            Type = Type,
            InputMode = InputMode,
            Value = value,
            Disabled = Disabled,
            DescribedBy = DescribedBy,
            Label = labelOptions,
            Hint = hintOptions,
            ErrorMessage = errorMessageOptions,
            Prefix = textInputContext.Prefix,
            Suffix = textInputContext.Suffix,
            FormGroup = formGroupOptions,
            Classes = classes,
            AutoComplete = AutoComplete,
            Pattern = Pattern,
            Spellcheck = Spellcheck,
            AutoCapitalize = Autocapitalize,
            InputWrapper = new InputOptionsInputWrapper()
            {
                Classes = inputWrapperClasses,
                Attributes = inputWrapperAttributes
            },
            Attributes = attributes
        });

        output.ApplyComponentHtml(component, HtmlEncoder.Default);

        if (errorMessageOptions is not null)
        {
            Debug.Assert(errorMessageOptions.Html is not null);
            var containerErrorContext = ViewContext!.HttpContext.GetContainerErrorContext();
            containerErrorContext.AddError(errorMessageOptions.Html, href: "#" + id);
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

    private string? ResolveValue()
    {
        if (_valueSpecified)
        {
            return _value;
        }

        return For is not null ? _modelHelper.GetModelValue(ViewContext!, For.ModelExplorer, For.Name) : null;
    }
}
