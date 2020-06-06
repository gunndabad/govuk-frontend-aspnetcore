using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class DateInputTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-date-input",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-date-input",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[typeof(FormGroupBuilder)];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("The label"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new DateInputTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                DescribedBy = "describedby",
                Name = "my-name",
                Value = new Date(2020, 4, 1)
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            var node = HtmlNode.CreateNode(html);
            var container = node.ChildNodes.FindFirst("div");
            Assert.Equal(
                "<div class=\"govuk-date-input\" id=\"my-id\">" +
                "<div class=\"govuk-date-input__item\">" +
                "<div class=\"govuk-form-group\">" +
                "<label class=\"govuk-date-input__label govuk-label\" for=\"my-id.Day\">Day</label>" +
                "<input class=\"govuk-input--width-2 govuk-date-input__input govuk-input\" id=\"my-id.Day\" inputmode=\"numeric\" name=\"my-name.Day\" pattern=\"[0-9]*\" type=\"text\" value=\"1\">" +
                "</div>" +
                "</div>" +
                "<div class=\"govuk-date-input__item\">" +
                "<div class=\"govuk-form-group\">" +
                "<label class=\"govuk-date-input__label govuk-label\" for=\"my-id.Month\">Month</label>" +
                "<input class=\"govuk-input--width-2 govuk-date-input__input govuk-input\" id=\"my-id.Month\" inputmode=\"numeric\" name=\"my-name.Month\" pattern=\"[0-9]*\" type=\"text\" value=\"4\">" +
                "</div>" +
                "</div>" +
                "<div class=\"govuk-date-input__item\">" +
                "<div class=\"govuk-form-group\">" +
                "<label class=\"govuk-date-input__label govuk-label\" for=\"my-id.Year\">Year</label>" +
                "<input class=\"govuk-input--width-4 govuk-date-input__input govuk-input\" id=\"my-id.Year\" inputmode=\"numeric\" name=\"my-name.Year\" pattern=\"[0-9]*\" type=\"text\" value=\"2020\">" +
                "</div>" +
                "</div>" +
                "</div>",
                container.OuterHtml);
        }

        [Fact]
        public async Task ProcessAsync_NullValue_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-date-input",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-date-input",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[typeof(FormGroupBuilder)];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("The label"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new DateInputTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id-prefix",
                DescribedBy = "describedby",
                Name = "my-id",
                Value = null
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            var node = HtmlNode.CreateNode(html);
            var container = node.ChildNodes.FindFirst("div");

            var day = container.SelectNodes("//input").First();
            Assert.Equal("", day.Attributes["value"].Value);

            var month = container.SelectNodes("//input").Skip(1).First();
            Assert.Equal("", month.Attributes["value"].Value);

            var year = container.SelectNodes("//input").Skip(2).First();
            Assert.Equal("", year.Attributes["value"].Value);
        }

        [Fact]
        public async Task ProcessAsync_HasErrorClassWhenErrorSpecified()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-date-input",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-date-input",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[typeof(FormGroupBuilder)];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("The label"));
                    formGroupContext.TrySetErrorMessage(
                         visuallyHiddenText: null,
                         attributes: null,
                         content: new HtmlString("Error"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new DateInputTagHelper(new DefaultGovUkHtmlGenerator())
            {
                IdPrefix = "my-id",
                DescribedBy = "describedby",
                Name = "my-name",
                Value = new Date(2020, 4, 1)
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            var node = HtmlNode.CreateNode(html);
            var container = node.ChildNodes.FindFirst("div");

            var day = container.SelectNodes("//input").First();
            Assert.Contains("govuk-input--error", day.GetClasses());

            var month = container.SelectNodes("//input").Skip(1).First();
            Assert.Contains("govuk-input--error", month.GetClasses());

            var year = container.SelectNodes("//input").Skip(2).First();
            Assert.Contains("govuk-input--error", year.GetClasses());
        }
    }
}
