using System;
using GovUk.Frontend.AspNetCore;
using NodaTime;

namespace Samples.DateInput
{
    public class LocalDateDateInputModelConverter : DateInputModelConverter
    {
        public override bool CanConvertModelType(Type modelType) => modelType == typeof(LocalDate) || modelType == typeof(LocalDate?);

        public override object CreateModelFromElements(Type modelType, (int Day, int Month, int Year) components)
        {
            var (day, month, year) = components;
            return new LocalDate(year, month, day);
        }

        public override (int Day, int Month, int Year)? GetElementsFromModel(Type modelType, object model)
        {
            var localDate = (LocalDate?)model;

            return localDate.HasValue ?
                (localDate.Value.Day, localDate.Value.Month, localDate.Value.Year) :
                null;
        }
    }
}
