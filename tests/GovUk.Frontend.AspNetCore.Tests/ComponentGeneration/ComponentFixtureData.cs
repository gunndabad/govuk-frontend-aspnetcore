using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Xunit.Sdk;

namespace GovUk.Frontend.AspNetCore.Tests.ComponentGeneration;

public class ComponentFixtureData : DataAttribute
{
    private static readonly JsonSerializerOptions _serializerOptions;

    static ComponentFixtureData()
    {
        _serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        _serializerOptions.Converters.Insert(0, new PermissiveStringJsonConverter());
    }

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
            if (fixture!["hidden"]?.GetValue<bool>() != true)
            {
                continue;
            }

            var name = fixture["name"]!.ToString();

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

    private class PermissiveStringJsonConverter : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    return reader.GetString();
                case JsonTokenType.Number:
                    return reader.GetInt64().ToString();
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.False:
                    return string.Empty;
                default:
                    throw new JsonException($"Cannot convert {reader.TokenType} tokens to a string.");
            }
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
        }
    }
}
