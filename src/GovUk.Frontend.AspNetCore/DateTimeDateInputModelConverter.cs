using System;

namespace GovUk.Frontend.AspNetCore
{
    internal class DateTimeDateInputModelConverter : DateInputModelConverter
    {
        public override bool CanConvertModelType(Type modelType) =>
            modelType == typeof(DateTime) || modelType == typeof(DateTime?);

        public override object CreateModelFromDate(Type modelType, Date date) => new DateTime(date.Year, date.Month, date.Day);

        public override Date? GetDateFromModel(Type modelType, object model)
        {
            if (model is null)
            {
                return null;
            }

            return Date.FromDateTime((DateTime)model);
        }
    }
}
