using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS textarea component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(LabelTagName, HintTagName, ErrorMessageTagName, TextAreaValueTagHelper.TagName)]
[OutputElementHint(ComponentGenerator.FormGroupElement)]
public class TextAreaTagHelper : FormGroupTagHelperBase
{
    internal const string ErrorMessageTagName = "govuk-textarea-error-message";
    internal const string HintTagName = "govuk-textarea-hint";
    internal const string LabelTagName = "govuk-textarea-label";
    internal const string TagName = "govuk-textarea";

    private const string AutocompleteAttributeName = "autocomplete";
    private const string DisabledAttributeName = "disabled";
    private const string IdAttributeName = "id";
    private const string LabelClassAttributeName = "label-class";
    private const string NameAttributeName = "name";
    private const string RowsAttributeName = "rows";
    private const string SpellcheckAttributeName = "spellcheck";
    private const string TextareaAttributesPrefix = "textarea-";

    /// <summary>
    /// Creates an <see cref="TextAreaTagHelper"/>.
    /// </summary>
    public TextAreaTagHelper()
        : this(htmlGenerator: null, modelHelper: null)
    {
    }

    internal TextAreaTagHelper(IGovUkHtmlGenerator? htmlGenerator = null, IModelHelper? modelHelper = null)
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
    /// Whether the <c>disabled</c> attribute should be added to the generated <c>textarea</c> element.
    /// </summary>
    [HtmlAttributeName(DisabledAttributeName)]
    public bool Disabled { get; set; } = ComponentGenerator.TextAreaDefaultDisabled;

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

    private protected override FormGroupContext CreateFormGroupContext() => new TextAreaContext();

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
            var textAreaContext = tagHelperContext.GetContextItem<TextAreaContext>();

            var resolvedId = ResolveIdPrefix();
            var resolvedName = ResolveNameUnencoded();

            var resolvedContent = textAreaContext.Value ??
                ((For != null ? ModelHelper.GetModelValue(ViewContext!, For.ModelExplorer, For.Name) : null) ??
                    string.Empty).EncodeHtml();

            var resolvedTextAreaAttributes = TextAreaAttributes.ToAttributeDictionary();
            resolvedTextAreaAttributes.MergeCssClass("govuk-js-textarea");

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

        if (Name == null && For == null)
        {
            throw ExceptionHelper.AtLeastOneOfAttributesMustBeProvided(
                IdAttributeName,
                NameAttributeName,
                AspForAttributeName);
        }

        var resolvedName = ResolveNameUnencoded();

        return TagBuilder.CreateSanitizedId(resolvedName, Constants.IdAttributeDotReplacement);
    }

    private string ResolveNameUnencoded()
    {
        if (Name == null && For == null)
        {
            throw ExceptionHelper.AtLeastOneOfAttributesMustBeProvided(
                NameAttributeName,
                AspForAttributeName);
        }

        return Name ?? ModelHelper.GetFullHtmlFieldName(ViewContext!, For!.Name);
    }
}
