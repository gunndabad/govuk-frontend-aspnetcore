using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit.Sdk;

namespace GovUk.Frontend.AspNetCore.Tests.ComponentGeneration;

public class ComponentFixtureData : DataAttribute
{
    private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

    private readonly string _componentName;
    private readonly Type _optionsType;
    private readonly string? _only;
    private readonly HashSet<string> _exclude;

    public ComponentFixtureData(
        string componentName,
        Type optionsType,
        string? only = null,
        params string[] exclude)
    {
        _componentName = componentName;
        _optionsType = optionsType;
        _only = only;
        _exclude = new HashSet<string>(exclude ?? Array.Empty<string>());
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var fixturesFile = Path.Combine("ComponentGeneration", "Fixtures", $"{_componentName}.json");

        if (!File.Exists(fixturesFile))
        {
            throw new FileNotFoundException(
                $"Could not find fixtures file at: '{fixturesFile}'.",
                fixturesFile);
        }

        var fixturesJson = File.ReadAllText(fixturesFile);
        var fixtures = JsonObject.Parse(fixturesJson)!["fixtures"]?.AsArray();

        if (fixtures == null)
        {
            throw new InvalidOperationException($"Couldn't find fixtures in '{fixturesFile}'.");
        }

        var testCaseDataType = typeof(ComponentTestCaseData<>).MakeGenericType(_optionsType);

        foreach (var fixture in fixtures)
        {
            var name = fixture!["name"]!.ToString();

            if (_exclude.Contains(name) || _only != null && name != _only)
            {
                continue;
            }

            var options = fixture["options"]!.Deserialize(_optionsType, _serializerOptions);
            var html = fixture["html"]!.ToString();

            var testCaseData = Activator.CreateInstance(testCaseDataType, name, options, html)!;

            yield return new object[]
            {
                testCaseData
            };
        }
    }
}
