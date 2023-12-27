using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS input component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(LabelTagName, HintTagName, ErrorMessageTagName, TextInputPrefixTagHelper.TagName, TextInputSuffixTagHelper.TagName)]
[OutputElementHint(DefaultComponentGenerator.FormGroupElement)]
public class TextInputTagHelper : TagHelper
{
    internal const string ErrorMessageTagName = "govuk-input-error-message";
    internal const string HintTagName = "govuk-input-hint";
    internal const string LabelTagName = "govuk-input-label";
    internal const string TagName = "govuk-input";

    private const string AspForAttributeName = "asp-for";
    private const string AttributesPrefix = "input-";
    private const string AutocompleteAttributeName = "autocomplete";
    private const string DescribedByAttributeName = "described-by";
    private const string DisabledAttributeName = "disabled";
    private const string ForAttributeName = "for";
    private const string IdAttributeName = "id";
    private const string IgnoreModelStateErrorsAttributeName = "ignore-modelstate-errors";
    private const string InputModeAttributeName = "inputmode";
    private const string LabelClassAttributeName = "label-class";
    private const string NameAttributeName = "name";
    private const string PatternAttributeName = "pattern";
    private const string SpellcheckAttributeName = "spellcheck";
    private const string TypeAttributeName = "type";
    private const string ValueAttributeName = "value";

    private readonly IComponentGenerator _componentGenerator;
    private readonly IModelHelper _modelHelper;

    private string? _value;
    private bool _valueSpecified;

    /// <summary>
    /// Creates an <see cref="TextInputTagHelper"/>.
    /// </summary>
    public TextInputTagHelper(IComponentGenerator componentGenerator)
        : this(componentGenerator, modelHelper: new DefaultModelHelper())
    {
    }

    internal TextInputTagHelper(IComponentGenerator componentGenerator, IModelHelper modelHelper)
    {
        _componentGenerator = componentGenerator;
        _modelHelper = modelHelper;
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
    /// The <c>autocomplete</c> attribute for the generated <c>input</c> element.
    /// </summary>
    [HtmlAttributeName(AutocompleteAttributeName)]
    public string? Autocomplete { get; set; }

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
    /// <para>The default is <c>false</c>.</para>
    /// </remarks>
    [HtmlAttributeName(IgnoreModelStateErrorsAttributeName)]
    public bool? IgnoreModelStateErrors { get; set; }

    /// <summary>
    /// Additional attributes to add to the generated <c>input</c> element.
    /// </summary>
    [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
    public IDictionary<string, string?> InputAttributes { get; set; } = new Dictionary<string, string?>();

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
        using (context.SetScopedContextItem(typeof(FormGroupContext2), textInputContext))
        {
            await output.GetChildContentAsync();
        }

        var name = ResolveName();
        var id = ResolveId(name);
        var value = ResolveValue();
        var labelOptions = textInputContext.GetLabelOptions(For, ViewContext!, _modelHelper, id, AspForAttributeName);
        var hintOptions = textInputContext.GetHintOptions(For, _modelHelper);
        var errorMessageOptions = textInputContext.GetErrorMessageOptions(For, ViewContext!, _modelHelper);
        var prefixOptions = textInputContext.GetPrefixOptions();
        var suffixOptions = textInputContext.GetSuffixOptions();

        if (LabelClass is not null)
        {
            labelOptions.Classes += " " + LabelClass;
        }

        var formGroupAttributes = output.Attributes.ToEncodedAttributeDictionary()
            .Remove("class", out var formGroupClasses);
        var formGroupOptions = new FormGroupOptions()
        {
            Attributes = formGroupAttributes,
            Classes = formGroupClasses
        };

        var attributes = InputAttributes!.ToImmutableDictionary()
            .Remove("class", out var classes);

        if (errorMessageOptions is not null)
        {
            classes ??= "";
            classes += " govuk-input--error";

            formGroupOptions.Classes += " govuk-form-group--error";
        }

        var component = _componentGenerator.GenerateTextInput(new TextInputOptions()
        {
            Id = id,
            Name = name,
            Type = Type,
            Inputmode = InputMode,
            Value = value,
            Disabled = Disabled,
            DescribedBy = DescribedBy,
            Label = labelOptions,
            Hint = hintOptions,
            ErrorMessage = errorMessageOptions,
            Prefix = prefixOptions,
            Suffix = suffixOptions,
            FormGroup = formGroupOptions,
            Classes = classes,
            Autocomplete = Autocomplete,
            Pattern = Pattern,
            Spellcheck = Spellcheck,
            Attributes = attributes
        });

        output.WriteComponent(component);

        if (errorMessageOptions is not null && context.TryGetContextItem<FormErrorContext>(out var formErrorContext))
        {
            formErrorContext.AddError(new HtmlString(errorMessageOptions.Html), href: "#" + id);
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

        return For != null ? _modelHelper.GetModelValue(ViewContext!, For.ModelExplorer, For.Name) : null;
    }
}
