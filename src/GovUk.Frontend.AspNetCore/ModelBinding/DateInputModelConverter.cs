namespace GovUk.Frontend.AspNetCore.ModelBinding;

/// <summary>
/// Converts a <see cref="DateOnly"/> to and from an alternative model type.
/// </summary>
public abstract class DateInputModelConverter
{
    /// <summary>
    /// Determines whether this converter can convert the specified model type.
    /// </summary>
    /// <param name="modelType">The model type.</param>
    /// <returns><see langword="true"/> if this instance can convert the specified model type; otherwise <see langword="false"/>.</returns>
    public abstract bool CanConvertModelType(Type modelType);

    /// <summary>
    /// Converts <paramref name="date"/> to an instance of <paramref name="modelType"/>.
    /// </summary>
    /// <param name="modelType">The model type to convert to.</param>
    /// <param name="date">The <see cref="DateOnly"/> instance to convert.</param>
    /// <returns>An instance of <paramref name="modelType"/> that represents the <paramref name="date"/> argument.</returns>
    public abstract object CreateModelFromDate(Type modelType, DateOnly date);

    /// <summary>
    /// Converts <paramref name="model"/> to instance of <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="modelType">The model type to convert from.</param>
    /// <param name="model">The model instance to convert.</param>
    /// <returns>The converted model instance.</returns>
    public abstract DateOnly? GetDateFromModel(Type modelType, object model);
}
