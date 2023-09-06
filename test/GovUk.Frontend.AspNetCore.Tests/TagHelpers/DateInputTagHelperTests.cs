using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.ModelBinding;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class DateInputTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_WithValue_GeneratesExpectedOutput()
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
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputTagHelper(Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            Id = "my-id",
            NamePrefix = "my-name",
            Value = new DateOnly(2020, 4, 1)
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-form-group"">
    <div class=""govuk-date-input"" id=""my-id"">
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Day"">Day</label>
                <input class=""govuk-input--width-2 govuk-date-input__input govuk-input"" id=""my-id.Day"" inputmode=""numeric"" name=""my-name.Day"" type=""text"" value=""1"">
            </div>
        </div>
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Month"">Month</label>
                <input class=""govuk-input--width-2 govuk-date-input__input govuk-input"" id=""my-id.Month"" inputmode=""numeric"" name=""my-name.Month"" type=""text"" value=""4"">
            </div>
        </div>
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Year"">Year</label>
                <input class=""govuk-input--width-4 govuk-date-input__input govuk-input"" id=""my-id.Year"" inputmode=""numeric"" name=""my-name.Year"" type=""text"" value=""2020"">
            </div>
        </div>
    </div>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithAspFor_GeneratesExpectedOutput()
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
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model() { Date = new DateOnly(2020, 4, 1) })
            .GetExplorerForProperty(nameof(Model.Date));

        var viewContext = new ViewContext();

        var tagHelper = new DateInputTagHelper(Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            AspFor = new ModelExpression(nameof(Model.Date), modelExplorer),
            Id = "my-id",
            NamePrefix = "my-name",
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-form-group"">
    <div class=""govuk-date-input"" id=""my-id"">
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Day"">Day</label>
                <input class=""govuk-input--width-2 govuk-date-input__input govuk-input"" id=""my-id.Day"" inputmode=""numeric"" name=""my-name.Day"" type=""text"" value=""1"">
            </div>
        </div>
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Month"">Month</label>
                <input class=""govuk-input--width-2 govuk-date-input__input govuk-input"" id=""my-id.Month"" inputmode=""numeric"" name=""my-name.Month"" type=""text"" value=""4"">
            </div>
        </div>
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Year"">Year</label>
                <input class=""govuk-input--width-4 govuk-date-input__input govuk-input"" id=""my-id.Year"" inputmode=""numeric"" name=""my-name.Year"" type=""text"" value=""2020"">
            </div>
        </div>
    </div>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithAspForAndValue_UsesValueAttribute()
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
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model() { Date = new DateOnly(2020, 4, 1) })
            .GetExplorerForProperty(nameof(Model.Date));

        var viewContext = new ViewContext();

        var tagHelper = new DateInputTagHelper(Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            AspFor = new ModelExpression(nameof(Model.Date), modelExplorer),
            Id = "my-id",
            NamePrefix = "my-name",
            ViewContext = viewContext,
            Value = new DateOnly(2022, 5, 3)
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-form-group"">
    <div class=""govuk-date-input"" id=""my-id"">
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Day"">Day</label>
                <input class=""govuk-input--width-2 govuk-date-input__input govuk-input"" id=""my-id.Day"" inputmode=""numeric"" name=""my-name.Day"" type=""text"" value=""3"">
            </div>
        </div>
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Month"">Month</label>
                <input class=""govuk-input--width-2 govuk-date-input__input govuk-input"" id=""my-id.Month"" inputmode=""numeric"" name=""my-name.Month"" type=""text"" value=""5"">
            </div>
        </div>
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Year"">Year</label>
                <input class=""govuk-input--width-4 govuk-date-input__input govuk-input"" id=""my-id.Year"" inputmode=""numeric"" name=""my-name.Year"" type=""text"" value=""2022"">
            </div>
        </div>
    </div>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithFieldset_GeneratesExpectedOutput()
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
                var dateInputContext = context.GetContextItem<DateInputContext>();

                dateInputContext.OpenFieldset();
                var dateInputFieldsetContext = new DateInputFieldsetContext(attributes: null, aspFor: null);
                dateInputFieldsetContext.SetLegend(isPageHeading: false, attributes: null, content: new HtmlString("Legend"));

                dateInputContext.CloseFieldset(dateInputFieldsetContext);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputTagHelper(Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            DescribedBy = "describedby",
            Id = "my-id",
            NamePrefix = "my-name",
            Value = new DateOnly(2020, 4, 1)
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-form-group"">
    <fieldset class=""govuk-fieldset"" role=""group"" aria-describedby=""describedby"">
        <legend class=""govuk-fieldset__legend"">
            Legend
        </legend>

        <div class=""govuk-date-input"" id=""my-id"">
            <div class=""govuk-date-input__item"">
                <div class=""govuk-form-group"">
                    <label class=""govuk-date-input__label govuk-label"" for=""my-id.Day"">Day</label>
                    <input class=""govuk-input--width-2 govuk-date-input__input govuk-input"" id=""my-id.Day"" inputmode=""numeric"" name=""my-name.Day"" type=""text"" value=""1"">
                </div>
            </div>
            <div class=""govuk-date-input__item"">
                <div class=""govuk-form-group"">
                    <label class=""govuk-date-input__label govuk-label"" for=""my-id.Month"">Month</label>
                    <input class=""govuk-input--width-2 govuk-date-input__input govuk-input"" id=""my-id.Month"" inputmode=""numeric"" name=""my-name.Month"" type=""text"" value=""4"">
                </div>
            </div>
            <div class=""govuk-date-input__item"">
                <div class=""govuk-form-group"">
                    <label class=""govuk-date-input__label govuk-label"" for=""my-id.Year"">Year</label>
                    <input class=""govuk-input--width-4 govuk-date-input__input govuk-input"" id=""my-id.Year"" inputmode=""numeric"" name=""my-name.Year"" type=""text"" value=""2020"">
                </div>
            </div>
        </div>
    </fieldset>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithCustomDateTypeInModel_GeneratesExpectedOutput()
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
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model() { CustomDate = new(2020, 4, 1) })
            .GetExplorerForProperty(nameof(Model.CustomDate));

        var viewContext = new ViewContext();

        var options = Options.Create(new GovUkFrontendAspNetCoreOptions()
        {
            DateInputModelConverters =
            {
                new CustomDateTypeConverter()
            }
        });

        var tagHelper = new DateInputTagHelper(options, new DateInputParseErrorsProvider())
        {
            AspFor = new ModelExpression(nameof(Model.CustomDate), modelExplorer),
            Id = "my-id",
            NamePrefix = "my-name",
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-form-group"">
    <div class=""govuk-date-input"" id=""my-id"">
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Day"">Day</label>
                <input class=""govuk-input--width-2 govuk-date-input__input govuk-input"" id=""my-id.Day"" inputmode=""numeric"" name=""my-name.Day"" type=""text"" value=""1"">
            </div>
        </div>
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Month"">Month</label>
                <input class=""govuk-input--width-2 govuk-date-input__input govuk-input"" id=""my-id.Month"" inputmode=""numeric"" name=""my-name.Month"" type=""text"" value=""4"">
            </div>
        </div>
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Year"">Year</label>
                <input class=""govuk-input--width-4 govuk-date-input__input govuk-input"" id=""my-id.Year"" inputmode=""numeric"" name=""my-name.Year"" type=""text"" value=""2020"">
            </div>
        </div>
    </div>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithHint_GeneratesExpectedOutput()
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
                var dateInputContext = context.GetContextItem<DateInputContext>();
                dateInputContext.SetHint(attributes: null, content: new HtmlString("The hint"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputTagHelper(Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            Id = "my-id",
            NamePrefix = "my-name",
            Value = new DateOnly(2020, 4, 1)
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-form-group"">
    <div class=""govuk-hint"" id=""my-id-hint"">The hint</div>
    <div class=""govuk-date-input"" id=""my-id"">
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Day"">Day</label>
                <input class=""govuk-input--width-2 govuk-date-input__input govuk-input"" id=""my-id.Day"" inputmode=""numeric"" name=""my-name.Day"" type=""text"" value=""1"">
            </div>
        </div>
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Month"">Month</label>
                <input class=""govuk-input--width-2 govuk-date-input__input govuk-input"" id=""my-id.Month"" inputmode=""numeric"" name=""my-name.Month"" type=""text"" value=""4"">
            </div>
        </div>
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Year"">Year</label>
                <input class=""govuk-input--width-4 govuk-date-input__input govuk-input"" id=""my-id.Year"" inputmode=""numeric"" name=""my-name.Year"" type=""text"" value=""2020"">
            </div>
        </div>
    </div>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithErrorMessage_GeneratesExpectedOutput()
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
                var dateInputContext = context.GetContextItem<DateInputContext>();
                dateInputContext.SetErrorMessage(errorComponents: null, visuallyHiddenText: null, attributes: null, content: new HtmlString("Error"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputTagHelper(Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            Id = "my-id",
            NamePrefix = "my-name",
            Value = new DateOnly(2020, 4, 1)
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-form-group govuk-form-group--error"">
    <p class=""govuk-error-message"" id=""my-id-error""><span class=""govuk-visually-hidden"">Error:</span>Error</p>
    <div class=""govuk-date-input"" id=""my-id"">
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Day"">Day</label>
                <input class=""govuk-input--width-2 govuk-date-input__input govuk-input govuk-input--error"" id=""my-id.Day"" inputmode=""numeric"" name=""my-name.Day"" type=""text"" value=""1"">
            </div>
        </div>
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Month"">Month</label>
                <input class=""govuk-input--width-2 govuk-date-input__input govuk-input govuk-input--error"" id=""my-id.Month"" inputmode=""numeric"" name=""my-name.Month"" type=""text"" value=""4"">
            </div>
        </div>
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Year"">Year</label>
                <input class=""govuk-input--width-4 govuk-date-input__input govuk-input govuk-input--error"" id=""my-id.Year"" inputmode=""numeric"" name=""my-name.Year"" type=""text"" value=""2020"">
            </div>
        </div>
    </div>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithHintAndErrorMessage_GeneratesExpectedOutput()
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
                var dateInputContext = context.GetContextItem<DateInputContext>();
                dateInputContext.SetHint(attributes: null, content: new HtmlString("The hint"));
                dateInputContext.SetErrorMessage(errorComponents: null, visuallyHiddenText: null, attributes: null, content: new HtmlString("Error"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputTagHelper(Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            Id = "my-id",
            NamePrefix = "my-name",
            Value = new DateOnly(2020, 4, 1)
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-form-group govuk-form-group--error"">
    <div class=""govuk-hint"" id=""my-id-hint"">The hint</div>
    <p class=""govuk-error-message"" id=""my-id-error""><span class=""govuk-visually-hidden"">Error:</span>Error</p>
    <div class=""govuk-date-input"" id=""my-id"">
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Day"">Day</label>
                <input class=""govuk-input--width-2 govuk-date-input__input govuk-input govuk-input--error"" id=""my-id.Day"" inputmode=""numeric"" name=""my-name.Day"" type=""text"" value=""1"">
            </div>
        </div>
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Month"">Month</label>
                <input class=""govuk-input--width-2 govuk-date-input__input govuk-input govuk-input--error"" id=""my-id.Month"" inputmode=""numeric"" name=""my-name.Month"" type=""text"" value=""4"">
            </div>
        </div>
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Year"">Year</label>
                <input class=""govuk-input--width-4 govuk-date-input__input govuk-input govuk-input--error"" id=""my-id.Year"" inputmode=""numeric"" name=""my-name.Year"" type=""text"" value=""2020"">
            </div>
        </div>
    </div>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithOverridenItemId_GeneratesExpectedItem()
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
                var dateInputContext = context.GetContextItem<DateInputContext>();
                dateInputContext.SetItem(DateInputItemType.Day, new DateInputContextItem()
                {
                    Id = "custom-id"
                });

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputTagHelper(Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            Id = "my-id",
            NamePrefix = "my-name",
            Value = new DateOnly(2020, 4, 1)
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var element = output.RenderToElement();
        var item = element.QuerySelectorAll(".govuk-date-input__item")[0];

        var expectedId = "custom-id";
        Assert.Equal(expectedId, item.QuerySelector("label").GetAttribute("for"));
        Assert.Equal(expectedId, item.QuerySelector("input").GetAttribute("id"));
    }

    [Fact]
    public async Task ProcessAsync_WithOverridenItemName_GeneratesExpectedItem()
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
                var dateInputContext = context.GetContextItem<DateInputContext>();
                dateInputContext.SetItem(DateInputItemType.Day, new DateInputContextItem()
                {
                    Name = "custom-name"
                });

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputTagHelper(Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            Id = "my-id",
            NamePrefix = "my-name",
            Value = new DateOnly(2020, 4, 1)
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var element = output.RenderToElement();
        var item = element.QuerySelectorAll(".govuk-date-input__item")[0];

        Assert.Equal("my-name.custom-name", item.QuerySelector("input").GetAttribute("name"));
    }

    [Fact]
    public async Task ProcessAsync_WithOverridenItemLabel_GeneratesExpectedItem()
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
                var dateInputContext = context.GetContextItem<DateInputContext>();
                dateInputContext.SetItem(DateInputItemType.Day, new DateInputContextItem()
                {
                    LabelContent = new HtmlString("Dydd")
                });

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputTagHelper(Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            Id = "my-id",
            NamePrefix = "my-name",
            Value = new DateOnly(2020, 4, 1)
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var element = output.RenderToElement();
        var item = element.QuerySelectorAll(".govuk-date-input__item")[0];

        Assert.Equal("Dydd", item.QuerySelector("label").InnerHtml.Trim());
    }

    [Fact]
    public async Task ProcessAsync_WithOverridenItemValue_GeneratesExpectedItem()
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
                var dateInputContext = context.GetContextItem<DateInputContext>();
                dateInputContext.SetItem(DateInputItemType.Day, new DateInputContextItem()
                {
                    Value = 28,
                    ValueSpecified = true
                });
                dateInputContext.SetItem(DateInputItemType.Month, new DateInputContextItem()
                {
                    Value = 5,
                    ValueSpecified = true
                });
                dateInputContext.SetItem(DateInputItemType.Year, new DateInputContextItem()
                {
                    Value = 2022,
                    ValueSpecified = true
                });

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputTagHelper(Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            Id = "my-id",
            NamePrefix = "my-name"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var element = output.RenderToElement();
        var item = element.QuerySelectorAll(".govuk-date-input__item")[0];

        Assert.Equal("28", item.QuerySelector("input").GetAttribute("value"));
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
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputTagHelper(Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            Id = "my-id",
            Value = null
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var html = output.RenderToElement();

        Assert.Collection(
            html.QuerySelectorAll("input").Cast<IHtmlInputElement>(),
            day => Assert.Equal("", day.Value),
            month => Assert.Equal("", month.Value),
            year => Assert.Equal("", year.Value));
    }

    [Fact]
    public async Task ProcessAsync_WithCustomDateTypeInValue_GeneratesExpectedOutput()
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
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var options = Options.Create(new GovUkFrontendAspNetCoreOptions()
        {
            DateInputModelConverters =
            {
                new CustomDateTypeConverter()
            }
        });

        var tagHelper = new DateInputTagHelper(options, new DateInputParseErrorsProvider())
        {
            Id = "my-id",
            NamePrefix = "my-name",
            Value = new CustomDateType(2020, 4, 1)
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-form-group"">
    <div class=""govuk-date-input"" id=""my-id"">
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Day"">Day</label>
                <input class=""govuk-input--width-2 govuk-date-input__input govuk-input"" id=""my-id.Day"" inputmode=""numeric"" name=""my-name.Day"" type=""text"" value=""1"">
            </div>
        </div>
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Month"">Month</label>
                <input class=""govuk-input--width-2 govuk-date-input__input govuk-input"" id=""my-id.Month"" inputmode=""numeric"" name=""my-name.Month"" type=""text"" value=""4"">
            </div>
        </div>
        <div class=""govuk-date-input__item"">
            <div class=""govuk-form-group"">
                <label class=""govuk-date-input__label govuk-label"" for=""my-id.Year"">Year</label>
                <input class=""govuk-input--width-4 govuk-date-input__input govuk-input"" id=""my-id.Year"" inputmode=""numeric"" name=""my-name.Year"" type=""text"" value=""2020"">
            </div>
        </div>
    </div>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Theory]
    [InlineData(null, true, true, true)]
    [InlineData(DateInputErrorComponents.All, true, true, true)]
    [InlineData(DateInputErrorComponents.Day, true, false, false)]
    [InlineData(DateInputErrorComponents.Month, false, true, false)]
    [InlineData(DateInputErrorComponents.Year, false, false, true)]
    [InlineData(DateInputErrorComponents.Day | DateInputErrorComponents.Month, true, true, false)]
    [InlineData(DateInputErrorComponents.Day | DateInputErrorComponents.Year, true, false, true)]
    [InlineData(DateInputErrorComponents.Month | DateInputErrorComponents.Year, false, true, true)]
    public async Task ProcessAsync_HaveErrorClassesWhenErrorSpecified(
        DateInputErrorComponents? specifiedErrorItems,
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
                var dateInputContext = context.GetContextItem<DateInputContext>();
                dateInputContext.SetErrorMessage(specifiedErrorItems, visuallyHiddenText: null, attributes: null, content: new HtmlString("Error"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputTagHelper(Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            Id = "my-id",
            DescribedBy = "describedby",
            NamePrefix = "my-name",
            Value = new DateOnly(2020, 4, 1)
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var element = output.RenderToElement();

        Assert.Collection(
            element.QuerySelectorAll("input"),
            day => AssertHaveErrorClass(day, expectDayError),
            month => AssertHaveErrorClass(month, expectMonthError),
            year => AssertHaveErrorClass(year, expectYearError));

        static void AssertHaveErrorClass(IElement element, bool expectError)
        {
            if (expectError)
            {
                Assert.Contains("govuk-input--error", element.ClassList);
            }
            else
            {
                Assert.DoesNotContain("govuk-input--error", element.ClassList);
            }
        }
    }

    [Theory]
    [InlineData(DateInputParseErrors.MissingDay, true, false, false)]
    [InlineData(DateInputParseErrors.MissingMonth, false, true, false)]
    [InlineData(DateInputParseErrors.MissingYear, false, false, true)]
    [InlineData(DateInputParseErrors.MissingDay | DateInputParseErrors.MissingMonth, true, true, false)]
    [InlineData(DateInputParseErrors.MissingDay | DateInputParseErrors.MissingYear, true, false, true)]
    [InlineData(DateInputParseErrors.MissingMonth | DateInputParseErrors.MissingYear, false, true, true)]
    [InlineData(DateInputParseErrors.MissingDay | DateInputParseErrors.MissingMonth | DateInputParseErrors.InvalidYear, true, true, true)]
    [InlineData(DateInputParseErrors.InvalidDay, true, false, false)]
    [InlineData(DateInputParseErrors.InvalidMonth, false, true, false)]
    [InlineData(DateInputParseErrors.InvalidYear, false, false, true)]
    [InlineData(DateInputParseErrors.InvalidDay | DateInputParseErrors.InvalidMonth, true, true, false)]
    [InlineData(DateInputParseErrors.InvalidDay | DateInputParseErrors.InvalidYear, true, false, true)]
    [InlineData(DateInputParseErrors.InvalidMonth | DateInputParseErrors.InvalidYear, false, true, true)]
    [InlineData(DateInputParseErrors.InvalidDay | DateInputParseErrors.MissingMonth | DateInputParseErrors.InvalidYear, true, true, true)]
    public async Task ProcessAsync_ErrorItemsNotSpecifiedAndErrorsFromModelBinder_InfersErrorItems(
        DateInputParseErrors parseErrors,
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
                var dateInputContext = context.GetContextItem<DateInputContext>();
                dateInputContext.SetErrorMessage(errorComponents: null, visuallyHiddenText: null, attributes: null, content: new HtmlString("Error"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model() { Date = new DateOnly(2020, 4, 1) })
            .GetExplorerForProperty(nameof(Model.Date));

        var viewContext = new ViewContext();

        var dateInputParseErrorsProvider = new DateInputParseErrorsProvider();

        SetModelErrors(nameof(Model.Date), parseErrors, dateInputParseErrorsProvider, viewContext);

        var tagHelper = new DateInputTagHelper(Options.Create(new GovUkFrontendAspNetCoreOptions()), dateInputParseErrorsProvider)
        {
            AspFor = new ModelExpression(nameof(Model.Date), modelExplorer),
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var element = output.RenderToElement();

        Assert.Collection(
            element.QuerySelectorAll("input"),
            day => AssertHaveErrorClass(day, expectDayError),
            month => AssertHaveErrorClass(month, expectMonthError),
            year => AssertHaveErrorClass(year, expectYearError));

        static void AssertHaveErrorClass(IElement element, bool expectError)
        {
            if (expectError)
            {
                Assert.Contains("govuk-input--error", element.ClassList);
            }
            else
            {
                Assert.DoesNotContain("govuk-input--error", element.ClassList);
            }
        }
    }

    [Fact]
    public async Task ProcessAsync_ErrorItemsSpecifiedAndErrorsFromModelBinder_UsesSpecifiedErrorItems()
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
                var dateInputContext = context.GetContextItem<DateInputContext>();

                // Explictly set a different set of DateInputErrorComponents than we have in DateInputParseErrorsProvider
                dateInputContext.SetErrorMessage(
                    errorComponents: DateInputErrorComponents.Month | DateInputErrorComponents.Year,
                    visuallyHiddenText: null,
                    attributes: null,
                    content: new HtmlString("Error"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model() { Date = new DateOnly(2020, 4, 1) })
            .GetExplorerForProperty(nameof(Model.Date));

        var viewContext = new ViewContext();

        var dateInputParseErrorsProvider = new DateInputParseErrorsProvider();

        SetModelErrors(nameof(Model.Date), DateInputParseErrors.InvalidDay, dateInputParseErrorsProvider, viewContext);

        var tagHelper = new DateInputTagHelper(Options.Create(new GovUkFrontendAspNetCoreOptions()), dateInputParseErrorsProvider)
        {
            AspFor = new ModelExpression(nameof(Model.Date), modelExplorer),
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var element = output.RenderToElement();

        Assert.Collection(
            element.QuerySelectorAll("input"),
            day => AssertHaveErrorClass(day, false),
            month => AssertHaveErrorClass(month, true),
            year => AssertHaveErrorClass(year, true));

        static void AssertHaveErrorClass(IElement element, bool expectError)
        {
            if (expectError)
            {
                Assert.Contains("govuk-input--error", element.ClassList);
            }
            else
            {
                Assert.DoesNotContain("govuk-input--error", element.ClassList);
            }
        }
    }

    [Theory]
    [InlineData(DateInputErrorComponents.Day, "my-id.Day")]
    [InlineData(DateInputErrorComponents.Day | DateInputErrorComponents.Month, "my-id.Day")]
    [InlineData(DateInputErrorComponents.Day | DateInputErrorComponents.Month | DateInputErrorComponents.Year, "my-id.Day")]
    [InlineData(DateInputErrorComponents.Month, "my-id.Month")]
    [InlineData(DateInputErrorComponents.Month | DateInputErrorComponents.Year, "my-id.Month")]
    [InlineData(null, "my-id.Day")]
    public async Task ProcessAsync_WithError_AddsErrorWithCorrectFieldIdToFormErrorContext(
        DateInputErrorComponents? errorComponents,
        string expectedErrorFieldId)
    {
        // Arrange
        var formErrorContext = new FormErrorContext();

        var context = new TagHelperContext(
            tagName: "govuk-date-input",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(FormErrorContext), formErrorContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-date-input",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var dateInputContext = context.GetContextItem<DateInputContext>();
                dateInputContext.SetErrorMessage(errorComponents, visuallyHiddenText: null, attributes: null, content: new HtmlString("Error"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputTagHelper(Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            Id = "my-id",
            DescribedBy = "describedby",
            NamePrefix = "my-name",
            Value = new DateOnly(2020, 4, 1)
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            formErrorContext.Errors,
            error =>
            {
                Assert.Equal("Error", error.Content?.ToHtmlString());
                Assert.Equal("#" + expectedErrorFieldId, error.Href);
            });
    }

    private static void SetModelErrors(
        string modelName,
        DateInputParseErrors dateInputParseErrors,
        DateInputParseErrorsProvider dateInputParseErrorsProvider,
        ViewContext viewContext,
        string? modelStateError = null)
    {
        dateInputParseErrorsProvider.SetErrorsForModel(modelName, dateInputParseErrors);
        viewContext.ModelState.AddModelError(modelName, modelStateError ?? $"{modelName} must be a real date.");
    }

    private class Model
    {
        public DateOnly? Date { get; set; }
        public CustomDateType? CustomDate { get; set; }
    }

    private class CustomDateType
    {
        public CustomDateType(int year, int month, int day)
        {
            Y = year;
            M = month;
            D = day;
        }

        public int D { get; }
        public int M { get; }
        public int Y { get; }
    }

    private class CustomDateTypeConverter : DateInputModelConverter
    {
        public override bool CanConvertModelType(Type modelType) => modelType == typeof(CustomDateType);

        public override object CreateModelFromDate(Type modelType, DateOnly date) => new CustomDateType(date.Year, date.Month, date.Day);

        public override DateOnly? GetDateFromModel(Type modelType, object model)
        {
            if (model is null)
            {
                return null;
            }

            var cdt = (CustomDateType)model;
            return new DateOnly(cdt.Y, cdt.M, cdt.D);
        }
    }
}
