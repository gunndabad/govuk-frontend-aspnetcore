using System.Text.Json.Serialization;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record CharacterCountOptions
{
    public TemplateString? Id { get; set; }
    public TemplateString? Name { get; set; }
    public int? Rows { get; set; }
    public TemplateString? Value { get; set; }
    [JsonPropertyName("maxlength")]
    public int? MaxLength { get; set; }
    [JsonPropertyName("maxwords")]
    public int? MaxWords { get; set; }
    public decimal? Threshold { get; set; }
    public LabelOptions? Label { get; set; }
    public HintOptions? Hint { get; set; }
    public ErrorMessageOptions? ErrorMessage { get; set; }
    public CharacterCountOptionsFormGroup? FormGroup { get; set; }
    public TemplateString? Classes { get; set; }
    public bool? Spellcheck { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public CharacterCountCountOptionsMessage? CountMessage { get; set; }
    public TemplateString? TextareaDescriptionText { get; set; }
    public CharacterCountOptionsLocalizedText? CharactersUnderLimitText { get; set; }
    public TemplateString? CharactersAtLimitText { get; set; }
    public CharacterCountOptionsLocalizedText? CharactersOverLimitText { get; set; }
    public CharacterCountOptionsLocalizedText? WordsUnderLimitText { get; set; }
    public TemplateString? WordsAtLimitText { get; set; }
    public CharacterCountOptionsLocalizedText? WordsOverLimitText { get; set; }
}

public record CharacterCountCountOptionsMessage
{
    public TemplateString? Classes { get; set; }
    [NonStandardParameter]
    public AttributeCollection? Attributes { get; set; }
}

public record CharacterCountOptionsLocalizedText
{
    public TemplateString? Other { get; set; }
    public TemplateString? One { get; set; }
}

public record CharacterCountOptionsFormGroup : FormGroupOptions
{
    public TextHtmlAndAttributesOptions? BeforeInput { get; set; }
    public TextHtmlAndAttributesOptions? AfterInput { get; set; }
}
