using System;
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
/// Generates a GDS character count component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(LabelTagName, HintTagName, ErrorMessageTagName, CharacterCountValueTagHelper.TagName)]
[OutputElementHint(DefaultComponentGenerator.CharacterCountElement)]
public class CharacterCountTagHelper : TagHelper
{
    internal const string ErrorMessageTagName = "govuk-character-count-error-message";
    internal const string HintTagName = "govuk-character-count-hint";
    internal const string LabelTagName = "govuk-character-count-label";
    internal const string TagName = "govuk-character-count";

    private const string AspForAttributeName = "asp-for";
    private const string AutocompleteAttributeName = "autocomplete";
    private const string CountMessageAttributesPrefix = "count-message-";
    private const string DisabledAttributeName = "disabled";
    private const string ForAttributeName = "for";
    private const string FormGroupAttributesPrefix = "form-group-";
    private const string IdAttributeName = "id";
    private const string IgnoreModelStateErrorsAttributeName = "ignore-modelstate-errors";
    private const string LabelClassAttributeName = "label-class";
    private const string MaxLengthAttributeName = "max-length";
    private const string MaxWordsLengthAttributeName = "max-words";
    private const string NameAttributeName = "name";
    private const string RowsAttributeName = "rows";
    private const string SpellcheckAttributeName = "spellcheck";
    private const string TextareaAttributesPrefix = "textarea-";
    private const string ThresholdAttributeName = "threshold";

    private readonly IComponentGenerator _componentGenerator;
    private readonly IModelHelper _modelHelper;

    private decimal? _threshold;
    private int? _maxLength;
    private int? _maxWords;

    /// <summary>
    /// Creates an <see cref="TextInputTagHelper"/>.
    /// </summary>
    public CharacterCountTagHelper(IComponentGenerator componentGenerator)
        : this(componentGenerator, modelHelper: new DefaultModelHelper())
    {
    }

    internal CharacterCountTagHelper(IComponentGenerator componentGenerator, IModelHelper modelHelper)
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

    ///// <summary>
    ///// An expression to be evaluated against the current model.
    ///// </summary>
    //[HtmlAttributeName(ForAttributeName)]
    //public ModelExpression? For { get; set; }

    /// <summary>
    /// The <c>autocomplete</c> attribute for the generated <c>textarea</c> element.
    /// </summary>
    [HtmlAttributeName(AutocompleteAttributeName)]
    public string? Autocomplete { get; set; }

    /// <summary>
    /// Additional attributes to add to the generated count message hint element.
    /// </summary>
    [HtmlAttributeName(DictionaryAttributePrefix = CountMessageAttributesPrefix)]
    public IDictionary<string, string?> CountMessageAttributes { get; set; } = new Dictionary<string, string?>();

    /// <summary>
    /// Whether the <c>disabled</c> attribute should be added to the generated <c>textarea</c> element.
    /// </summary>
    [HtmlAttributeName(DisabledAttributeName)]
    public bool Disabled { get; set; } = ComponentGenerator.TextAreaDefaultDisabled;

    /// <summary>
    /// Additional attributes to add to the generated form-group wrapper element.
    /// </summary>
    [HtmlAttributeName(DictionaryAttributePrefix = FormGroupAttributesPrefix)]
    public IDictionary<string, string?> FormGroupAttributes { get; set; } = new Dictionary<string, string?>();

    /// <summary>
    /// The <c>id</c> attribute for the generated <c>textarea</c> element.
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
    /// Additional classes for the generated <c>label</c> element.
    /// </summary>
    [HtmlAttributeName(LabelClassAttributeName)]
    public string? LabelClass { get; set; }

