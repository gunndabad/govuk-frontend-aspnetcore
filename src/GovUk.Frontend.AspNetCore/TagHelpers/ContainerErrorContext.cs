using System;
using System.Collections.Generic;
using System.Linq;
using GovUk.Frontend.AspNetCore.Components;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class ContainerErrorContext
{
    private readonly List<(string Html, string? Href)> _errors = new();

    public IReadOnlyCollection<(string Html, string? Href)> Errors => _errors.AsReadOnly();

    public void AddError(string html, string? href)
    {
        ArgumentNullException.ThrowIfNull(html);

        _errors.Add((html, href));
    }

    public IReadOnlyCollection<ErrorSummaryOptionsErrorItem> GetErrorList() =>
        Errors
            .Select(i => new ErrorSummaryOptionsErrorItem()
            {
                Href = i.Href,
                Text = null,
                Html = i.Html,
                Attributes = null,
                ItemAttributes = null
            })
            .ToList();
}
