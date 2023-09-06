#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TaskListElement = "ul";
        internal const string TaskListTaskElement = "li";
        internal const string TaskListTaskNameElement = "span";
        internal const string TaskListHintElement = "span";
        internal const string TaskListStatusElement = "span";

        public TagBuilder GenerateTaskList(
            AttributeDictionary? attributes,
            IEnumerable<TaskListTask> tasks)
        {
            Guard.ArgumentNotNull(nameof(tasks), tasks);
            Guard.ArgumentValid(nameof(tasks), "A task list must contain at least one task", tasks.Any());

            var tagBuilder = new TagBuilder(TaskListElement);
            if (attributes != null) { tagBuilder.MergeAttributes(attributes); }
            tagBuilder.MergeCssClass("govuk-task-list");

            var taskNumber = 1;
            foreach (var task in tasks)
            {
                Guard.ArgumentValid(nameof(tasks), "Task name cannot be null or empty", task.Name.Content != null);

                string taskId = GenerateTaskId(attributes, taskNumber, task);
                var itemTagBuilder = new TagBuilder(TaskListTaskElement);
                if (task.Attributes != null) { itemTagBuilder.MergeAttributes(task.Attributes); }
                if (!itemTagBuilder.Attributes.ContainsKey("id")) { itemTagBuilder.MergeAttribute("id", taskId); }
                itemTagBuilder.MergeCssClass("govuk-task-list__item");
                tagBuilder.InnerHtml.AppendHtml(itemTagBuilder);

                var taskNameTagBuilder = new TagBuilder(TaskListTaskNameElement);
                taskNameTagBuilder.MergeAttributes(task.Name.Attributes);
                taskNameTagBuilder.MergeCssClass("govuk-task-list__task-name-and-hint");

                var statusText = TaskListTaskStatusText(task.Status.Status, task.Status.Content);

                if (!string.IsNullOrEmpty(task.Href) && task.Status.Status != TaskListTaskStatus.NotApplicable && task.Status.Status != TaskListTaskStatus.CannotStartYet)
                {
                    var taskLinkTagBuilder = new TagBuilder("a");
                    taskLinkTagBuilder.MergeAttribute("href", task.Href);
                    taskLinkTagBuilder.MergeCssClass("govuk-link");
                    taskLinkTagBuilder.MergeCssClass("govuk-task-list__link");
                    taskLinkTagBuilder.InnerHtml.AppendHtml(task.Name.Content!);
                    taskNameTagBuilder.InnerHtml.AppendHtml(taskLinkTagBuilder);
                    itemTagBuilder.MergeCssClass("govuk-task-list__item--with-link");

                    if (!string.IsNullOrEmpty(statusText))
                    {
                        var statusId = task.Status.Attributes.ContainsKey("id") ? task.Status.Attributes["id"] : taskId + "-status";
                        taskLinkTagBuilder.MergeAttribute("aria-describedby", statusId);
                    }
                }
                else
                {
                    var unlinkedTaskTagBuilder = new TagBuilder("span");
                    unlinkedTaskTagBuilder.MergeCssClass("govuk-task-list__task-no-link");
                    unlinkedTaskTagBuilder.InnerHtml.AppendHtml(task.Name.Content!);
                    taskNameTagBuilder.InnerHtml.AppendHtml(unlinkedTaskTagBuilder);
                }

                if (task.Hint.Content != null)
                {
                    var hintTagBuilder = new TagBuilder(TaskListHintElement);
                    hintTagBuilder.MergeAttributes(task.Hint.Attributes);
                    hintTagBuilder.MergeCssClass("govuk-task-list__task_hint");
                    hintTagBuilder.InnerHtml.AppendHtml(task.Hint.Content);
                    taskNameTagBuilder.InnerHtml.AppendHtml(hintTagBuilder);
                }

                itemTagBuilder.InnerHtml.AppendHtml(taskNameTagBuilder);

                if (!string.IsNullOrEmpty(statusText))
                {
                    var statusOuterTagBuilder = new TagBuilder(TaskListStatusElement);
                    statusOuterTagBuilder.MergeCssClass("govuk-task-list__status-container");

                    var statusInnerTagBuilder = new TagBuilder("span");
                    statusInnerTagBuilder.MergeAttributes(task.Status.Attributes);
                    statusInnerTagBuilder.MergeCssClass("govuk-task-list__status");
                    if (task.Status.Status.HasValue)
                    {
                        statusInnerTagBuilder.MergeCssClass(TaskStatusCssClass(task.Status.Status.Value));
                    }
                    if (!statusInnerTagBuilder.Attributes.ContainsKey("id")) { statusInnerTagBuilder.MergeAttribute("id", taskId + "-status"); }

                    statusInnerTagBuilder.InnerHtml.AppendHtml(statusText);
                    statusOuterTagBuilder.InnerHtml.AppendHtml(statusInnerTagBuilder);
                    itemTagBuilder.InnerHtml.AppendHtml(statusOuterTagBuilder);
                }

                taskNumber++;
            }

            return tagBuilder;
        }

        private string TaskStatusCssClass(TaskListTaskStatus status)
        {
            return "govuk-task-list__status-" + Regex.Replace(status.ToString(), "([A-Z])", "-$1").ToLowerInvariant();
        }

        public static string? TaskListTaskStatusText(TaskListTaskStatus? status, string? customStatus = null)
        {
            if (!string.IsNullOrEmpty(customStatus))
            {
                return customStatus;
            }
            else if (status.HasValue)
            {
                var statusText = Regex.Replace(status.ToString()!, "([A-Z])", " $1").ToLowerInvariant().Trim();
                return statusText.Substring(0, 1).ToUpperInvariant() + statusText.Substring(1);
            }
            else return null;
        }

        private static string GenerateTaskId(AttributeDictionary? attributes, int taskNumber, TaskListTask task)
        {
            string taskId = string.Empty;
            if (task.Attributes != null && task.Attributes.ContainsKey("id"))
            {
                taskId = task.Attributes["id"]!;
            }
            else
            {
                taskId = Regex.Replace(Regex.Replace(task.Name.Content!.Value!, "<.*?>", string.Empty, RegexOptions.IgnoreCase), "[^A-Z0-9- ]", string.Empty, RegexOptions.IgnoreCase).Replace(" ", "-").ToLowerInvariant();
            }
            if (string.IsNullOrEmpty(taskId)) { taskId = "task-" + taskNumber; }
            if (attributes != null && attributes.ContainsKey("id")) { taskId = attributes["id"] + "-" + taskId; }

            return taskId;
        }
    }
}
