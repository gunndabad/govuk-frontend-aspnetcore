using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson
{
    public record Checkboxes
    {
        public string DescribedBy { get; set; }
        public Fieldset Fieldset { get; set; }
        public Hint Hint { get; set; }
        public ErrorMessage ErrorMessage { get; set; }
        public FormGroup FormGroup { get; set; }
        public string IdPrefix { get; set; }
        public string Name { get; set; }
        public IList<CheckboxesItem> Items { get; set; }
        public string Classes { get; set; }
        public IDictionary<string, object> Attributes { get; set; }
    }

    public record CheckboxesItem
    {
        public string Text { get; set; }
        public string Html { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Label Label { get; set; }
        public Hint Hint { get; set; }
        public string Divider { get; set; }
        public bool? Checked { get; set; }
        public CheckboxesItemConditional Conditional { get; set; }
        public string Behaviour { get; set; }
        public bool? Disabled { get; set; }
        public IDictionary<string, object> Attributes { get; set; }
    }

    public record CheckboxesItemConditional
    {
        [JsonConverter(typeof(CheckboxesItemConditionalHtmlJsonConverter))]
        public string Html { get; set; }  
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CheckboxesItemConditionalHtmlJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(string);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                return (string)reader.Value;
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
