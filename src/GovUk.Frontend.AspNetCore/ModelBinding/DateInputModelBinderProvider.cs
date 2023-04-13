using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GovUk.Frontend.AspNetCore.ModelBinding
{
    internal class DateInputModelBinderProvider : IModelBinderProvider
    {
        private readonly GovUkFrontendAspNetCoreOptions _options;

        public DateInputModelBinderProvider(GovUkFrontendAspNetCoreOptions options)
        {
            Guard.ArgumentNotNull(nameof(options), options);

            _options = options;
        }

        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            Guard.ArgumentNotNull(nameof(context), context);

            var modelType = context.Metadata.UnderlyingOrModelType;
            return _options.GetDateInputModelBinder(modelType);
        }
    }
}
