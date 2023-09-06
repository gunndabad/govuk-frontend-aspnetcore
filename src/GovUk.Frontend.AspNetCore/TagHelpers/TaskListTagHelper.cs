using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GOV.UK task list component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [RestrictChildren(TaskListTaskTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TaskListElement)]
    public class TaskListTagHelper : TagHelper
    {
        internal const string TagName = "govuk-task-list";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="TaskListTagHelper"/>.
        /// </summary>
        public TaskListTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal TaskListTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var taskListContext = new TaskListContext();

            using (context.SetScopedContextItem(taskListContext))
            {
                await output.GetChildContentAsync();
            }

            taskListContext.ThrowIfIncomplete();

            var tagBuilder = _htmlGenerator.GenerateTaskList(
                output.Attributes.ToAttributeDictionary(),
                taskListContext.Tasks);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
