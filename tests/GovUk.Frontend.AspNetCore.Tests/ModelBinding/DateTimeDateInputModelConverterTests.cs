using System;
using GovUk.Frontend.AspNetCore.ModelBinding;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.ModelBinding;

public class DateTimeDateInputModelConverterTests
{
    [Theory]
    [MemberData(nameof(CreateModelFromDateData))]
    public void CreateModelFromDate_ReturnsExpectedResult(
        Type modelType,
        DateOnly date,
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
       DateOnly? expectedResult)
    {
        // Arrange
        var converter = new DateTimeDateInputModelConverter();

        // Act
        var result = converter.GetDateFromModel(modelType, model);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    public static TheoryData<Type, DateOnly, object> CreateModelFromDateData { get; } = new()
    {
        { typeof(DateTime), new DateOnly(2020, 4, 1), new DateTime(2020, 4, 1) },
        { typeof(DateTime?), new DateOnly(2020, 4, 1), (DateTime?)new DateTime(2020, 4, 1) }
    };

    public static TheoryData<Type, object, DateOnly?> GetDateFromModelData { get; } = new()
    {
        { typeof(DateTime), new DateTime(2020, 4, 1), new DateOnly(2020, 4, 1) },
        { typeof(DateTime?), (DateTime?)new DateTime(2020, 4, 1), new DateOnly(2020, 4, 1) },
    };
}
