using System;

namespace GovUk.Frontend.AspNetCore.ModelBinding;

internal class DateOnlyDateInputModelConverter : DateInputModelConverter
{
    public override bool CanConvertModelType(Type modelType) => modelType == typeof(DateOnly);

    public override object CreateModelFromDate(Type modelType, DateOnly date) => date;

    public override DateOnly? GetDateFromModel(Type modelType, object model) => (DateOnly?)model;
}
