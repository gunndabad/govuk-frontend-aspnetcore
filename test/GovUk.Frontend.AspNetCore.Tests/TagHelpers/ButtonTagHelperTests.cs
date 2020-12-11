using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
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
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>())
            {
                Href = "http://foo.com"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            Assert.Equal(
                "<a class=\"govuk-button\" data-module=\"govuk-button\" draggable=\"false\" href=\"http://foo.com\" role=\"button\">" +
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
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>());

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            Assert.Equal(
                "<button class=\"govuk-button\" data-module=\"govuk-button\" type=\"submit\">" +
                "Button text" +
                "</button>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_CorrectlyInfersButtonElementType()
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
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>())
            {
                Type = "button"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("button", output.TagName);
        }

        [Theory]
        [MemberData(nameof(ProcessAsync_CorrectlyInfersAnchorElementTypeData))]
        public async Task ProcessAsync_CorrectlyInfersAnchorElementType(
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

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            urlHelperFactory
                .Setup(mock => mock.GetUrlHelper(It.IsAny<ActionContext>()))
                .Returns((ActionContext actionContext) =>
                {
                    var urlHelper = new Mock<IUrlHelper>();

                    urlHelper.SetupGet(mock => mock.ActionContext).Returns(actionContext);

                    urlHelper
                        .Setup(mock => mock.Action(
                            /*actionContext: */It.IsAny<UrlActionContext>()))
                        .Returns("http://place.com");

                    urlHelper
                        .Setup(mock => mock.Link(
                            /*routeName: */It.IsAny<string>(),
                            /*values: */It.IsAny<object>()))
                        .Returns("http://place.com");

                    urlHelper
                        .Setup(mock => mock.RouteUrl(
                            /*routeContext: */It.IsAny<UrlRouteContext>()))
                        .Returns("http://place.com");

                    return urlHelper.Object;
                });

            var viewContext = new ViewContext()
            {
                RouteData = new Microsoft.AspNetCore.Routing.RouteData()
            };

            var tagHelper = new ButtonTagHelper(new DefaultGovUkHtmlGenerator(), urlHelperFactory.Object)
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
                RouteValues = routeValues,
                ViewContext = viewContext
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
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>())
            {
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
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>())
            {
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
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>())
            {
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
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>())
            {
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
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>())
            {
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
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>())
            {
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
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>())
            {
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
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>())
            {
                Href = "https://place.com",
                Name = "A name"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Cannot specify the 'name' attribute for 'a' elements.", ex.Message);
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
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>())
            {
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
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>())
            {
                PreventDoubleClick = true,
                Href = "https://place.com"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Cannot specify the 'prevent-double-click' attribute for 'a' elements.", ex.Message);
        }

        [Theory]
        [MemberData(nameof(ProcessAsync_AspLinkAttributesSpecifiedWithButtonElementTypeGeneratesFormActionAttributeData))]
        public async Task ProcessAsync_AspLinkAttributesSpecifiedWithButtonElementTypeGeneratesFormActionAttribute(
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

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            urlHelperFactory
                .Setup(mock => mock.GetUrlHelper(It.IsAny<ActionContext>()))
                .Returns((ActionContext actionContext) =>
                {
                    var urlHelper = new Mock<IUrlHelper>();

                    urlHelper.SetupGet(mock => mock.ActionContext).Returns(actionContext);

                    urlHelper
                        .Setup(mock => mock.Action(
                            /*actionContext: */It.IsAny<UrlActionContext>()))
                        .Returns("http://place.com");

                    urlHelper
                        .Setup(mock => mock.Link(
                            /*routeName: */It.IsAny<string>(),
                            /*values: */It.IsAny<object>()))
                        .Returns("http://place.com");

                    urlHelper
                        .Setup(mock => mock.RouteUrl(
                            /*routeContext: */It.IsAny<UrlRouteContext>()))
                        .Returns("http://place.com");

                    return urlHelper.Object;
                });

            var viewContext = new ViewContext()
            {
                RouteData = new Microsoft.AspNetCore.Routing.RouteData()
            };

            var tagHelper = new ButtonTagHelper(new DefaultGovUkHtmlGenerator(), urlHelperFactory.Object)
            {
                Action = action,
                Area = area,
                Controller = controller,
                Fragment = fragment,
                Host = host,
                Page = page,
                PageHandler = pageHandler,
                Protocol = protocol,
                Route = route,
                RouteValues = routeValues,
                Type = "button",
                ViewContext = viewContext
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("button", output.TagName);
            Assert.Equal("http://place.com", output.Attributes["formaction"].Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("button")]
        [InlineData("submit")]
        public async Task ProcessAsync_FormActionSpecifiedAddsAttributeToOutput(string type)
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
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>())
            {
                FormAction = "foo",
                Type = type
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("button", output.TagName);
            Assert.Equal("foo", output.Attributes["formaction"].Value);
        }

        public static IEnumerable<object[]> ProcessAsync_CorrectlyInfersAnchorElementTypeData { get; } =
            new[]
            {
                new object[] { "href", null, null, null, null, null, null, null, null, null, null },
                new object[] { null, "action", null, null, null, null, null, null, null, null, null },
                new object[] { null, null, "controller", null, null, null, null, null, null, null, null },
                new object[] { null, null, null, "area", null, null, null, null, null, null, null },
                new object[] { null, null, null, null, "fragment", null, null, null, null, null, null },
                new object[] { null, null, null, null, null, "host", null, null, null, null, null },
                // FIXME
                //new object[] { null, null, null, null, null, null, "page", null, null, null, null },
                new object[] { null, null, null, null, null, null, null, "pageHandler", null, null, null },
                new object[] { null, null, null, null, null, null, null, null, "protocol", null, null },
                new object[] { null, null, null, null, null, null, null, null, null, "route", null },
                new object[] { null, null, null, null, null, null, null, null, null, null, new Dictionary<string, string>() { { "controller", "controller" } } }
            };

        public static IEnumerable<object[]> ProcessAsync_AspLinkAttributesSpecifiedWithButtonElementTypeGeneratesFormActionAttributeData { get; } =
            new[]
            {
                new object[] { "action", null, null, null, null, null, null, null, null, null },
                new object[] { null, "controller", null, null, null, null, null, null, null, null },
                new object[] { null, null, "area", null, null, null, null, null, null, null },
                new object[] { null, null, null, "fragment", null, null, null, null, null, null },
                new object[] { null, null, null, null, "host", null, null, null, null, null },
                // FIXME
                //new object[] { null, null, null, null, null, "page", null, null, null, null },
                new object[] { null, null, null, null, null, null, "pageHandler", null, null, null },
                new object[] { null, null, null, null, null, null, null, "protocol", null, null },
                new object[] { null, null, null, null, null, null, null, null, "route", null },
                new object[] { null, null, null, null, null, null, null, null, null, new Dictionary<string, string>() { { "controller", "controller" } } }
            };
    }
}
