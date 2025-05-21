using GovUk.Frontend.AspNetCore.ModelBinding;

namespace GovUk.Frontend.AspNetCore;

/// <summary>
/// An attribute that can specify the error message prefix to use in model binding from date input components.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class DateInputAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the prefix used in error messages.
    /// </summary>
    /// <remarks>
    /// This prefix is used at the start of error messages produced by <see cref="DateInputModelBinder"/>
    /// e.g. <c>{ErrorMessagePrefix} must be a real date</c>
    /// </remarks>
    public string? ErrorMessagePrefix { get; set; }
}
