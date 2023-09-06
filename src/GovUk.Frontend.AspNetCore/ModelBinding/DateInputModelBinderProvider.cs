using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GovUk.Frontend.AspNetCore.ModelBinding;

/// <summary>
/// An <see cref="IModelBinderProvider"/> for binding Date input components.
/// </summary>
public class DateInputModelBinderProvider : IModelBinderProvider
{
    private readonly GovUkFrontendAspNetCoreOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateInputModelBinderProvider"/> class.
    /// </summary>
    public DateInputModelBinderProvider(GovUkFrontendAspNetCoreOptions options)
    {
        Guard.ArgumentNotNull(nameof(options), options);

        _options = options;
    }

    /// <inheritdoc/>
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        Guard.ArgumentNotNull(nameof(context), context);

        var modelType = context.Metadata.UnderlyingOrModelType;

        foreach (var converter in _options.DateInputModelConverters)
        {
            if (converter.CanConvertModelType(modelType))
            {
                return new DateInputModelConverterModelBinder(converter);
            }
        }

        return null;
    }
}
