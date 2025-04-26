using System.Text.Json;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

internal static class ComponentOptionsJsonSerializerOptions
{
    static ComponentOptionsJsonSerializerOptions()
    {
        var serializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        Instance = serializerOptions;
    }

    public static JsonSerializerOptions Instance { get; }
}
