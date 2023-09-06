using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;

public class TextInput
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Inputmode { get; set; }
    public string Value { get; set; }
    public string DescribedBy { get; set; }
    public Label Label { get; set; }
    public Hint Hint { get; set; }
    public ErrorMessage ErrorMessage { get; set; }
    public TextInputPrefix Prefix { get; set; }
    public TextInputSuffix Suffix { get; set; }
    public FormGroup FormGroup { get; set; }
    public string Classes { get; set; }
    public string Autocomplete { get; set; }
    public string Pattern { get; set; }
    public bool? Spellcheck { get; set; }
    public IDictionary<string, object> Attributes { get; set; }
}

public class TextInputPrefix
{
    public string Text { get; set; }
    public string Html { get; set; }
    public string Classes { get; set; }
    public IDictionary<string, object> Attributes { get; set; }
}

public class TextInputSuffix
{
    public string Text { get; set; }
    public string Html { get; set; }
    public string Classes { get; set; }
    public IDictionary<string, object> Attributes { get; set; }
}
