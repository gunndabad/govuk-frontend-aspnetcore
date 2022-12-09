using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GovUk.Frontend.AspNetCore.ModelBinding
{
    internal class DateInputModelBinderProvider : IModelBinderProvider
    {
        private readonly DateInputModelConverter[] _dateInputModelConverters;

        public DateInputModelBinderProvider(GovUkFrontendAspNetCoreOptions options)
        {
            Guard.ArgumentNotNull(nameof(options), options);

            _dateInputModelConverters = options.DateInputModelConverters.ToArray();
        }

        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            Guard.ArgumentNotNull(nameof(context), context);

            var modelType = context.Metadata.UnderlyingOrModelType;

            foreach (var converter in _dateInputModelConverters)
            {
                if (converter.CanConvertModelType(modelType))
                {
                    return new DateInputModelBinder(converter);
                }
            }

            return null;
        }
    }
}
