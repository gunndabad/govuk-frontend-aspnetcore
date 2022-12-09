using System;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests
{
    public class DateTimeDateInputModelConverterTests
    {
        [Theory]
        [MemberData(nameof(CreateModelFromDateData))]
        public void CreateModelFromDate_ReturnsExpectedResult(
            Type modelType,
            Date date,
            object expectedResult)
        {
            // Arrange
            var converter = new DateTimeDateInputModelConverter();

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
            var converter = new DateTimeDateInputModelConverter();

            // Act
            var result = converter.GetDateFromModel(modelType, model);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static TheoryData<Type, Date?, object> CreateModelFromDateData { get; } = new TheoryData<Type, Date?, object>()
        {
            { typeof(DateTime), new Date(2020, 4, 1), new DateTime(2020, 4, 1) },
            { typeof(DateTime?), new Date(2020, 4, 1), (DateTime?)new DateTime(2020, 4, 1) }
        };

        public static TheoryData<Type, object, Date?> GetDateFromModelData { get; } = new TheoryData<Type, object, Date?>()
        {
            { typeof(DateTime), new DateTime(2020, 4, 1), new Date(2020, 4, 1) },
            { typeof(DateTime?), (DateTime?)new DateTime(2020, 4, 1), new Date(2020, 4, 1) }
        };
    }
}
