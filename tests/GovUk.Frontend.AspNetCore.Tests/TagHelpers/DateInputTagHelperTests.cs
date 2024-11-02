using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Xml.Linq;
using AngleSharp.Common;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.ModelBinding;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class DateInputTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var id = "my-id";
        var namePrefix = "my-name";
        var value = new DateOnly(2020, 4, 1);
        var classes = "custom-class";
        var dataFooAttrValue = "bar";
        var hintHtml = "The hint";

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

                dateInputContext.SetHint(
                    attributes: ImmutableDictionary<string, string?>.Empty,
                    hintHtml);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        DateInputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateDateInput(It.IsAny<DateInputOptions>())).Callback<DateInputOptions>(o => actualOptions = o);

        var tagHelper = new DateInputTagHelper(componentGeneratorMock.Object, Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            Id = id,
            NamePrefix = namePrefix,
            Value = value,
            DateInputAttributes = new Dictionary<string, string?>()
            {
                { "class", classes },
                { "data-foo", dataFooAttrValue },
            }
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(id, actualOptions!.Id);
        Assert.Null(actualOptions.NamePrefix);
        Assert.NotNull(actualOptions.Items);
        Assert.Collection(
            actualOptions.Items,
            item =>
            {
                Assert.Equal(namePrefix + ".Day", item.Name);
                Assert.Equal(id + "-day", item.Id);
                Assert.Equal(value.Day.ToString(), item.Value);
            },
            item =>
            {
                Assert.Equal(namePrefix + ".Month", item.Name);
                Assert.Equal(id + "-month", item.Id);
                Assert.Equal(value.Month.ToString(), item.Value);
            },
            item =>
            {
                Assert.Equal(namePrefix + ".Year", item.Name);
                Assert.Equal(id + "-year", item.Id);
                Assert.Equal(value.Year.ToString(), item.Value);
            });
        Assert.Equal(hintHtml, actualOptions.Hint?.Html);
        Assert.Null(actualOptions.ErrorMessage);
        Assert.Null(actualOptions.Fieldset);
        Assert.Equal(classes, actualOptions.Classes);
        Assert.NotNull(actualOptions.Attributes);
        Assert.Collection(actualOptions.Attributes, kvp =>
        {
            Assert.Equal("data-foo", kvp.Key);
            Assert.Equal(dataFooAttrValue, kvp.Value);
        });
    }

    [Fact]
    public async Task ProcessAsync_WithErrorMessage_GeneratesOptionsWithErrorMessageAndAddsErrorClasses()
    {
        // Arrange
        var id = "my-id";
        var namePrefix = "my-name";
        var value = new DateOnly(2020, 4, 1);
        var errorHtml = "The error message";
        var errorVht = "visually hidden text";
        var errorDataFooAttribute = "bar";

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

                dateInputContext.SetErrorMessage(
                    DateInputErrorFields.All,
                    visuallyHiddenText: errorVht,
                    attributes: ImmutableDictionary<string, string?>.Empty.Add("data-foo", errorDataFooAttribute),
                    errorHtml);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        DateInputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateDateInput(It.IsAny<DateInputOptions>())).Callback<DateInputOptions>(o => actualOptions = o);

        var tagHelper = new DateInputTagHelper(componentGeneratorMock.Object, Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            Id = id,
            NamePrefix = namePrefix,
            Value = value
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions?.ErrorMessage);
        Assert.Equal(errorHtml, actualOptions.ErrorMessage.Html);
        Assert.Equal(errorVht, actualOptions.ErrorMessage.VisuallyHiddenText);
        Assert.NotNull(actualOptions.ErrorMessage.Attributes);
        Assert.Collection(actualOptions.ErrorMessage.Attributes, kvp =>
        {
            Assert.Equal("data-foo", kvp.Key);
            Assert.Equal(errorDataFooAttribute, kvp.Value);
        });
        Assert.NotNull(actualOptions.Items);
        Assert.Collection(
            actualOptions.Items,
            AssertItemHasErrorClass,
            AssertItemHasErrorClass,
            AssertItemHasErrorClass
        );
        Assert.NotNull(actualOptions.FormGroup?.Classes);
        Assert.Contains("govuk-form-group--error", actualOptions.FormGroup.Classes.Split(' '));

        static void AssertItemHasErrorClass(DateInputOptionsItem item)
        {
            Assert.NotNull(item.Classes);
            Assert.Contains("govuk-input--error", item.Classes.Split(' '));
        }
    }

    [Fact]
    public async Task ProcessAsync_WithFieldset_GeneratesOptionsWithFieldset()
    {
        // Arrange
        var id = "my-id";
        var namePrefix = "my-name";
        var value = new DateOnly(2020, 4, 1);
        var legendIsPageHeading = true;
        var legendHtml = "The legend";

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
                var fieldsetContext = new DateInputFieldsetContext(attributes: ImmutableDictionary<string, string?>.Empty, @for: null, describedBy: null);
                fieldsetContext.SetLegend(legendIsPageHeading, attributes: ImmutableDictionary<string, string?>.Empty, legendHtml);
                dateInputContext.CloseFieldset(fieldsetContext);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        DateInputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateDateInput(It.IsAny<DateInputOptions>())).Callback<DateInputOptions>(o => actualOptions = o);

        var tagHelper = new DateInputTagHelper(componentGeneratorMock.Object, Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            Id = id,
            NamePrefix = namePrefix,
            Value = value
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions?.Fieldset?.Legend);
        Assert.Equal(legendIsPageHeading, actualOptions.Fieldset.Legend.IsPageHeading);
        Assert.Equal(legendHtml, actualOptions.Fieldset.Legend.Html);
    }

    [Fact]
    public async Task ProcessAsync_WithFor_GeneratesOptionsFromModelMetadata()
    {
        // Arrange
        var modelStateValue = new DateOnly(2020, 4, 1);
        var displayName = "Label";
        var description = "Description";
        var modelStateError = "An error";

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model() { Date = modelStateValue })
            .GetExplorerForProperty(nameof(Model.Date));

        var @for = new ModelExpression(nameof(Model.Date), modelExplorer);

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
                dateInputContext.CloseFieldset(new(attributes: ImmutableDictionary<string, string?>.Empty, @for, describedBy: null));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        modelHelperMock
            .Setup(mock => mock.GetFullHtmlFieldName(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*expression: */It.IsAny<string>()))
            .Returns(nameof(Model.Date));

        modelHelperMock
            .Setup(mock => mock.GetDisplayName(
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(displayName);

        modelHelperMock
            .Setup(mock => mock.GetDescription(/*modelExplorer: */It.IsAny<ModelExplorer>()))
            .Returns(description);

        modelHelperMock
            .Setup(mock => mock.GetValidationMessage(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(modelStateError);

        modelHelperMock
            .Setup(mock => mock.GetModelValue(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */ "Date"))
            .Returns(modelStateValue.ToString());

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        DateInputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateDateInput(It.IsAny<DateInputOptions>())).Callback<DateInputOptions>(o => actualOptions = o);

        var tagHelper = new DateInputTagHelper(componentGeneratorMock.Object, Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider(), modelHelperMock.Object)
        {
            For = new ModelExpression(nameof(Model.Date), modelExplorer),
            ViewContext = new ViewContext(),
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions?.Fieldset?.Legend);
        Assert.Equal(HtmlEncoder.Default.Encode(displayName), actualOptions.Fieldset.Legend.Html);
        Assert.NotNull(actualOptions.Hint);
        Assert.Equal(HtmlEncoder.Default.Encode(description), actualOptions.Hint.Html);
        Assert.NotNull(actualOptions.ErrorMessage);
        Assert.Equal(HtmlEncoder.Default.Encode(modelStateError), actualOptions.ErrorMessage.Html);
        var items = actualOptions.Items?.ToArray();
        Assert.NotNull(items);
        Assert.Equal(modelStateValue.Day.ToString(), items[0].Value);
        Assert.Equal(modelStateValue.Month.ToString(), items[1].Value);
        Assert.Equal(modelStateValue.Year.ToString(), items[2].Value);
    }

    [Fact]
    public async Task ProcessAsync_WithForAndExplicitHint_UsesSpecifiedHint()
    {
        // Arrange
        var modelStateValue = new DateOnly(2020, 4, 1);
        var displayName = "Label";
        var modelStateDescription = "ModelState description";
        var hintHtml = "Explicit hint";

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model() { Date = modelStateValue })
            .GetExplorerForProperty(nameof(Model.Date));

        var @for = new ModelExpression(nameof(Model.Date), modelExplorer);

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

                dateInputContext.SetHint(attributes: ImmutableDictionary<string, string?>.Empty, html: hintHtml);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        modelHelperMock
            .Setup(mock => mock.GetFullHtmlFieldName(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*expression: */It.IsAny<string>()))
            .Returns(nameof(Model.Date));

        modelHelperMock
            .Setup(mock => mock.GetDisplayName(
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(displayName);

        modelHelperMock
            .Setup(mock => mock.GetDescription(/*modelExplorer: */It.IsAny<ModelExplorer>()))
            .Returns(modelStateDescription);

        modelHelperMock
            .Setup(mock => mock.GetModelValue(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */ "Date"))
            .Returns(modelStateValue.ToString());

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        DateInputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateDateInput(It.IsAny<DateInputOptions>())).Callback<DateInputOptions>(o => actualOptions = o);

        var tagHelper = new DateInputTagHelper(componentGeneratorMock.Object, Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider(), modelHelperMock.Object)
        {
            For = new ModelExpression(nameof(Model.Date), modelExplorer),
            ViewContext = new ViewContext(),
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(hintHtml, actualOptions?.Hint?.Html);
    }

    [Fact]
    public async Task ProcessAsync_WithForAndExplicitErrorMessage_UsesSpecifiedErrorMessage()
    {
        // Arrange
        var modelStateValue = new DateOnly(2020, 4, 1);
        var displayName = "Label";
        var modelStateError = "ModelState error";
        var errorHtml = "Explicit error";

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model() { Date = modelStateValue })
            .GetExplorerForProperty(nameof(Model.Date));

        var @for = new ModelExpression(nameof(Model.Date), modelExplorer);

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

                dateInputContext.SetErrorMessage(errorFields: null, visuallyHiddenText: null, attributes: ImmutableDictionary<string, string?>.Empty, html: errorHtml);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        modelHelperMock
            .Setup(mock => mock.GetFullHtmlFieldName(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*expression: */It.IsAny<string>()))
            .Returns(nameof(Model.Date));

        modelHelperMock
            .Setup(mock => mock.GetDisplayName(
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(displayName);

        modelHelperMock
            .Setup(mock => mock.GetValidationMessage(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(modelStateError);

        modelHelperMock
            .Setup(mock => mock.GetModelValue(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */ "Date"))
            .Returns(modelStateValue.ToString());

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        DateInputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateDateInput(It.IsAny<DateInputOptions>())).Callback<DateInputOptions>(o => actualOptions = o);

        var tagHelper = new DateInputTagHelper(componentGeneratorMock.Object, Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider(), modelHelperMock.Object)
        {
            For = new ModelExpression(nameof(Model.Date), modelExplorer),
            ViewContext = new ViewContext(),
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(errorHtml, actualOptions?.ErrorMessage?.Html);
    }

    [Fact]
    public async Task ProcessAsync_WithForAndExplicitFieldsetLegend_UsesSpecifiedLegend()
    {
        // Arrange
        var modelStateValue = new DateOnly(2020, 4, 1);
        var displayName = "Label";
        var modelStateDescription = "ModelState description";
        var fieldsetLegendHtml = "Explicit legend";

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model() { Date = modelStateValue })
            .GetExplorerForProperty(nameof(Model.Date));

        var @for = new ModelExpression(nameof(Model.Date), modelExplorer);

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
                var dateInputFieldsetContext = new DateInputFieldsetContext(attributes: ImmutableDictionary<string, string?>.Empty, @for, describedBy: null);
                dateInputFieldsetContext.SetLegend(isPageHeading: false, attributes: ImmutableDictionary<string, string?>.Empty, html: fieldsetLegendHtml);
                dateInputContext.CloseFieldset(dateInputFieldsetContext);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        modelHelperMock
            .Setup(mock => mock.GetFullHtmlFieldName(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*expression: */It.IsAny<string>()))
            .Returns(nameof(Model.Date));

        modelHelperMock
            .Setup(mock => mock.GetDisplayName(
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(displayName);

        modelHelperMock
            .Setup(mock => mock.GetDescription(/*modelExplorer: */It.IsAny<ModelExplorer>()))
            .Returns(modelStateDescription);

        modelHelperMock
            .Setup(mock => mock.GetModelValue(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */ "Date"))
            .Returns(modelStateValue.ToString());

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        DateInputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateDateInput(It.IsAny<DateInputOptions>())).Callback<DateInputOptions>(o => actualOptions = o);

        var tagHelper = new DateInputTagHelper(componentGeneratorMock.Object, Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider(), modelHelperMock.Object)
        {
            For = new ModelExpression(nameof(Model.Date), modelExplorer),
            ViewContext = new ViewContext(),
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(fieldsetLegendHtml, actualOptions?.Fieldset?.Legend?.Html);
    }

    [Fact]
    public async Task ProcessAsync_WithDisabledSetToTrue_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var id = "my-id";
        var namePrefix = "my-name";
        var value = new DateOnly(2020, 4, 1);

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

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        DateInputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateDateInput(It.IsAny<DateInputOptions>())).Callback<DateInputOptions>(o => actualOptions = o);

        var tagHelper = new DateInputTagHelper(componentGeneratorMock.Object, Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            Id = id,
            NamePrefix = namePrefix,
            Value = value,
            Disabled = true
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.NotNull(actualOptions.Items);
        Assert.Collection(
            actualOptions.Items,
            AssetItemOptionHasDisabled,
            AssetItemOptionHasDisabled,
            AssetItemOptionHasDisabled);

        static void AssetItemOptionHasDisabled(DateInputOptionsItem item)
        {
            Assert.NotNull(item.Attributes);
            Assert.Contains(item.Attributes, kvp => kvp.Key == "disabled");
        }
    }

    //ProcessAsync_WithExplicitItems_GeneratesExpectedItems
    //ProcessAsync_WithPartialExplicitItems_GeneratesExpectedItems
    //ProcessAsync_WithForAndExplicitValue_UsesValueAttribute
    //ProcessAsync_WithExplicitItemId_GeneratesOptionsWithSpecifiedItemId
    //ProcessAsync_WithExplicitItemName_GeneratesOptionsWithSpecifiedItemName
    //ProcessAsync_WithExplicitItemValue_GeneratesOptionsWithSpecifiedItemValue

    [Theory]
    [InlineData(null, true, true, true)]
    [InlineData(DateInputErrorFields.All, true, true, true)]
    [InlineData(DateInputErrorFields.Day, true, false, false)]
    [InlineData(DateInputErrorFields.Month, false, true, false)]
    [InlineData(DateInputErrorFields.Year, false, false, true)]
    [InlineData(DateInputErrorFields.Day | DateInputErrorFields.Month, true, true, false)]
    [InlineData(DateInputErrorFields.Day | DateInputErrorFields.Year, true, false, true)]
    [InlineData(DateInputErrorFields.Month | DateInputErrorFields.Year, false, true, true)]
    public async Task ProcessAsync_WithExplicitErrorFields_GeneratesExpectedItemClasses(
        DateInputErrorFields? specifiedErrorItems,
        bool expectDayError,
        bool expectMonthError,
        bool expectYearError)
    {
        // Arrange
        var id = "my-id";
        var namePrefix = "my-name";
        var value = new DateOnly(2020, 4, 1);

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

                dateInputContext.SetErrorMessage(
                    specifiedErrorItems,
                    visuallyHiddenText: null,
                    attributes: ImmutableDictionary<string, string?>.Empty,
                    html: HtmlEncoder.Default.Encode("Error message"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        DateInputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateDateInput(It.IsAny<DateInputOptions>())).Callback<DateInputOptions>(o => actualOptions = o);

        var tagHelper = new DateInputTagHelper(componentGeneratorMock.Object, Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            Id = id,
            NamePrefix = namePrefix,
            Value = value
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions?.Items);
        Assert.Collection(
            actualOptions.Items,
            item => AssertHaveErrorClass(item, expectDayError),
            item => AssertHaveErrorClass(item, expectMonthError),
            item => AssertHaveErrorClass(item, expectYearError));

        static void AssertHaveErrorClass(DateInputOptionsItem item, bool expectError)
        {
            var classes = item.Classes?.Split(' ') ?? [];

            if (expectError)
            {
                Assert.Contains("govuk-input--error", classes);
            }
            else
            {
                Assert.DoesNotContain("govuk-input--error", classes);
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
    public async Task ProcessAsync_WithForButErrorFieldsNotSpecified_GetsErrorItemsFromModelBinder(
        DateInputParseErrors parseErrors,
        bool expectDayError,
        bool expectMonthError,
        bool expectYearError)
    {
        // Arrange
        var modelStateValue = new DateOnly(2020, 4, 1);

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

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        DateInputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateDateInput(It.IsAny<DateInputOptions>())).Callback<DateInputOptions>(o => actualOptions = o);

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model() { Date = modelStateValue })
            .GetExplorerForProperty(nameof(Model.Date));

        var @for = new ModelExpression(nameof(Model.Date), modelExplorer);

        var viewContext = new ViewContext();

        var dateInputParseErrorsProvider = new DateInputParseErrorsProvider();
        SetModelErrors(nameof(Model.Date), parseErrors, dateInputParseErrorsProvider, viewContext);

        var tagHelper = new DateInputTagHelper(componentGeneratorMock.Object, Options.Create(new GovUkFrontendAspNetCoreOptions()), dateInputParseErrorsProvider)
        {
            For = @for,
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions?.Items);
        Assert.Collection(
            actualOptions.Items,
            item => AssertHaveErrorClass(item, expectDayError),
            item => AssertHaveErrorClass(item, expectMonthError),
            item => AssertHaveErrorClass(item, expectYearError));

        static void AssertHaveErrorClass(DateInputOptionsItem item, bool expectError)
        {
            var classes = item.Classes?.Split(' ') ?? [];

            if (expectError)
            {
                Assert.Contains("govuk-input--error", classes);
            }
            else
            {
                Assert.DoesNotContain("govuk-input--error", classes);
            }
        }
    }

    [Theory]
    [InlineData(DateInputErrorFields.Day, "-day")]
    [InlineData(DateInputErrorFields.Day | DateInputErrorFields.Month, "-day")]
    [InlineData(DateInputErrorFields.Day | DateInputErrorFields.Month | DateInputErrorFields.Year, "-day")]
    [InlineData(DateInputErrorFields.Month, "-month")]
    [InlineData(DateInputErrorFields.Month | DateInputErrorFields.Year, "-month")]
    [InlineData(DateInputErrorFields.Year, "-year")]
    public async Task ProcessAsync_WithError_AddsErrorWithCorrectFieldIdToFormErrorContext(
        DateInputErrorFields errorFields,
        string expectedErrorHrefSuffix)
    {
        // Arrange
        var containerErrorContext = new ContainerErrorContext();

        var id = "my-id";
        var namePrefix = "my-name";
        var value = new DateOnly(2020, 4, 1);
        var errorHtml = "Error message";

        var context = new TagHelperContext(
            tagName: "govuk-date-input",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(ContainerErrorContext), containerErrorContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-date-input",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var dateInputContext = context.GetContextItem<DateInputContext>();

                dateInputContext.SetErrorMessage(
                    errorFields,
                    visuallyHiddenText: null,
                    attributes: ImmutableDictionary<string, string?>.Empty,
                    html: errorHtml);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        DateInputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateDateInput(It.IsAny<DateInputOptions>())).Callback<DateInputOptions>(o => actualOptions = o);

        var tagHelper = new DateInputTagHelper(componentGeneratorMock.Object, Options.Create(new GovUkFrontendAspNetCoreOptions()), new DateInputParseErrorsProvider())
        {
            Id = id,
            NamePrefix = namePrefix,
            Value = value
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            containerErrorContext.Errors,
            error =>
            {
                Assert.Equal(errorHtml, error.Html);
                Assert.Equal($"#{id}{expectedErrorHrefSuffix}", error.Href);
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
    }
}
