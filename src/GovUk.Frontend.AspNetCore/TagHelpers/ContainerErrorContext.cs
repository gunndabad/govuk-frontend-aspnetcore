using GovUk.Frontend.AspNetCore.Components;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class ContainerErrorContext
{
    private readonly List<(TemplateString Html, TemplateString? Href)> _errors = new();

    public IReadOnlyCollection<(TemplateString Html, TemplateString? Href)> Errors => _errors.AsReadOnly();

    public bool ErrorSummaryHasBeenRendered { get; set; }

    public void AddError(TemplateString html, TemplateString? href)
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
            .ToArray();
}
