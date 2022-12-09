using System;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests
{
    public class DateDateInputModelConverterTests
    {
        [Theory]
        [MemberData(nameof(CreateDateFromElementsData))]
        public void CreateDateFromComponents_ReturnsExpectedResult(
            Type modelType,
            Date date,
            object expectedResult)
        {
            // Arrange
            var converter = new DateDateInputModelConverter();

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
           Date? expectedResult)
        {
            // Arrange
            var converter = new DateDateInputModelConverter();

            // Act
            var result = converter.GetDateFromModel(modelType, model);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static TheoryData<Type, Date?, object> CreateDateFromElementsData { get; } = new TheoryData<Type, Date?, object>()
        {
            { typeof(Date), new Date(2020, 4, 1), new Date(2020, 4, 1) },
            { typeof(Date?), new Date(2020, 4, 1), (Date?)new Date(2020, 4, 1) }
        };

        public static TheoryData<Type, object, Date?> GetDateFromModelData { get; } = new TheoryData<Type, object, Date?>()
        {
            { typeof(Date), new Date(2020, 4, 1), new Date(2020, 4, 1) },
            { typeof(Date?), (Date?)new Date(2020, 4, 1), new Date(2020, 4, 1) }
        };
    }
}
