using System;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GOV.UK task list summary component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.TaskListSummaryElement)]
    public class TaskListSummaryTagHelper : TagHelper
    {
        internal const string TagName = "govuk-task-list-summary";

        private readonly IGovUkHtmlGenerator _htmlGenerator;
        private int _headingLevel = ComponentGenerator.TaskListSummaryDefaultHeadingLevel;

        /// <summary>
        /// The heading to display when the task list is incomplete.
        /// </summary>
        [HtmlAttributeName("incomplete-status")]
        public string IncompleteStatus { get; set; } = ComponentGenerator.TaskListSummaryDefaultIncompleteStatus;

        /// <summary>
        /// The heading to display when the task list is completed.
        /// </summary>
        [HtmlAttributeName("completed-status")]
        public string CompletedStatus { get; set; } = ComponentGenerator.TaskListSummaryDefaultCompletedStatus;

        /// <summary>
        /// Text that displays how many tasks are completed. {0} is replaced with the number of completed tasks, and {1} is replaced with the total number of tasks.
        /// </summary>
        [HtmlAttributeName("tracker")]
        public string Tracker { get; set; } = ComponentGenerator.TaskListSummaryDefaultTracker;

        /// <summary>
        /// The total number of tasks in the task list that must be completed.
        /// </summary>
        [HtmlAttributeName("total-tasks")]
        public int TotalTasks { get; set; } = 0;

        /// <summary>
        /// The number of tasks in the task list that have been completed so far.
        /// </summary>
        [HtmlAttributeName("completed-tasks")]
        public int CompletedTasks { get; set; } = 0;


        /// <summary>
        /// The heading level.
        /// </summary>
        /// <remarks>
        /// Must be between <c>1</c> and <c>6</c> (inclusive). The default is <c>2</c>.
        /// </remarks>
        [HtmlAttributeName("heading-level")]
        public int HeadingLevel
        {
            get => _headingLevel;
            set
            {
                if (value < ComponentGenerator.TaskListSummaryMinHeadingLevel ||
                    value > ComponentGenerator.TaskListSummaryMaxHeadingLevel)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        $"{nameof(HeadingLevel)} must be between {ComponentGenerator.TaskListSummaryMinHeadingLevel} and {ComponentGenerator.TaskListSummaryMaxHeadingLevel}.");
                }

                _headingLevel = value;
            }
        }

        /// <summary>
        /// Creates a new <see cref="TaskListSummaryTagHelper"/>.
        /// </summary>
        public TaskListSummaryTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal TaskListSummaryTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <inheritdoc/>
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var tagBuilder = _htmlGenerator.GenerateTaskListSummary(new TaskListSummary
            {
                Attributes = output.Attributes.ToAttributeDictionary(),
                HeadingLevel = HeadingLevel,
                IncompleteStatus = IncompleteStatus,
                CompletedStatus = CompletedStatus,
                Tracker = Tracker,
                TotalTasks = TotalTasks,
                CompletedTasks = CompletedTasks
            });

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);

            return Task.CompletedTask;
        }
    }
}
