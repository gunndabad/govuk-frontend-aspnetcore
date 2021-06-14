using System;

namespace GovUk.Frontend.AspNetCore
{
    internal class DateDateInputModelConverter : DateInputModelConverter
    {
        public override bool CanConvertModelType(Type modelType) =>
            modelType == typeof(Date) || modelType == typeof(Date?);

        public override object CreateModelFromElements(Type modelType, (int Day, int Month, int Year) components)
        {
            var date = new Date(components.Year, components.Month, components.Day);
            return modelType == typeof(Date?) ? (Date?)date : date;
        }

        public override (int Day, int Month, int Year)? GetElementsFromModel(Type modelType, object model)
        {
            var date = (Date?)model;

            return date.HasValue ?
                (date.Value.Day, date.Value.Month, date.Value.Year) :
                null;
        }
    }
}
