using System;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class FormErrorContext
{
    private readonly List<(string Html, string? Href)> _errors;

    public FormErrorContext()
    {
        _errors = new();
    }

    public IReadOnlyCollection<(string Html, string? Href)> Errors => _errors.AsReadOnly();

    public void AddError(string html, string? href)
    {
        ArgumentNullException.ThrowIfNull(html);
        _errors.Add((html, href));
    }
}
