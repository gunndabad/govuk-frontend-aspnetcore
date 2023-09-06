using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.ModelBinding;

/// <summary>
/// An <see cref="IModelBinder"/> for binding Date input components.
/// </summary>
public class DateInputModelBinder : IModelBinder
{
    /// <inheritdoc/>
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var options = bindingContext.HttpContext.RequestServices.GetRequiredService<IOptions<GovUkFrontendAspNetCoreOptions>>().Value;

        var modelType = bindingContext.ModelMetadata.UnderlyingOrModelType;

        foreach (var converter in options.DateInputModelConverters)
        {
            if (converter.CanConvertModelType(modelType))
            {
                var innerBinder = new DateInputModelConverterModelBinder(converter);
                return innerBinder.BindModelAsync(bindingContext);
            }
        }

        throw new NotSupportedException($"Cannot bind '{modelType.FullName}' types.");
    }
}
