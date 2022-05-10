#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class FormErrorContext
    {
        private readonly List<(IHtmlContent Content, string? Href)> _errors;

        public FormErrorContext()
        {
            _errors = new();
        }

        public IReadOnlyCollection<(IHtmlContent Content, string? Href)> Errors => _errors.AsReadOnly();

        public void AddError(IHtmlContent content, string? href)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            _errors.Add((content, href));
        }
    }
}
