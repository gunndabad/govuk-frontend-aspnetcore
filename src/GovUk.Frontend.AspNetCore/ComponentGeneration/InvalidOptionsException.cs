using System;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

/// <summary>
/// The exception that is thrown when invalid options are passed to <see cref="ILegacyComponentGenerator"/>.
/// </summary>
public class InvalidOptionsException : Exception
{
    /// Initializes a new instance of the <see cref="InvalidOptionsException"/> class with the specified message.
    public InvalidOptionsException(Type optionsType, string message)
        : base(GetMessage(optionsType, message))
    {
    }

    private static string GetMessage(Type optionsType, string message)
    {
        return message + "\n" + "Options type: " + optionsType.FullName;
    }
}
