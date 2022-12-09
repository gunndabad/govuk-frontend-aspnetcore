using System;

namespace GovUk.Frontend.AspNetCore
{
    internal class DateDateInputModelConverter : DateInputModelConverter
    {
        public override bool CanConvertModelType(Type modelType) =>
            modelType == typeof(Date) || modelType == typeof(Date?);

        public override object CreateModelFromDate(Type modelType, Date date) => date;

        public override Date? GetDateFromModel(Type modelType, object model) => (Date?)model;
    }
}
