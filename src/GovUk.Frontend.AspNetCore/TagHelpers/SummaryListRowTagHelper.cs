using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents a row in a GDS summary list component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = SummaryListTagHelper.TagName)]
    [RestrictChildren(SummaryListRowKeyTagHelper.TagName, SummaryListRowValueTagHelper.TagName, SummaryListRowActionsTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.SummaryListRowElement)]
    public class SummaryListRowTagHelper : TagHelper
    {
        internal const string TagName = "govuk-summary-list-row";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var summaryListContext = context.GetContextItem<SummaryListContext>();

            var rowContext = new SummaryListRowContext();

            using (context.SetScopedContextItem(rowContext))
            {
                await output.GetChildContentAsync();
            }

            rowContext.ThrowIfIncomplete();

            summaryListContext.AddRow(new SummaryListRow()
            {
                Actions = new SummaryListRowActions()
                {
                    Items = rowContext.Actions,
                    Attributes = rowContext.ActionsAttributes
                },
                Attributes = output.Attributes.ToAttributeDictionary(),
                Key = new SummaryListRowKey()
                {
                    Content = rowContext.Key!.Value.Content,
                    Attributes = rowContext.Key!.Value.Attributes
                },
                Value = rowContext.Value is not null ?
                    new SummaryListRowValue()
                    {
                        Content = rowContext.Value.Value.Content,
                        Attributes = rowContext.Value.Value.Attributes
                    } :
                    null
            });

            output.SuppressOutput();
        }
    }
}
