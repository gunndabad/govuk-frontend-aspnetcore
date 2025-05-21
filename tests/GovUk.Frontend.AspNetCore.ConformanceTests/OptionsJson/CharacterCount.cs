namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;

public class CharacterCount
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int? Rows { get; set; }
    public string Value { get; set; }
    public int? MaxLength { get; set; }
    public int? MaxWords { get; set; }
    public int? Threshold { get; set; }
    public Label Label { get; set; }
    public Hint Hint { get; set; }
    public ErrorMessage ErrorMessage { get; set; }
    public TextInputPrefix Prefix { get; set; }
    public TextInputSuffix Suffix { get; set; }
    public FormGroup FormGroup { get; set; }
    public string Classes { get; set; }
    public bool? Spellcheck { get; set; }
    public IDictionary<string, object> Attributes { get; set; }
    public CharacterCountCountMessage CountMessage { get; set; }
    public string TextareaDescriptionText { get; set; }
    public CharacterCountLocalizedText CharactersUnderLimitText { get; set; }
    public string CharactersAtLimitText { get; set; }
    public CharacterCountLocalizedText CharactersOverLimitText { get; set; }
    public CharacterCountLocalizedText WordsUnderLimitText { get; set; }
    public string WordsAtLimitText { get; set; }
    public CharacterCountLocalizedText WordsOverLimitText { get; set; }
}

public class CharacterCountCountMessage
{
    public string Classes { get; set; }
}

public class CharacterCountLocalizedText
{
    public string One { get; set; }
    public string Other { get; set; }
}
