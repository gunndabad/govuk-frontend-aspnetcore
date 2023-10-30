using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class CharacterCountOptions
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public int? Rows { get; set; }
    public string? Value { get; set; }
    public int? MaxLength { get; set; }
    public int? MaxWords { get; set; }
    public int? Threshold { get; set; }
    public LabelOptions? Label { get; set; }
    public HintOptions? Hint { get; set; }
    public ErrorMessageOptions? ErrorMessage { get; set; }
    public FormGroupOptions? FormGroup { get; set; }
    public string? Classes { get; set; }
    public bool? Spellcheck { get; set; }
    public IReadOnlyDictionary<string, string?>? Attributes { get; set; }
    public CharacterCountCountOptionsMessage? CountMessage { get; set; }
    public string? TextareaDescriptionText { get; set; }
    public CharacterCountOptionsLocalizedText? CharactersUnderLimitText { get; set; }
    public string? CharactersAtLimitText { get; set; }
    public CharacterCountOptionsLocalizedText? CharactersOverLimitText { get; set; }
    public CharacterCountOptionsLocalizedText? WordsUnderLimitText { get; set; }
    public string? WordsAtLimitText { get; set; }
    public CharacterCountOptionsLocalizedText? WordsOverLimitText { get; set; }
}

public class CharacterCountCountOptionsMessage
{
    public string? Classes { get; set; }
}

public class CharacterCountOptionsLocalizedText
{
    public string? One { get; set; }
    public string? Other { get; set; }
}
