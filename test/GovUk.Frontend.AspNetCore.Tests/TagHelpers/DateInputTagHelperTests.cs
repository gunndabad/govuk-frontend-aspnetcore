using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
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

            var tagHelper = new DateInputTagHelper(new DefaultGovUkHtmlGenerator(), new DefaultModelHelper())
            {
                IdPrefix = "my-id",
                DescribedBy = "describedby",
                Name = "my-name",
                Value = new Date(2020, 4, 1)
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
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

            var tagHelper = new DateInputTagHelper(new DefaultGovUkHtmlGenerator(), new DefaultModelHelper())
            {
                IdPrefix = "my-id-prefix",
                DescribedBy = "describedby",
                Name = "my-id",
                Value = null
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            var node = HtmlNode.CreateNode(html);
            var container = node.ChildNodes.FindFirst("div");

            var day = container.SelectNodes("//input").First();
            var month = container.SelectNodes("//input").Skip(1).First();
            var year = container.SelectNodes("//input").Skip(2).First();

            Assert.Equal("", day.Attributes["value"].Value);
            Assert.Equal("", month.Attributes["value"].Value);
            Assert.Equal("", year.Attributes["value"].Value);
        }

        [Theory]
        [InlineData(null, true, true, true)]
        [InlineData(DateInputErrorItems.All, true, true, true)]
        [InlineData(DateInputErrorItems.Day, true, false, false)]
        [InlineData(DateInputErrorItems.Month, false, true, false)]
        [InlineData(DateInputErrorItems.Year, false, false, true)]
        [InlineData(DateInputErrorItems.Day | DateInputErrorItems.Month, true, true, false)]
        [InlineData(DateInputErrorItems.Day | DateInputErrorItems.Year, true, false, true)]
        [InlineData(DateInputErrorItems.Month | DateInputErrorItems.Year, false, true, true)]
        public async Task ProcessAsync_HaveErrorClassesWhenErrorSpecified(
            DateInputErrorItems? specifiedErrorItems,
            bool expectDayError,
            bool expectMonthError,
            bool expectYearError)
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

                    if (specifiedErrorItems != null)
                    {
                        var dateInputContext = (DateInputContext)context.Items[typeof(DateInputContext)];
                        dateInputContext.SetErrorItems(specifiedErrorItems.Value);
                    }

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new DateInputTagHelper(new DefaultGovUkHtmlGenerator(), new DefaultModelHelper())
            {
                IdPrefix = "my-id",
                DescribedBy = "describedby",
                Name = "my-name",
                Value = new Date(2020, 4, 1)
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            var node = HtmlNode.CreateNode(html);
            var container = node.ChildNodes.FindFirst("div");

            var day = container.SelectNodes("//input").First();
            var month = container.SelectNodes("//input").Skip(1).First();
            var year = container.SelectNodes("//input").Skip(2).First();

            AssertHaveErrorClass(day, expectDayError);
            AssertHaveErrorClass(month, expectMonthError);
            AssertHaveErrorClass(year, expectYearError);

            static void AssertHaveErrorClass(HtmlNode node, bool expectError)
            {
                if (expectError)
                {
                    Assert.Contains("govuk-input--error", node.GetClasses());
                }
                else
                {
                    Assert.DoesNotContain("govuk-input--error", node.GetClasses());
                }
            }
        }

        [Theory]
        [InlineData("Day is not valid.", null, null, true, false, false)]
        [InlineData(null, "Month is not valid.", null, false, true, false)]
        [InlineData(null, null, "Year is not valid.", false, false, true)]
        public async Task ProcessAsync_ErrorItemsNotSpecifiedAndErrorsFromModelBinder_InfersErrorItems(
            string dayModelError,
            string monthModelError,
            string yearModelError,
            bool expectDayError,
            bool expectMonthError,
            bool expectYearError)
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

            var htmlGenerator = new Mock<DefaultGovUkHtmlGenerator>()
            {
                CallBase = true
            };

            var modelExplorer = new EmptyModelMetadataProvider()
                .GetModelExplorerForType(typeof(Date), new Date(2020, 4, 1));

            var viewContext = new ViewContext();

            var errorComponents = (DateParseErrorComponents)0;

            if (dayModelError != null)
            {
                var dayModelExplorer = modelExplorer.GetExplorerForProperty("Day");
                viewContext.ModelState.AddModelError(".Day", dayModelError);
                errorComponents |= DateParseErrorComponents.Day;
            }

            if (monthModelError != null)
            {
                var monthModelExplorer = modelExplorer.GetExplorerForProperty("Month");
                viewContext.ModelState.AddModelError(".Month", monthModelError);
                errorComponents |= DateParseErrorComponents.Month;
            }

            if (yearModelError != null)
            {
                var yearModelExplorer = modelExplorer.GetExplorerForProperty("Year");
                viewContext.ModelState.AddModelError(".Year", yearModelError);
                errorComponents |= DateParseErrorComponents.Year;
            }

            if (errorComponents != 0)
            {
                viewContext.ModelState.AddModelError(
                    "",
                    new DateParseException("Invalid date.", errorComponents), modelExplorer.Metadata);
            }

            var tagHelper = new DateInputTagHelper(htmlGenerator.Object, new DefaultModelHelper())
            {
                AspFor = new ModelExpression("", modelExplorer),
                ViewContext = viewContext
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            var node = HtmlNode.CreateNode(html);
            var container = node.ChildNodes.FindFirst("div");

            var day = container.SelectNodes("//input").First();
            var month = container.SelectNodes("//input").Skip(1).First();
            var year = container.SelectNodes("//input").Skip(2).First();

            AssertHaveErrorClass(day, expectDayError);
            AssertHaveErrorClass(month, expectMonthError);
            AssertHaveErrorClass(year, expectYearError);

            static void AssertHaveErrorClass(HtmlNode node, bool expectError)
            {
                if (expectError)
                {
                    Assert.Contains("govuk-input--error", node.GetClasses());
                }
                else
                {
                    Assert.DoesNotContain("govuk-input--error", node.GetClasses());
                }
            }
        }

        [Theory]
        [InlineData(DateInputErrorItems.Month, "Day is not valid.", null, null, false, true, false)]
        [InlineData(DateInputErrorItems.Year, null, "Month is not valid.", null, false, false, true)]
        [InlineData(DateInputErrorItems.Day, null, null, "Year is not valid.", true, false, false)]
        public async Task ProcessAsync_ErrorItemsSpecifiedAndErrorsFromModelBinder_UsesSpecifiedErrorItems(
            DateInputErrorItems errorItems,
            string dayModelError,
            string monthModelError,
            string yearModelError,
            bool expectDayError,
            bool expectMonthError,
            bool expectYearError)
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

                    var dateInputContext = (DateInputContext)context.Items[typeof(DateInputContext)];
                    dateInputContext.SetErrorItems(errorItems);

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<DefaultGovUkHtmlGenerator>()
            {
                CallBase = true
            };

            var modelExplorer = new EmptyModelMetadataProvider()
                .GetModelExplorerForType(typeof(Date), new Date(2020, 4, 1));

            var viewContext = new ViewContext();

            if (dayModelError != null)
            {
                var dayModelExplorer = modelExplorer.GetExplorerForProperty("Day");
                viewContext.ModelState.AddModelError(".Day", dayModelError);
            }

            if (monthModelError != null)
            {
                var monthModelExplorer = modelExplorer.GetExplorerForProperty("Month");
                viewContext.ModelState.AddModelError(".Month", monthModelError);
            }

            if (yearModelError != null)
            {
                var yearModelExplorer = modelExplorer.GetExplorerForProperty("Year");
                viewContext.ModelState.AddModelError(".Year", yearModelError);
            }

            var tagHelper = new DateInputTagHelper(htmlGenerator.Object, new DefaultModelHelper())
            {
                AspFor = new ModelExpression("", modelExplorer),
                ViewContext = viewContext
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            var node = HtmlNode.CreateNode(html);
            var container = node.ChildNodes.FindFirst("div");

            var day = container.SelectNodes("//input").First();
            var month = container.SelectNodes("//input").Skip(1).First();
            var year = container.SelectNodes("//input").Skip(2).First();

            AssertHaveErrorClass(day, expectDayError);
            AssertHaveErrorClass(month, expectMonthError);
            AssertHaveErrorClass(year, expectYearError);

            static void AssertHaveErrorClass(HtmlNode node, bool expectError)
            {
                if (expectError)
                {
                    Assert.Contains("govuk-input--error", node.GetClasses());
                }
                else
                {
                    Assert.DoesNotContain("govuk-input--error", node.GetClasses());
                }
            }
        }
    }
}
