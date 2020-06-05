using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.ModelBinding
{
    public class DateInputModelBinderTests
    {
        [Fact]
        public async Task NullableModelTypeAllComponentsEmpty_BindsNull()
        {
            // Arrange
            var modelType = typeof(Date?);

            ModelBindingContext bindingContext = new DefaultModelBindingContext()
            {
                ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType),
                ModelName = "TheModelName",
                ModelState = new ModelStateDictionary(),
                ValueProvider = new SimpleValueProvider()
            };

            var modelBinder = new DateInputModelBinder();

            // Act
            await modelBinder.BindModelAsync(bindingContext);

            // Assert
            Assert.True(bindingContext.Result.IsModelSet);
            Assert.Null(bindingContext.Result.Model);
        }

        [Fact]
        public async Task NonNullableModelTypeAllComponentsEmpty_FailsBinding()
        {
            // Arrange
            var modelType = typeof(Date);

            ModelBindingContext bindingContext = new DefaultModelBindingContext()
            {
                ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType),
                ModelName = "TheModelName",
                ModelState = new ModelStateDictionary(),
                ValueProvider = new SimpleValueProvider()
            };

            var modelBinder = new DateInputModelBinder();

            // Act
            await modelBinder.BindModelAsync(bindingContext);

            // Assert
            Assert.Equal(ModelBindingResult.Failed(), bindingContext.Result);
            Assert.Null(bindingContext.ModelState["TheModelName"]?.Errors);
        }

        [Theory]
        [InlineData(typeof(Date))]
        [InlineData(typeof(Date?))]
        public async Task AllComponentsProvider_BindsSuccessfully(Type modelType)
        {
            // Arrange
            ModelBindingContext bindingContext = new DefaultModelBindingContext()
            {
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

            var modelBinder = new DateInputModelBinder();

            // Act
            await modelBinder.BindModelAsync(bindingContext);

            // Assert
            Assert.True(bindingContext.Result.IsModelSet);
            Assert.IsType<Date>(bindingContext.Result.Model);

            Date date = (Date)bindingContext.Result.Model;
            Assert.Equal(2020, date.Year);
            Assert.Equal(4, date.Month);
            Assert.Equal(1, date.Day);
        }

        [Theory]
        [InlineData("", "4", "2020", "Day is missing.", null, null)]
        [InlineData("1", "", "2020", null, "Month is missing.", null)]
        [InlineData("1", "4", "", null, null, "Year is missing.")]
        [InlineData("0", "4", "2020", "Day is not valid.", null, null)]
        [InlineData("-1", "4", "2020", "Day is not valid.", null, null)]
        [InlineData("32", "4", "2020", "Day is not valid.", null, null)]
        [InlineData("1", "0", "2020", null, "Month is not valid.", null)]
        [InlineData("1", "-1", "2020", null, "Month is not valid.", null)]
        [InlineData("1", "13", "2020", null, "Month is not valid.", null)]
        [InlineData("1", "4", "0", null, null, "Year is not valid.")]
        [InlineData("1", "4", "-1", null, null, "Year is not valid.")]
        [InlineData("1", "4", "10000", null, null, "Year is not valid.")]
        public async Task MissingOrInvalidComponents_FailsBinding(
            string day,
            string month,
            string year,
            string expectedDayModelError,
            string expectedMonthModelError,
            string expectedYearModelError)
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
                ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType),
                ModelName = "TheModelName",
                ModelState = new ModelStateDictionary(),
                ValueProvider = valueProvider
            };

            var modelBinder = new DateInputModelBinder();

            // Act
            await modelBinder.BindModelAsync(bindingContext);

            // Assert
            Assert.Equal(ModelBindingResult.Failed(), bindingContext.Result);

            var topLevelError = Assert.Single(bindingContext.ModelState["TheModelName"].Errors);
            Assert.Equal("Invalid date specified.", topLevelError.ErrorMessage);

            if (expectedDayModelError != null)
            {
                var dayError = Assert.Single(bindingContext.ModelState["TheModelName.Day"].Errors);
                Assert.Equal(expectedDayModelError, dayError.ErrorMessage);
            }

            if (expectedMonthModelError != null)
            {
                var monthError = Assert.Single(bindingContext.ModelState["TheModelName.Month"].Errors);
                Assert.Equal(expectedMonthModelError, monthError.ErrorMessage);
            }

            if (expectedYearModelError != null)
            {
                var yearError = Assert.Single(bindingContext.ModelState["TheModelName.Year"].Errors);
                Assert.Equal(expectedYearModelError, yearError.ErrorMessage);
            }

            Assert.Equal(day, bindingContext.ModelState["TheModelName.Day"].AttemptedValue);
            Assert.Equal(month, bindingContext.ModelState["TheModelName.Month"].AttemptedValue);
            Assert.Equal(year, bindingContext.ModelState["TheModelName.Year"].AttemptedValue);
        }
    }
}
