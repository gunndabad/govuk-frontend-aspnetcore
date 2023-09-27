namespace GovUk.Frontend.AspNetCore;

internal sealed class NoAppendHtmlSnippetsMarker
{
    public const string ViewDataKey = "GovUk.Frontend.AspNetCore.NoAppendHtmlSnippetsMarker";

    public static NoAppendHtmlSnippetsMarker Instance { get; } = new();
}
