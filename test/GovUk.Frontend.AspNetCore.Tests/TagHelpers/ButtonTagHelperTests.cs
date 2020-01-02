using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class ButtonTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AnchorElementGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ButtonTagHelper(
                new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>(), Mock.Of<IHtmlGenerator>()))
            {
                Element = ButtonTagHelperElementType.Anchor,
                Href = "http://foo.com"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<a class=\"govuk-button\" data-module=\"govuk-button\" href=\"http://foo.com\" role=\"button\" draggable=\"false\">" +
                "Button text" +
                "</a>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_ButtonElementGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ButtonTagHelper(
                new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>(), Mock.Of<IHtmlGenerator>()))
            {
                Element = ButtonTagHelperElementType.Button
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<button class=\"govuk-button\" data-module=\"govuk-button\">" +
                "Button text" +
                "</button>",
                html);
        }

        [Theory]
        [MemberData(nameof(ProcessAsync_CorrectlyInfersElementTypeData))]
        public async Task ProcessAsync_CorrectlyInfersElementType(
            string href,
            string action,
            string controller,
            string area,
            string fragment,
            string host,
            string page,
            string pageHandler,
            string protocol,
            string route,
            IDictionary<string, string> routeValues)
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<IGovUkHtmlGenerator>();

            htmlGenerator
                .Setup(mock => mock
                    .GenerateAnchor(/*href: */It.IsAny<string>()))
                .Returns(new TagBuilder("a"));

            htmlGenerator
                .Setup(mock => mock
                    .GetActionLinkHref(
                        /*viewContext: */It.IsAny<ViewContext>(),
                        /*action: */It.IsAny<string>(),
                        /*controller: */It.IsAny<string>(),
                        /*values: */It.IsAny<object>(),
                        /*protocol: */It.IsAny<string>(),
                        /*host: */It.IsAny<string>(),
                        /*fragment: */It.IsAny<string>()
                    ))
                .Returns("http://place.com");

            htmlGenerator
                .Setup(mock => mock
                    .GetPageLinkHref(
                        /*viewContext: */It.IsAny<ViewContext>(),
                        /*pageName: */It.IsAny<string>(),
                        /*pageHandler: */It.IsAny<string>(),
                        /*values: */It.IsAny<object>(),
                        /*protocol: */It.IsAny<string>(),
                        /*host: */It.IsAny<string>(),
                        /*fragment: */It.IsAny<string>()
                    ))
                .Returns("http://place.com");

            htmlGenerator
                .Setup(mock => mock
                    .GetRouteLinkHref(
                        /*viewContext: */It.IsAny<ViewContext>(),
                        /*routeName: */It.IsAny<string>(),
                        /*values: */It.IsAny<object>(),
                        /*protocol: */It.IsAny<string>(),
                        /*host: */It.IsAny<string>(),
                        /*fragment: */It.IsAny<string>()
                    ))
                .Returns("http://place.com");

            var tagHelper = new ButtonTagHelper(htmlGenerator.Object)
            {
                Action = action,
                Area = area,
                Controller = controller,
                Fragment = fragment,
                Host = host,
                Href = href,
                Page = page,
                PageHandler = pageHandler,
                Protocol = protocol,
                Route = route,
                RouteValues = routeValues
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("a", output.TagName);
        }

        [Fact]
        public async Task ProcessAsync_IsStartButtonAddsIconToOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ButtonTagHelper(
                new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>(), Mock.Of<IHtmlGenerator>()))
            {
                Element = ButtonTagHelperElementType.Button,
                IsStartButton = true
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Contains("govuk-button--start", output.Attributes["class"].Value.ToString().Split(' '));
            Assert.Contains("<svg class=\"govuk-button__start-icon\"", output.Content.GetContent());
        }

        [Fact]
        public async Task ProcessAsync_ButtonDisabledSpecifiedAddsDisabledAttributesToOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ButtonTagHelper(
                new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>(), Mock.Of<IHtmlGenerator>()))
            {
                Element = ButtonTagHelperElementType.Button,
                Disabled = true
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("button", output.TagName);
            Assert.Contains("govuk-button--disabled", output.Attributes["class"].Value.ToString().Split(' '));
            Assert.Equal("disabled", output.Attributes["disabled"].Value);
            Assert.Equal("true", output.Attributes["aria-disabled"].Value);
        }

        [Fact]
        public async Task ProcessAsync_AnchorDisabledSpecifiedAddsDisabledAttributesToOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ButtonTagHelper(
                new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>(), Mock.Of<IHtmlGenerator>()))
            {
                Element = ButtonTagHelperElementType.Anchor,
                Disabled = true,
                Href = "https://place.com"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("a", output.TagName);
            Assert.Contains("govuk-button--disabled", output.Attributes["class"].Value.ToString().Split(' '));
        }

        [Fact]
        public async Task ProcessAsync_ValueSpecifiedAddsAttributesToOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ButtonTagHelper(
                new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>(), Mock.Of<IHtmlGenerator>()))
            {
                Element = ButtonTagHelperElementType.Button,
                Value = "some value"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("some value", output.Attributes["value"].Value);
        }

        [Fact]
        public async Task ProcessAsync_TypeSpecifiedAddsAttributesToOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ButtonTagHelper(
                new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>(), Mock.Of<IHtmlGenerator>()))
            {
                Element = ButtonTagHelperElementType.Button,
                Type = "submit"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("submit", output.Attributes["type"].Value);
        }

        [Fact]
        public async Task ProcessAsync_NameSpecifiedAddsAttributesToOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ButtonTagHelper(
                new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>(), Mock.Of<IHtmlGenerator>()))
            {
                Element = ButtonTagHelperElementType.Button,
                Name = "Some name"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Some name", output.Attributes["name"].Value);
        }

        [Fact]
        public async Task ProcessAsync_PreventDoubleClickSpecifiedAddsAttributesToOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ButtonTagHelper(
                new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>(), Mock.Of<IHtmlGenerator>()))
            {
                Element = ButtonTagHelperElementType.Button,
                PreventDoubleClick = true
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("true", output.Attributes["data-prevent-double-click"].Value);
        }

        [Fact]
        public async Task ProcessAsync_NameSpecifiedForAnchorElementThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ButtonTagHelper(
                new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>(), Mock.Of<IHtmlGenerator>()))
            {
                Element = ButtonTagHelperElementType.Anchor,
                Href = "https://place.com",
                Name = "A name"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Cannot specify the 'name' attribute for 'a' elements.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_TypeSpecifiedForAnchorElementThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ButtonTagHelper(
                new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>(), Mock.Of<IHtmlGenerator>()))
            {
                Element = ButtonTagHelperElementType.Anchor,
                Href = "https://place.com",
                Type = "submit"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Cannot specify the 'type' attribute for 'a' elements.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_ValueSpecifiedForAnchorElementThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ButtonTagHelper(
                new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>(), Mock.Of<IHtmlGenerator>()))
            {
                Element = ButtonTagHelperElementType.Anchor,
                Value = "Some value",
                Href = "https://place.com"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Cannot specify the 'value' attribute for 'a' elements.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_PreventDoubleClickSpecifiedForAnchorElementThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-button",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-button",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Button text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ButtonTagHelper(
                new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>(), Mock.Of<IHtmlGenerator>()))
            {
                Element = ButtonTagHelperElementType.Anchor,
                PreventDoubleClick = true,
                Href = "https://place.com"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Cannot specify the 'prevent-double-click' attribute for 'a' elements.", ex.Message);
        }

        public static IEnumerable<object[]> ProcessAsync_CorrectlyInfersElementTypeData { get; } =
            new[]
            {
                new object[] { "href", null, null, null, null, null, null, null, null, null, null },
                new object[] { null, "action", null, null, null, null, null, null, null, null, null },
                new object[] { null, null, "controller", null, null, null, null, null, null, null, null },
                new object[] { null, null, null, "area", null, null, null, null, null, null, null },
                new object[] { null, null, null, null, "fragment", null, null, null, null, null, null },
                new object[] { null, null, null, null, null, "host", null, null, null, null, null },
                new object[] { null, null, null, null, null, null, "page", null, null, null, null },
                new object[] { null, null, null, null, null, null, null, "pageHandler", null, null, null },
                new object[] { null, null, null, null, null, null, null, null, "protocol", null, null },
                new object[] { null, null, null, null, null, null, null, null, null, "route", null },
                new object[] { null, null, null, null, null, null, null, null, null, null, new Dictionary<string, string>() { { "controller", "controller" } } }
            };
    }
}
