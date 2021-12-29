using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson
{
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
        public CharacterCountCountMessage CountMessage{ get; set; }
    }

    public class CharacterCountCountMessage
    {
        public string Classes { get; set; }
    }
}
