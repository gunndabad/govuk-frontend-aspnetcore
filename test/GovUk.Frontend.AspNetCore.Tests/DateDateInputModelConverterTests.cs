using System;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests
{
    public class DateDateInputModelConverterTests
    {
        [Theory]
        [MemberData(nameof(CreateModelFromElementsData))]
        public void CreateModelFromComponents_ReturnsExpectedResult(
            Type modelType,
            (int Year, int Month, int Day) components,
            object expectedResult)
        {
            // Arrange
            var converter = new DateDateInputModelConverter();

            // Act
            var result = converter.CreateModelFromElements(modelType, components);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [MemberData(nameof(GetElementsFromModelData))]
        public void GetComponentsFromModel_ReturnsExpectedResult(
           Type modelType,
           object model,
           (int Year, int Month, int Day)? expectedResult)
        {
            // Arrange
            var converter = new DateDateInputModelConverter();

            // Act
            var result = converter.GetElementsFromModel(modelType, model);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static TheoryData<Type, (int, int, int)?, object> CreateModelFromElementsData { get; } = new TheoryData<Type, (int, int, int)?, object>()
        {
            { typeof(Date), (1, 4, 2020), new Date(2020, 4, 1) },
            { typeof(Date?), (1, 4, 2020), (Date?)new Date(2020, 4, 1) }
        };

        public static TheoryData<Type, object, (int, int, int)?> GetElementsFromModelData { get; } = new TheoryData<Type, object, (int, int, int)?>()
        {
            { typeof(Date), new Date(2020, 4, 1), (1, 4, 2020) },
            { typeof(Date?), (Date?)new Date(2020, 4, 1), (1, 4, 2020) },
            { typeof(Date?), null, null }
        };
    }
}
