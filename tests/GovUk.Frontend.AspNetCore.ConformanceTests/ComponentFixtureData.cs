using System.Reflection;
using Newtonsoft.Json.Linq;
using Xunit.Sdk;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public class ComponentFixtureData : DataAttribute
{
    private readonly string _fixtureFileName;
    private readonly Type _optionsType;
    private readonly string _only;
    private readonly HashSet<string> _exclude;

    public ComponentFixtureData(
        string fixtureFileName,
        Type optionsType,
        string only = null,
        params string[] exclude)
    {
        _fixtureFileName = fixtureFileName ?? throw new ArgumentNullException(nameof(fixtureFileName));
        _optionsType = optionsType ?? throw new ArgumentNullException(nameof(optionsType));
        _only = only;
        _exclude = new HashSet<string>(exclude ?? Array.Empty<string>());

        if (!_fixtureFileName.EndsWith(".json"))
        {
            _fixtureFileName += ".json";
        }
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var fixturesFile = Path.Combine("Fixtures", _fixtureFileName);

        if (!File.Exists(fixturesFile))
        {
            throw new FileNotFoundException(
                $"Could not find fixtures file at: '{fixturesFile}'.",
                fixturesFile);
        }

        var fixturesJson = File.ReadAllText(fixturesFile);
        var fixtures = JObject.Parse(fixturesJson).SelectToken("fixtures");

        if (fixtures == null)
        {
            throw new InvalidOperationException($"Couldn't find fixtures in '{fixturesFile}'.");
        }

        var testCaseDataType = typeof(ComponentTestCaseData<>).MakeGenericType(_optionsType);

        foreach (var fixture in fixtures)
        {
            var name = fixture["name"].ToString();

            if (_exclude.Contains(name) || (_only != null && name != _only))
            {
                continue;
            }

            var options = fixture["options"].ToObject(_optionsType);
            var html = fixture["html"].ToString();

            var testCaseData = Activator.CreateInstance(testCaseDataType, name, options, html);

            yield return new object[]
            {
                testCaseData
            };
        }
    }
}
