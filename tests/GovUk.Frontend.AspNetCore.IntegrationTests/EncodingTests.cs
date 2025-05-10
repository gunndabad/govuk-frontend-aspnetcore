
namespace GovUk.Frontend.AspNetCore.IntegrationTests;

public class EncodingTests(EncodingsTestFixture fixture) : IClassFixture<EncodingsTestFixture>
{
    [Theory]
    [MemberData(nameof(ComponentWithHrefData))]
    public async Task StringHref(string subPath, int startIndexForAttributeSearch)
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, $"/EncodingTests/{subPath}/StringHref");

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var html = (await response.Content.ReadAsStringAsync()).Trim();
        var href = GetLinkAttributeValue(html, "href", startIndexForAttributeSearch);
        Assert.Equal("/Empty?foo=bar&baz=qux", href);
    }

    [Theory]
    [MemberData(nameof(ComponentWithHrefData))]
    public async Task GeneratedHref(string subPath, int startIndexForAttributeSearch)
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, $"/EncodingTests/{subPath}/GeneratedHref");

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var html = (await response.Content.ReadAsStringAsync()).Trim();
        var href = GetLinkAttributeValue(html, "href", startIndexForAttributeSearch);
        Assert.Equal("/Empty?foo=bar&amp;baz=qux", href);
    }

    [Theory]
    [MemberData(nameof(ComponentWithHrefData))]
    public async Task ExpressionHref(string subPath, int startIndexForAttributeSearch)
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, $"/EncodingTests/{subPath}/ExpressionHref");

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var html = (await response.Content.ReadAsStringAsync()).Trim();
        var href = GetLinkAttributeValue(html, "href", startIndexForAttributeSearch);
        Assert.Equal("/Empty?foo=bar&amp;baz=qux", href);
    }

    [Theory]
    [MemberData(nameof(ComponentWithFormActionData))]
    public async Task StringFormAction(string subPath, int startIndexForAttributeSearch)
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, $"/EncodingTests/{subPath}/StringFormAction");

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var html = (await response.Content.ReadAsStringAsync()).Trim();
        var formaction = GetLinkAttributeValue(html, "formaction", startIndexForAttributeSearch);
        Assert.Equal("/Empty?foo=bar&baz=qux", formaction);
    }

    [Theory]
    [MemberData(nameof(ComponentWithFormActionData))]
    public async Task GeneratedFormAction(string subPath, int startIndexForAttributeSearch)
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, $"/EncodingTests/{subPath}/GeneratedFormAction");

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var html = (await response.Content.ReadAsStringAsync()).Trim();
        var formaction = GetLinkAttributeValue(html, "formaction", startIndexForAttributeSearch);
        Assert.Equal("/Empty?foo=bar&amp;baz=qux", formaction);
    }

    [Theory]
    [MemberData(nameof(ComponentWithFormActionData))]
    public async Task ExpressionFormAction(string subPath, int startIndexForAttributeSearch)
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, $"/EncodingTests/{subPath}/ExpressionFormAction");

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var html = (await response.Content.ReadAsStringAsync()).Trim();
        var formaction = GetLinkAttributeValue(html, "formaction", startIndexForAttributeSearch);
        Assert.Equal("/Empty?foo=bar&amp;baz=qux", formaction);
    }

    public static TheoryData<string, int> ComponentWithHrefData { get; } = new()
    {
        { "BackLink", 0 },
        { "Breadcrumbs", 0 },
        { "ButtonLink", 0 },
        //{ "ErrorSummary", 0 },  // skipping for now; we shouldn't need this as links are fragments and won't contain query params
        { "Pagination", 0 },
        { "SummaryList", 0 }
    };

    public static TheoryData<string, int> ComponentWithFormActionData { get; } = new()
    {
        { "Button", 0 }
    };

    private static string GetLinkAttributeValue(string html, string attributeName, int startIndex = 0)
    {
        // We can't use AngleSharp because it does weird decoding things and we don't get accurate values back
        var hrefPos = html.IndexOf(attributeName + "=\"", startIndex, StringComparison.Ordinal);
        var startOfAttributeValue = hrefPos + attributeName.Length + 2;
        var endOfAttributeValue = html.IndexOf("\"", startIndex: startOfAttributeValue + 1, StringComparison.Ordinal);
        return html[startOfAttributeValue..endOfAttributeValue];
    }
}

public class EncodingsTestFixture : ServerFixture
{
    public EncodingsTestFixture()
    {
        HttpClient = new HttpClient()
        {
            BaseAddress = new Uri(BaseUrl)
        };
    }

    public HttpClient HttpClient { get; }

    public override Task DisposeAsync()
    {
        HttpClient.Dispose();
        return base.DisposeAsync();
    }

    protected override void Configure(IApplicationBuilder app)
    {
        base.Configure(app);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services
            .AddRazorPages();
    }
}
