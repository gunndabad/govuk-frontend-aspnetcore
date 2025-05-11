namespace GovUk.Frontend.AspNetCore.ModelBinding;

internal class DateTimeDateInputModelConverter : DateInputModelConverter
{
    public override bool CanConvertModelType(Type modelType) => modelType == typeof(DateTime);

    public override object CreateModelFromDate(Type modelType, DateOnly date) => new DateTime(date.Year, date.Month, date.Day);

    public override DateOnly? GetDateFromModel(Type modelType, object model)
    {
        if (model is null)
        {
            return null;
        }

        return DateOnly.FromDateTime((DateTime)model);
    }
}
