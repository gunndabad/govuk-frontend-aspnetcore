using System;

namespace GovUk.Frontend.AspNetCore
{
    public class DateTimeDateInputModelConverter : DateInputModelConverter
    {
        public override bool CanConvertModelType(Type modelType) =>
            modelType == typeof(DateTime) || modelType == typeof(DateTime?);

        public override object CreateModelFromElements(Type modelType, (int Day, int Month, int Year) components)
        {
            var date = new DateTime(components.Year, components.Month, components.Day);
            return modelType == typeof(DateTime?) ? (DateTime?)date : date;
        }

        public override (int Day, int Month, int Year)? GetElementsFromModel(Type modelType, object model)
        {
            var date = (DateTime?)model;

            return date.HasValue ?
                (date.Value.Day, date.Value.Month, date.Value.Year) :
                null;
        }
    }
}
