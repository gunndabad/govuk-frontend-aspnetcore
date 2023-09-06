using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS character count component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(LabelTagName, HintTagName, ErrorMessageTagName, CharacterCountValueTagHelper.TagName)]
[OutputElementHint(ComponentGenerator.CharacterCountElement)]
public class CharacterCountTagHelper : FormGroupTagHelperBase
{
    internal const string ErrorMessageTagName = "govuk-character-count-error-message";
    internal const string HintTagName = "govuk-character-count-hint";
    internal const string LabelTagName = "govuk-character-count-label";
    internal const string TagName = "govuk-character-count";

    private const string AutocompleteAttributeName = "autocomplete";
    private const string CountMessageAttributesPrefix = "count-message-";
    private const string DisabledAttributeName = "disabled";
    private const string FormGroupAttributesPrefix = "form-group-";
    private const string IdAttributeName = "id";
    private const string LabelClassAttributeName = "label-class";
    private const string MaxLengthAttributeName = "max-length";
    private const string MaxWordsLengthAttributeName = "max-words";
    private const string NameAttributeName = "name";
    private const string RowsAttributeName = "rows";
    private const string SpellcheckAttributeName = "spellcheck";
    private const string TextareaAttributesPrefix = "textarea-";
    private const string ThresholdAttributeName = "threshold";

    private decimal? _threshold;
    private int? _maxLength;
    private int? _maxWords;

    /// <summary>
    /// Creates an <see cref="CharacterCountTagHelper"/>.
    /// </summary>
    public CharacterCountTagHelper()
        : this(htmlGenerator: null, modelHelper: null)
    {
    }

    internal CharacterCountTagHelper(IGovUkHtmlGenerator? htmlGenerator = null, IModelHelper? modelHelper = null)
        : base(
              htmlGenerator ?? new ComponentGenerator(),
              modelHelper ?? new DefaultModelHelper())
    {
    }

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

    private protected override TagBuilder CreateTagBuilder(bool haveError, IHtmlContent content, TagHelperOutput tagHelperOutput)
    {
        if (MaxLength.HasValue && MaxWords.HasValue)
        {
            throw new InvalidOperationException($"Only one of the '{MaxLengthAttributeName}' or '{MaxWordsLengthAttributeName}' attributes can be specified.");
        }

        // N.B. We specifically pass FormGroupAttributes here and not tagHelperOutput.Attributes since GenerateCharacterCount() doesn't return a form group
        var formGroup = Generator.GenerateFormGroup(
            haveError,
            content,
            FormGroupAttributes.ToAttributeDictionary());

        var resolvedId = ResolveIdPrefix();

        return Generator.GenerateCharacterCount(
            resolvedId,
            MaxLength,
            MaxWords,
            Threshold,
            formGroup,
            CountMessageAttributes.ToAttributeDictionary(),
            null,
            null,
            null,
            null,
            null,
            null,
            null);
    }

    private protected override FormGroupContext CreateFormGroupContext() => new CharacterCountContext();

    private protected override IHtmlContent GenerateFormGroupContent(
        TagHelperContext tagHelperContext,
        FormGroupContext formGroupContext,
        TagHelperOutput tagHelperOutput,
        IHtmlContent childContent,
        out bool haveError)
    {
        var contentBuilder = new HtmlContentBuilder();

        var label = GenerateLabel(formGroupContext, LabelClass);
        contentBuilder.AppendHtml(label);

        var hint = GenerateHint(tagHelperContext, formGroupContext);
        if (hint != null)
        {
            contentBuilder.AppendHtml(hint);
        }

        var errorMessage = GenerateErrorMessage(tagHelperContext, formGroupContext);
        if (errorMessage != null)
        {
            contentBuilder.AppendHtml(errorMessage);
        }

        haveError = errorMessage != null;

        var textAreaTagBuilder = GenerateTextArea(haveError);
        contentBuilder.AppendHtml(textAreaTagBuilder);

        return contentBuilder;

        TagBuilder GenerateTextArea(bool haveError)
        {
            var characterCountContext = tagHelperContext.GetContextItem<CharacterCountContext>();

            var resolvedId = ResolveIdPrefix();
            var resolvedName = ResolveName();

            var resolvedContent = characterCountContext.Value ??
                new HtmlString(HtmlEncoder.Default.Encode(
                    AspFor != null ? ModelHelper.GetModelValue(ViewContext!, AspFor.ModelExplorer, AspFor.Name) ?? string.Empty : string.Empty));

            var resolvedTextAreaAttributes = TextAreaAttributes.ToAttributeDictionary();
            resolvedTextAreaAttributes.MergeCssClass("govuk-js-character-count");

            return Generator.GenerateTextArea(
                haveError,
                resolvedId,
                resolvedName,
                Rows,
                DescribedBy,
                Autocomplete,
                Spellcheck,
                Disabled,
                resolvedContent,
                resolvedTextAreaAttributes);
        }
    }

    private protected override string ResolveIdPrefix()
    {
        if (Id != null)
        {
            return Id;
        }

        if (Name == null && AspFor == null)
        {
            throw ExceptionHelper.AtLeastOneOfAttributesMustBeProvided(
                IdAttributeName,
                NameAttributeName,
                AspForAttributeName);
        }

        var resolvedName = ResolveName();

        return TagBuilder.CreateSanitizedId(resolvedName, Constants.IdAttributeDotReplacement);
    }

    private string ResolveName()
    {
        if (Name == null && AspFor == null)
        {
            throw ExceptionHelper.AtLeastOneOfAttributesMustBeProvided(
                NameAttributeName,
                AspForAttributeName);
        }

        return Name ?? ModelHelper.GetFullHtmlFieldName(ViewContext!, AspFor!.Name);
    }
}
