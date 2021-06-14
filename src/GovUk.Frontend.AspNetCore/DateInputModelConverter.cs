using System;

namespace GovUk.Frontend.AspNetCore
{
    public abstract class DateInputModelConverter
    {
        public abstract bool CanConvertModelType(Type modelType);
        public abstract object CreateModelFromElements(Type modelType, (int Day, int Month, int Year) elements);
        public abstract (int Day, int Month, int Year)? GetElementsFromModel(Type modelType, object model);

        public virtual bool TryCreateModelFromErrors(Type modelType, DateInputParseErrors errors, out object model)
        {
            model = default;
            return false;
        }
    }
}
