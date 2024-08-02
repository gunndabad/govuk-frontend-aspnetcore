using System;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

/// <summary>
/// The exception that is thrown when invalid options are passed to <see cref="IComponentGenerator"/>.
/// </summary>
public class InvalidOptionsException : Exception
{
    /// Initializes a new instance of the <see cref="InvalidOptionsException"/> class with the specified message.
    public InvalidOptionsException(string message)
        : base(message)
    {
    }
}
