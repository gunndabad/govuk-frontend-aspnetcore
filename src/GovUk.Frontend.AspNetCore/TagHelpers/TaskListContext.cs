using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.HtmlGeneration;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class TaskListContext
    {
        private readonly List<TaskListTask> _tasks;

        public TaskListContext()
        {
            _tasks = new List<TaskListTask>();
        }

        public IReadOnlyList<TaskListTask> Tasks => _tasks;

        public void AddTask(TaskListTask task)
        {
            Guard.ArgumentNotNull(nameof(task), task);

            _tasks.Add(task);
        }

        public void ThrowIfIncomplete()
        {
            if (Tasks.Count < 1)
            {
                throw ExceptionHelper.AChildElementMustBeProvided(TaskListTaskTagHelper.TagName);
            }
        }
    }
}
