using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;

public class FileUpload
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public bool Disabled { get; set; }
    public string DescribedBy { get; set; }
    public Label Label { get; set; }
    public Hint Hint { get; set; }
    public ErrorMessage ErrorMessage { get; set; }
    public FormGroup FormGroup { get; set; }
    public string Classes { get; set; }
    public IDictionary<string, object> Attributes { get; set; }
}
