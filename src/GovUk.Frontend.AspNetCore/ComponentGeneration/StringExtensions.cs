namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

internal static class StringExtensions
{
    public static string? NormalizeEmptyString(this string? value) => string.IsNullOrEmpty(value) ? null : value;
}
