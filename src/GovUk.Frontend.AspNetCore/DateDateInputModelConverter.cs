#pragma warning disable CS0618 // Type or member is obsolete
using System;

namespace GovUk.Frontend.AspNetCore
{
    internal class DateDateInputModelConverter : DateInputModelConverter
    {
        public override bool CanConvertModelType(Type modelType) => modelType == typeof(Date);

        public override object CreateModelFromDate(Type modelType, DateOnly date) => new Date(date.Year, date.Month, date.Day);

        public override DateOnly? GetDateFromModel(Type modelType, object model)
        {
            if (model is null)
            {
                return null;
            }

            return (Date)model;
        }
    }
}
