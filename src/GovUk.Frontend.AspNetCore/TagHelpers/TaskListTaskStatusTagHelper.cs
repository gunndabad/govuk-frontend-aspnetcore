using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the status of a task in a GOV.UK task list component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TaskListTaskTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TaskListStatusElement)]
    public class TaskListTaskStatusTagHelper : TagHelper
    {
        internal const string TagName = "govuk-task-list-task-status";

        /// <summary>
        /// The status of the task. Set to <c>null</c> if no status is possible or the status does not fit one of the standard values.
        /// </summary>
        [HtmlAttributeName("status")]
        public TaskListTaskStatus? Status { get; set; }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var taskContext = context.GetContextItem<TaskListTaskContext>();

            string textContent = null;
            using (context.SetScopedContextItem(taskContext))
            {
                textContent = (await output.GetChildContentAsync()).GetContent();
            }

            taskContext.Status = (output.Attributes.ToAttributeDictionary(), Status, textContent);

            output.SuppressOutput();
        }
    }
}
