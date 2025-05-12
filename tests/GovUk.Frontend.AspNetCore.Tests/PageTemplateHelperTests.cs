using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.Tests;

public class PageTemplateHelperTests
{
    [Fact]
    public void GenerateJsEnabledScript_WithNonce_AppendsNonceToScript()
    {
        // Arrange
        var options = Options.Create(new GovUkFrontendAspNetCoreOptions() { CompiledContentPath = "/govuk" });
        var pageTemplateHelper = new PageTemplateHelper(options);
        var nonce = "nonce123";

        // Act
        var result = pageTemplateHelper.GenerateJsEnabledScript(nonce);

        // Assert
        var element = result.RenderToElement();
        Assert.Equal(nonce, element.GetAttribute("nonce"));
    }

    [Fact]
    public void GenerateJsEnabledScript_WithoutNonce_DoesNotHaveNonceAttribute()
    {
        // Arrange
        var options = Options.Create(new GovUkFrontendAspNetCoreOptions() { CompiledContentPath = "/govuk" });
        var pageTemplateHelper = new PageTemplateHelper(options);

        // Act
        var result = pageTemplateHelper.GenerateJsEnabledScript(cspNonce: null);

        // Assert
        var element = result.RenderToElement();
        Assert.DoesNotContain(element.Attributes, attr => attr.Name == "nonce");
    }

    [Fact]
    public void GenerateScriptImports_WithNonce_AppendsNonceToScript()
    {
        // Arrange
        var options = Options.Create(new GovUkFrontendAspNetCoreOptions() { CompiledContentPath = "/govuk" });
        var pageTemplateHelper = new PageTemplateHelper(options);
        var nonce = "nonce123";

        // Act
        var result = pageTemplateHelper.GenerateScriptImports(nonce);

        // Assert
        var inlineScriptElement = result.RenderToElements().Last();
        Assert.Equal(nonce, inlineScriptElement.GetAttribute("nonce"));
    }

    [Fact]
    public void GenerateScriptImports_WithoutNonce_DoesNotHaveNonceAttribute()
    {
        // Arrange
        var options = Options.Create(new GovUkFrontendAspNetCoreOptions() { CompiledContentPath = "/govuk" });
        var pageTemplateHelper = new PageTemplateHelper(options);

        // Act
        var result = pageTemplateHelper.GenerateScriptImports(cspNonce: null);

        // Assert
        var inlineScriptElement = result.RenderToElements().Last();
        Assert.DoesNotContain(inlineScriptElement.Attributes, attr => attr.Name == "nonce");
    }

    [Fact]
    public void GetCspScriptHashes()
    {
        // Arrange
        var options = Options.Create(new GovUkFrontendAspNetCoreOptions() { CompiledContentPath = "/govuk" });
        var pageTemplateHelper = new PageTemplateHelper(options);

        // Act
        var result = pageTemplateHelper.GetCspScriptHashes();

        // Assert
        Assert.Equal("'sha256-l5MP+9OapFXGxjKMNj/89ExAW2TvAFFoADrbsmtSJXo=' 'sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw='", result);
    }

    [Fact]
    public void GetJsEnabledScriptCspHash()
    {
        // Arrange
        var options = Options.Create(new GovUkFrontendAspNetCoreOptions() { CompiledContentPath = "/govuk" });
        var pageTemplateHelper = new PageTemplateHelper(options);

        // Act
        var result = pageTemplateHelper.GetJsEnabledScriptCspHash();

        // Assert
        Assert.Equal("'sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw='", result);
    }

    [Fact]
    public void GetInitScriptCspHash()
    {
        // Arrange
        var options = Options.Create(new GovUkFrontendAspNetCoreOptions() { CompiledContentPath = "/govuk" });
        var pageTemplateHelper = new PageTemplateHelper(options);

        // Act
        var result = pageTemplateHelper.GetInitScriptCspHash();

        // Assert
        Assert.Equal("'sha256-l5MP+9OapFXGxjKMNj/89ExAW2TvAFFoADrbsmtSJXo='", result);
    }

    [Fact]
    public void GetGovUkFrontendVersion_ReturnsVersion()
    {
        // Arrange

        // Act
        var result = PageTemplateHelper.GovUkFrontendVersion;

        // Assert
        Assert.NotNull(result);
    }
}
