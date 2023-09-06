using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class TaskListTaskContext
    {
        public (AttributeDictionary Attributes, HtmlString Content) Name { get; internal set; }
        public (AttributeDictionary Attributes, HtmlString Content) Hint { get; internal set; }
        public (AttributeDictionary Attributes, TaskListTaskStatus? Status, string Content) Status { get; internal set; }

        public void ThrowIfIncomplete()
        {
            if (Name.Attributes == null)
            {
                throw ExceptionHelper.AChildElementMustBeProvided(TaskListTaskNameTagHelper.TagName);
            }
        }
    }
}
