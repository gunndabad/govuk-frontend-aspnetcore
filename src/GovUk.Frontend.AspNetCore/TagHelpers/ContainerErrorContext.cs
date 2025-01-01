using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class ContainerErrorContext
{
    private readonly List<(IHtmlContent Content, IHtmlContent? Href)> _errors = new();

    public IReadOnlyCollection<(IHtmlContent Content, IHtmlContent? Href)> Errors => _errors.AsReadOnly();

    public void AddError(IHtmlContent content, IHtmlContent? href)
    {
        ArgumentNullException.ThrowIfNull(content);

        _errors.Add((content, href));
    }
}
