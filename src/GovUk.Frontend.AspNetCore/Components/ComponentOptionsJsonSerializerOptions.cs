using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace GovUk.Frontend.AspNetCore.Components;

internal static class ComponentOptionsJsonSerializerOptions
{
    static ComponentOptionsJsonSerializerOptions()
    {
        var serializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver()
        };

        Instance = serializerOptions;
    }

    public static JsonSerializerOptions Instance { get; }
}
