using GovUk.Frontend.AspNetCore.HtmlGeneration;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class SummaryListContext
{
    private readonly List<SummaryListRow> _rows;

    public SummaryListContext()
    {
        _rows = new List<SummaryListRow>();
    }

    public IReadOnlyList<SummaryListRow> Rows => _rows;

    public void AddRow(SummaryListRow row)
    {
        Guard.ArgumentNotNull(nameof(row), row);

        _rows.Add(row);
    }
}
