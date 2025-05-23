using System.Reflection;

namespace GovUk.Frontend.AspNetCore;

/// <summary>
/// Contains information about the target govuk-frontend library.
/// </summary>
public static class GovUkFrontendInfo
{
    /// <summary>
    /// Gets the version of the GOV.UK Frontend library.
    /// </summary>
    public static string Version { get; } = GetGovUkFrontendVersion();

    private static string GetGovUkFrontendVersion() =>
        typeof(PageTemplateHelper).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(a => a.Key == "GovUkFrontendVersion")
            .Value!;
}
