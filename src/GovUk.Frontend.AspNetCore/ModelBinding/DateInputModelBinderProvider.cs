using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.ModelBinding;

/// <summary>
/// An <see cref="IModelBinderProvider"/> for binding Date input components.
/// </summary>
public class DateInputModelBinderProvider : IModelBinderProvider
{
    private readonly IOptions<GovUkFrontendAspNetCoreOptions> _optionsAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateInputModelBinderProvider"/> class.
    /// </summary>
    public DateInputModelBinderProvider(IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
    {
        Guard.ArgumentNotNull(nameof(optionsAccessor), optionsAccessor);

        _optionsAccessor = optionsAccessor;
    }

    /// <inheritdoc/>
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        Guard.ArgumentNotNull(nameof(context), context);

        var modelType = context.Metadata.UnderlyingOrModelType;

        foreach (var converter in _optionsAccessor.Value.DateInputModelConverters)
        {
            if (converter.CanConvertModelType(modelType))
            {
                return new DateInputModelConverterModelBinder(converter, _optionsAccessor);
            }
        }

        return null;
    }
}
