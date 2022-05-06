using System;
using GovUk.Frontend.AspNetCore;
using NodaTime;

namespace Samples.DateInput
{
    public class LocalDateDateInputModelConverter : DateInputModelConverter
    {
        public override bool CanConvertModelType(Type modelType) => modelType == typeof(LocalDate) || modelType == typeof(LocalDate?);

        public override object CreateModelFromDate(Type modelType, Date date)
        {
            return new LocalDate(date.Year, date.Month, date.Day);
        }

        public override Date? GetDateFromModel(Type modelType, object model)
        {
            var localDate = (LocalDate?)model;

            return localDate.HasValue ?
                new Date(localDate.Value.Year, localDate.Value.Month, localDate.Value.Day) :
                null;
        }
    }
}
