using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents an task in a GOV.UK task list component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TaskListTagHelper.TagName)]
    [RestrictChildren(TaskListTaskNameTagHelper.TagName, TaskListTaskHintTagHelper.TagName, TaskListTaskStatusTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TaskListTaskElement)]
    public class TaskListTaskTagHelper : TagHelper
    {
        internal const string TagName = "govuk-task-list-task";
        private const string TaskHrefAttributeName = "href";

        /// <summary>
        /// A link to the task.
        /// </summary>
        [HtmlAttributeName(TaskHrefAttributeName)]
        public string Href { get; set; }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var taskListContext = context.GetContextItem<TaskListContext>();

            var taskContext = new TaskListTaskContext();

            using (context.SetScopedContextItem(taskContext))
            {
                await output.GetChildContentAsync();
            }

            taskContext.ThrowIfIncomplete();

            taskListContext.AddTask(new TaskListTask
            {
                Attributes = output.Attributes.ToAttributeDictionary(),
                Name = taskContext.Name,
                Href = Href,
                Status = taskContext.Status,
                Hint = taskContext.Hint
            });

            output.SuppressOutput();
        }
    }
}
