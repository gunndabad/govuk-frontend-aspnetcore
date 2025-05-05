using System.Text.Json.Serialization;

namespace GovUk.Frontend.AspNetCore.Components;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record CharacterCountOptions2
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public int? Rows { get; set; }
    public string? Value { get; set; }
    [JsonPropertyName("maxlength")]
    public int? MaxLength { get; set; }
    [JsonPropertyName("maxwords")]
    public int? MaxWords { get; set; }
    public decimal? Threshold { get; set; }
    public LabelOptions2? Label { get; set; }
    public HintOptions2? Hint { get; set; }
    public ErrorMessageOptions2? ErrorMessage { get; set; }
    public CharacterCountOptions2FormGroup? FormGroup { get; set; }
    public string? Classes { get; set; }
    public bool? Spellcheck { get; set; }
    public AttributeCollection? Attributes { get; set; }
    public CharacterCountCountOptions2Message? CountMessage { get; set; }
    public string? TextareaDescriptionText { get; set; }
    public CharacterCountOptions2LocalizedText? CharactersUnderLimitText { get; set; }
    public string? CharactersAtLimitText { get; set; }
    public CharacterCountOptions2LocalizedText? CharactersOverLimitText { get; set; }
    public CharacterCountOptions2LocalizedText? WordsUnderLimitText { get; set; }
    public string? WordsAtLimitText { get; set; }
    public CharacterCountOptions2LocalizedText? WordsOverLimitText { get; set; }
}

public record CharacterCountCountOptions2Message
{
    public string? Classes { get; set; }
    [NonStandardParameter]
    public AttributeCollection? Attributes { get; set; }
}

public record CharacterCountOptions2LocalizedText
{
    public string? Other { get; set; }
    public string? One { get; set; }
}

public record CharacterCountOptions2FormGroup : FormGroupOptions2
{
    public TextHtmlAndAttributesOptions? BeforeInput { get; set; }
    public TextHtmlAndAttributesOptions? AfterInput { get; set; }
}
