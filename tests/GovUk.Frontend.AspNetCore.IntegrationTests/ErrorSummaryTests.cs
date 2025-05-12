using AngleSharp.Dom;

namespace GovUk.Frontend.AspNetCore.IntegrationTests;

public class ErrorSummaryTests(EncodingsTestFixture fixture) : IClassFixture<EncodingsTestFixture>
{
    [Fact]
    public async Task ComponentInPartialHasError_ShowsErrorInErrorSummary()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/FormWithErrorsInChildViews");

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        var doc = await AssertEx.GetHtmlDocument(response);
        var errorSummary = Assert.Single(doc.GetElementsByClassName("govuk-error-summary"));
        Assert.Contains("#InputInAPartial", GetErrorItemLinksFromErrorSummary(errorSummary));
    }

    [Fact]
    public async Task ComponentInViewComponentHasError_ShowsErrorInErrorSummary()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/FormWithErrorsInChildViews");

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        var doc = await AssertEx.GetHtmlDocument(response);
        var errorSummary = Assert.Single(doc.GetElementsByClassName("govuk-error-summary"));
        Assert.Contains("#InputInAViewComponent", GetErrorItemLinksFromErrorSummary(errorSummary));
    }

    private IEnumerable<string> GetErrorItemLinksFromErrorSummary(IElement element) =>
        element
            .GetElementsByClassName("govuk-error-summary__list")
            .Single()
            .GetElementsByTagName("li")
            .SelectMany(li => li.GetElementsByTagName("a"))
            .Select(a => a.GetAttribute("href"));
}

public class ErrorSummaryTestFixture : ServerFixture
{
    public ErrorSummaryTestFixture()
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
