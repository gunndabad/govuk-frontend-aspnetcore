using System.Linq;
using GovUk.Frontend.AspNetCore.TestCommon;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests
{
    public class PageTemplateHelperTests
    {
        [Fact]
        public void GenerateJsEnabledScript_WithNonce_AppendsNonceToScript()
        {
            // Arrange
            var nonce = "nonce123";
            var pageTemplateHelper = new PageTemplateHelper();

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
            var pageTemplateHelper = new PageTemplateHelper();

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
            var nonce = "nonce123";
            var pageTemplateHelper = new PageTemplateHelper();

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
            var pageTemplateHelper = new PageTemplateHelper();

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
            var pageTemplateHelper = new PageTemplateHelper();

            // Act
            var result = pageTemplateHelper.GetCspScriptHashes();

            // Assert
            Assert.Equal("'sha256-l1eTVSK8DTnK8+yloud7wZUqFrI0atVo6VlC6PJvYaQ=' 'sha256-wmo5EWLjw+Yuj9jZzGNNeSsUOBQmBvE1pvSPVNQzJ34='", result);
        }

        [Fact]
        public void GetJsEnabledScriptCspHash()
        {
            // Arrange
            var pageTemplateHelper = new PageTemplateHelper();

            // Act
            var result = pageTemplateHelper.GetJsEnabledScriptCspHash();

            // Assert
            Assert.Equal("'sha256-wmo5EWLjw+Yuj9jZzGNNeSsUOBQmBvE1pvSPVNQzJ34='", result);
        }

        [Fact]
        public void GetInitScriptCspHash()
        {
            // Arrange
            var pageTemplateHelper = new PageTemplateHelper();

            // Act
            var result = pageTemplateHelper.GetInitScriptCspHash();

            // Assert
            Assert.Equal("'sha256-l1eTVSK8DTnK8+yloud7wZUqFrI0atVo6VlC6PJvYaQ='", result);
        }
    }
}
