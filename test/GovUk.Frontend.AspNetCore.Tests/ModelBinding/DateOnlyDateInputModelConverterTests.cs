using System;
using GovUk.Frontend.AspNetCore.ModelBinding;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.ModelBinding
{
    public class DateOnlyDateInputModelConverterTests
    {
        [Theory]
        [MemberData(nameof(CreateDateFromElementsData))]
        public void CreateDateFromComponents_ReturnsExpectedResult(
            Type modelType,
            DateOnly date,
            object expectedResult)
        {
            // Arrange
            var converter = new DateOnlyDateInputModelConverter();

            // Act
            var result = converter.CreateModelFromDate(modelType, date);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [MemberData(nameof(GetDateFromModelData))]
        public void GetDateFromModel_ReturnsExpectedResult(
           Type modelType,
           object model,
           DateOnly? expectedResult)
        {
            // Arrange
            var converter = new DateOnlyDateInputModelConverter();

            // Act
            var result = converter.GetDateFromModel(modelType, model);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static TheoryData<Type, DateOnly?, object> CreateDateFromElementsData { get; } = new()
        {
            { typeof(DateOnly), new DateOnly(2020, 4, 1), new DateOnly(2020, 4, 1) },
            { typeof(DateOnly?), new DateOnly(2020, 4, 1), (DateOnly?)new DateOnly(2020, 4, 1) }
        };

        public static TheoryData<Type, object, DateOnly?> GetDateFromModelData { get; } = new()
        {
            { typeof(DateOnly), new DateOnly(2020, 4, 1), new DateOnly(2020, 4, 1) },
            { typeof(DateOnly?), (DateOnly?)new DateOnly(2020, 4, 1), new DateOnly(2020, 4, 1) },
        };
    }
}