    /// <summary>
    /// The maximum number of characters the generated <c>textarea</c> may contain.
    /// </summary>
    /// <remarks>
    /// Required unless <see cref="MaxWords"/> is specified.
    /// </remarks>
    [HtmlAttributeName(MaxLengthAttributeName)]
    public int? MaxLength
    {
        get => _maxLength;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    $"{nameof(MaxLength)} must be greater than 0.");
            }

            _maxLength = value;
        }
    }

    /// <summary>
    /// The maximum number of words the generated <c>textarea</c> may contain.
    /// </summary>
    /// <remarks>
    /// Required unless <see cref="MaxLength"/> is specified.
    /// </remarks>
    [HtmlAttributeName(MaxWordsLengthAttributeName)]
    public int? MaxWords
    {
        get => _maxWords;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    $"{nameof(MaxWords)} must be greater than 0.");
            }

            _maxWords = value;
        }
    }

    /// <summary>
    /// The <c>name</c> attribute for the generated <c>textarea</c> element.
    /// </summary>
    /// <remarks>
    /// Required unless <see cref="FormGroupTagHelperBase.AspFor"/> is specified.
    /// </remarks>
    [HtmlAttributeName(NameAttributeName)]
    public string? Name { get; set; }

    /// <summary>
    /// The <c>rows</c> attribute for the generated <c>textarea</c> element.
    /// </summary>
    /// <remarks>
    /// The default is <c>5</c>.
    /// </remarks>
    [HtmlAttributeName(RowsAttributeName)]
    public int Rows { get; set; } = ComponentGenerator.TextAreaDefaultRows;

    /// <summary>
    /// The <c>spellcheck</c> attribute for the generated <c>textarea</c> element.
    /// </summary>
    [HtmlAttributeName(SpellcheckAttributeName)]
    public bool? Spellcheck { get; set; }

    /// <summary>
    /// Additional attributes to add to the generated <c>textarea</c> element.
    /// </summary>
    [HtmlAttributeName(DictionaryAttributePrefix = TextareaAttributesPrefix)]
    public IDictionary<string, string?> TextAreaAttributes { get; set; } = new Dictionary<string, string?>();

    /// <summary>
    /// The percentage value of the limit at which point the count message is displayed.
    /// </summary>
    /// <remarks>
    /// If this is specified the count message will be hidden by default.
    /// </remarks>
    [HtmlAttributeName(ThresholdAttributeName)]
    public decimal? Threshold
    {
        get => _threshold;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    $"{nameof(Threshold)} cannot be less than 0.");
            }

            _threshold = value;
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
        if (MaxLength.HasValue && MaxWords.HasValue)
        {
            throw new InvalidOperationException($"Only one of the '{MaxLengthAttributeName}' or '{MaxWordsLengthAttributeName}' attributes can be specified.");
        }

        var characterCountContext = new CharacterCountContext();

        using (context.SetScopedContextItem(characterCountContext))
        using (context.SetScopedContextItem(typeof(FormGroupContext2), characterCountContext))
        {
            await output.GetChildContentAsync();
        }

        var name = ResolveNameUnencoded();
        var id = ResolveId(name);
        var value = ResolveValue(characterCountContext);
        var labelOptions = characterCountContext.GetLabelOptions(AspFor, ViewContext!, _modelHelper, id, AspForAttributeName);
        var hintOptions = characterCountContext.GetHintOptions(AspFor, _modelHelper);
        var errorMessageOptions = characterCountContext.GetErrorMessageOptions(AspFor, ViewContext!, _modelHelper, IgnoreModelStateErrors);

        if (LabelClass is not null)
        {
            labelOptions.Classes = new HtmlString(labelOptions.Classes?.ToHtmlString() + " " + LabelClass);
        }

        var formGroupAttributes = EncodedAttributesDictionary.FromDictionaryWithEncodedValues(FormGroupAttributes);
        formGroupAttributes.Remove("class", out var formGroupClasses);
        var formGroupOptions = new FormGroupOptions()
        {
            Attributes = formGroupAttributes,
            Classes = formGroupClasses
        };

        var attributes = EncodedAttributesDictionary.FromDictionaryWithEncodedValues(TextAreaAttributes);
        attributes.Remove("class", out var classes);

        if (Autocomplete is not null)
        {
            attributes.Add("autocomplete", Autocomplete!.EncodeHtml()!);
        }

        if (Disabled)
        {
            attributes.AddBoolean("disabled");
        }

        var countMessageAttributes =
            EncodedAttributesDictionary.FromDictionaryWithEncodedValues(CountMessageAttributes);
        countMessageAttributes.Remove("class", out var countMessageClasses);

        var component = _componentGenerator.GenerateCharacterCount(new CharacterCountOptions
        {
            Id = id,
            Name = name.EncodeHtml(),
            Rows = Rows,
            Value = value,
            MaxLength = MaxLength,
            MaxWords = MaxWords,
            Threshold = Threshold,
            Label = labelOptions,
            Hint = hintOptions,
            ErrorMessage = errorMessageOptions,
            FormGroup = formGroupOptions,
            Classes = classes,
            Spellcheck = Spellcheck,
            Attributes = attributes,
            CountMessage = new()
            {
                Attributes = countMessageAttributes,
                Classes = countMessageClasses
            },
            TextareaDescriptionText = null,
            CharactersUnderLimitText = null,
            CharactersAtLimitText = null,
            CharactersOverLimitText = null,
            WordsUnderLimitText = null,
            WordsAtLimitText = null,
            WordsOverLimitText = null
        });

        component.WriteTo(output);

        if (errorMessageOptions is not null && context.TryGetContextItem<ContainerErrorContext>(out var containerErrorContext))
        {
            Debug.Assert(errorMessageOptions.Html is not null);
            containerErrorContext.AddError(errorMessageOptions.Html!, href: new HtmlString("#" + id.ToHtmlString()));
        }
    }

    private IHtmlContent ResolveId(string nameUnencoded)
    {
        if (Id is not null)
        {
            return Id.EncodeHtml();
        }

        return TagBuilder.CreateSanitizedId(nameUnencoded, Constants.IdAttributeDotReplacement).EncodeHtml();
    }

    private string ResolveNameUnencoded()
    {
        if (Name is null && AspFor is null)
        {
            throw ExceptionHelper.AtLeastOneOfAttributesMustBeProvided(
                NameAttributeName,
                AspForAttributeName);
        }

        return Name ?? _modelHelper.GetFullHtmlFieldName(ViewContext!, AspFor!.Name);
    }

    private IHtmlContent? ResolveValue(CharacterCountContext characterCountContext)
    {
        if (characterCountContext.Value is not null)
        {
            return characterCountContext.Value;
        }

        return AspFor is not null ? new HtmlString(_modelHelper.GetModelValue(ViewContext!, AspFor.ModelExplorer, AspFor.Name)) : null;
    }
}
