using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ModelBinding;
using GovUk.Frontend.AspNetCore.Tests.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.ModelBinding
{
    public class DateInputModelBinderTests
    {
        [Fact]
        public async Task BindModelAsync_AllComponentsEmpty_DoesNotBind()
        {
            // Arrange
            var modelType = typeof(Date);

            ModelBindingContext bindingContext = new DefaultModelBindingContext()
            {
                ActionContext = CreateActionContextWithServices(),
                ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType),
                ModelName = "TheModelName",
                ModelState = new ModelStateDictionary(),
                ValueProvider = new SimpleValueProvider()
            };

            var converterMock = new Mock<DateInputModelConverter>();
            converterMock.Setup(mock => mock.CanConvertModelType(modelType)).Returns(true);

            var modelBinder = new DateInputModelBinder(converterMock.Object);

            // Act
            await modelBinder.BindModelAsync(bindingContext);

            // Assert
            Assert.False(bindingContext.Result.IsModelSet);
        }

        [Fact]
        public async Task BindModelAsync_AllComponentsProvided_PassesValuesToConverterAndBindsResult()
        {
            // Arrange
            var modelType = typeof(Date);

            ModelBindingContext bindingContext = new DefaultModelBindingContext()
            {
                ActionContext = CreateActionContextWithServices(),
                ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType),
                ModelName = "TheModelName",
                ModelState = new ModelStateDictionary(),
                ValueProvider = new SimpleValueProvider()
                {
                    { "TheModelName.Day", "1" },
                    { "TheModelName.Month", "4" },
                    { "TheModelName.Year", "2020" }
                }
            };

            var converterMock = new Mock<DateInputModelConverter>();
            converterMock.Setup(mock => mock.CanConvertModelType(modelType)).Returns(true);

            converterMock
                .Setup(mock => mock.CreateModelFromElements(modelType, new ValueTuple<int, int, int>(1, 4, 2020)))
                .Returns(new Date(2020, 4, 1))
                .Verifiable();

            var modelBinder = new DateInputModelBinder(converterMock.Object);

            // Act
            await modelBinder.BindModelAsync(bindingContext);

            // Assert
            converterMock.Verify();

            Assert.True(bindingContext.Result.IsModelSet);

            var date = Assert.IsType<Date>(bindingContext.Result.Model);
            Assert.Equal(2020, date.Year);
            Assert.Equal(4, date.Month);
            Assert.Equal(1, date.Day);

            Assert.Null(bindingContext.ModelState["TheModelName.Day"]?.AttemptedValue);
            Assert.Null(bindingContext.ModelState["TheModelName.Month"]?.AttemptedValue);
            Assert.Null(bindingContext.ModelState["TheModelName.Year"]?.AttemptedValue);

            Assert.Equal(0, bindingContext.ModelState.ErrorCount);
        }

        [Theory]
        [InlineData("", "4", "2020")]
        [InlineData("1", "", "2020")]
        [InlineData("1", "4", "")]
        [InlineData("0", "4", "2020")]
        [InlineData("-1", "4", "2020")]
        [InlineData("32", "4", "2020")]
        [InlineData("1", "0", "2020")]
        [InlineData("1", "-1", "2020")]
        [InlineData("1", "13", "2020")]
        [InlineData("1", "4", "0")]
        [InlineData("1", "4", "-1")]
        [InlineData("1", "4", "10000")]
        public async Task BindModelAsync_MissingOrInvalidComponents_FailsBinding(string day, string month, string year)
        {
            // Arrange
            var modelType = typeof(Date);

            var valueProvider = new SimpleValueProvider();

            if (day != null)
            {
                valueProvider.Add("TheModelName.Day", day);
            }

            if (month != null)
            {
                valueProvider.Add("TheModelName.Month", month);
            }

            if (year != null)
            {
                valueProvider.Add("TheModelName.Year", year);
            }

            ModelBindingContext bindingContext = new DefaultModelBindingContext()
            {
                ActionContext = CreateActionContextWithServices(),
                ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType),
                ModelName = "TheModelName",
                ModelState = new ModelStateDictionary(),
                ValueProvider = valueProvider
            };

            var converterMock = new Mock<DateInputModelConverter>();
            converterMock.Setup(mock => mock.CanConvertModelType(modelType)).Returns(true);

            var modelBinder = new DateInputModelBinder(converterMock.Object);

            // Act
            await modelBinder.BindModelAsync(bindingContext);

            // Assert
            Assert.Equal(ModelBindingResult.Failed(), bindingContext.Result);

            Assert.Equal(day, bindingContext.ModelState["TheModelName.Day"].AttemptedValue);
            Assert.Equal(month, bindingContext.ModelState["TheModelName.Month"].AttemptedValue);
            Assert.Equal(year, bindingContext.ModelState["TheModelName.Year"].AttemptedValue);
        }

        [Fact]
        public async Task BindModelAsync_MissingOrInvalidComponentsAndConverterCanCreateModelFromErrors_PassesValuesToConverterAndBindsResult()
        {
            // Arrange
            var modelType = typeof(CustomDateType);

            var day = "1";
            var month = "4";
            var year = "-1";

            ModelBindingContext bindingContext = new DefaultModelBindingContext()
            {
                ActionContext = CreateActionContextWithServices(),
                ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType),
                ModelName = "TheModelName",
                ModelState = new ModelStateDictionary(),
                ValueProvider = new SimpleValueProvider()
                {
                    { "TheModelName.Day", day },
                    { "TheModelName.Month", month },
                    { "TheModelName.Year", year }
                }
            };

            var parseErrors = DateInputParseErrors.InvalidYear;
            object modelFromErrors = new CustomDateType() { ParseErrors = parseErrors };

            var converterMock = new Mock<DateInputModelConverter>();
            converterMock.Setup(mock => mock.CanConvertModelType(modelType)).Returns(true);

            converterMock
                .Setup(mock => mock.TryCreateModelFromErrors(modelType, parseErrors, out modelFromErrors))
                .Returns(true)
                .Verifiable();

            var modelBinder = new DateInputModelBinder(converterMock.Object);

            // Act
            await modelBinder.BindModelAsync(bindingContext);

            // Assert
            converterMock.Verify();

            Assert.True(bindingContext.Result.IsModelSet);

            Assert.Same(modelFromErrors, bindingContext.Result.Model);

            Assert.Equal(day, bindingContext.ModelState["TheModelName.Day"].AttemptedValue);
            Assert.Equal(month, bindingContext.ModelState["TheModelName.Month"].AttemptedValue);
            Assert.Equal(year, bindingContext.ModelState["TheModelName.Year"].AttemptedValue);

            Assert.Equal(0, bindingContext.ModelState.ErrorCount);
        }

        [Theory]
        [InlineData(DateInputParseErrors.MissingYear, "Date of birth must include a year")]
        [InlineData(DateInputParseErrors.InvalidYear, "Date of birth must be a real date")]
        [InlineData(DateInputParseErrors.MissingMonth, "Date of birth must include a month")]
        [InlineData(DateInputParseErrors.InvalidMonth, "Date of birth must be a real date")]
        [InlineData(DateInputParseErrors.InvalidDay, "Date of birth must be a real date")]
        [InlineData(DateInputParseErrors.MissingDay, "Date of birth must include a day")]
        [InlineData(DateInputParseErrors.MissingYear | DateInputParseErrors.MissingMonth, "Date of birth must include a month and year")]
        [InlineData(DateInputParseErrors.MissingYear | DateInputParseErrors.MissingDay, "Date of birth must include a day and year")]
        [InlineData(DateInputParseErrors.MissingMonth | DateInputParseErrors.MissingDay, "Date of birth must include a day and month")]
        [InlineData(DateInputParseErrors.InvalidYear | DateInputParseErrors.InvalidMonth, "Date of birth must be a real date")]
        [InlineData(DateInputParseErrors.InvalidYear | DateInputParseErrors.InvalidMonth | DateInputParseErrors.InvalidDay, "Date of birth must be a real date")]
        [InlineData(DateInputParseErrors.InvalidMonth | DateInputParseErrors.InvalidDay, "Date of birth must be a real date")]
        public void GetModelStateErrorMessage(DateInputParseErrors parseErrors, string expectedMessage)
        {
            // Arrange
            var modelMetadata = new DisplayNameModelMetadata("Date of birth");

            // Act
            var result = DateInputModelBinder.GetModelStateErrorMessage(parseErrors, modelMetadata);

            // Assert
            Assert.Equal(expectedMessage, result);
        }

        [Theory]
        [InlineData("", "4", "2020", DateInputParseErrors.MissingDay)]
        [InlineData("1", "", "2020", DateInputParseErrors.MissingMonth)]
        [InlineData("1", "4", "", DateInputParseErrors.MissingYear)]
        [InlineData("0", "4", "2020", DateInputParseErrors.InvalidDay)]
        [InlineData("-1", "4", "2020", DateInputParseErrors.InvalidDay)]
        [InlineData("32", "4", "2020", DateInputParseErrors.InvalidDay)]
        [InlineData("1", "0", "2020", DateInputParseErrors.InvalidMonth)]
        [InlineData("1", "-1", "2020", DateInputParseErrors.InvalidMonth)]
        [InlineData("1", "13", "2020", DateInputParseErrors.InvalidMonth)]
        [InlineData("1", "4", "0", DateInputParseErrors.InvalidYear)]
        [InlineData("1", "4", "-1", DateInputParseErrors.InvalidYear)]
        [InlineData("1", "4", "10000", DateInputParseErrors.InvalidYear)]
        public void Parse_InvalidDate_ComputesExpectedParseErrors(
            string day, string month, string year, DateInputParseErrors expectedParseErrors)
        {
            // Arrange

            // Act
            var result = DateInputModelBinder.Parse(day, month, year, out var dateComponents);

            // Assert
            Assert.Equal(default, dateComponents);
            Assert.Equal(expectedParseErrors, result);
        }

        private static ActionContext CreateActionContextWithServices()
        {
            var services = new ServiceCollection();
            services.AddScoped<DateInputParseErrorsProvider>();
            var serviceProvider = services.BuildServiceProvider();

            var httpContext = new DefaultHttpContext();
            httpContext.RequestServices = serviceProvider;

            return new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
        }

        private class DisplayNameModelMetadata : ModelMetadata
        {
            public DisplayNameModelMetadata(string displayName)
                : base(ModelMetadataIdentity.ForType(typeof(Date?)))
            {
                DisplayName = displayName;
            }

            public override IReadOnlyDictionary<object, object> AdditionalValues => throw new NotImplementedException();

            public override ModelPropertyCollection Properties => throw new NotImplementedException();

            public override string BinderModelName => throw new NotImplementedException();

            public override Type BinderType => throw new NotImplementedException();

            public override BindingSource BindingSource => throw new NotImplementedException();

            public override bool ConvertEmptyStringToNull => throw new NotImplementedException();

            public override string DataTypeName => throw new NotImplementedException();

            public override string Description => throw new NotImplementedException();

            public override string DisplayFormatString => throw new NotImplementedException();

            public override string DisplayName { get; }

            public override string EditFormatString => throw new NotImplementedException();

            public override ModelMetadata ElementMetadata => throw new NotImplementedException();

            public override IEnumerable<KeyValuePair<EnumGroupAndName, string>> EnumGroupedDisplayNamesAndValues => throw new NotImplementedException();

            public override IReadOnlyDictionary<string, string> EnumNamesAndValues => throw new NotImplementedException();

            public override bool HasNonDefaultEditFormat => throw new NotImplementedException();

            public override bool HtmlEncode => throw new NotImplementedException();

            public override bool HideSurroundingHtml => throw new NotImplementedException();

            public override bool IsBindingAllowed => throw new NotImplementedException();

            public override bool IsBindingRequired => throw new NotImplementedException();

            public override bool IsEnum => throw new NotImplementedException();

            public override bool IsFlagsEnum => throw new NotImplementedException();

            public override bool IsReadOnly => throw new NotImplementedException();

            public override bool IsRequired => throw new NotImplementedException();

            public override ModelBindingMessageProvider ModelBindingMessageProvider => throw new NotImplementedException();

            public override int Order => throw new NotImplementedException();

            public override string Placeholder => throw new NotImplementedException();

            public override string NullDisplayText => throw new NotImplementedException();

            public override IPropertyFilterProvider PropertyFilterProvider => throw new NotImplementedException();

            public override bool ShowForDisplay => throw new NotImplementedException();

            public override bool ShowForEdit => throw new NotImplementedException();

            public override string SimpleDisplayProperty => throw new NotImplementedException();

            public override string TemplateHint => throw new NotImplementedException();

            public override bool ValidateChildren => throw new NotImplementedException();

            public override IReadOnlyList<object> ValidatorMetadata => throw new NotImplementedException();

            public override Func<object, object> PropertyGetter => throw new NotImplementedException();

            public override Action<object, object> PropertySetter => throw new NotImplementedException();
        }

        private class CustomDateType
        {
            public DateInputParseErrors ParseErrors { get; set; }
        }
    }
}
