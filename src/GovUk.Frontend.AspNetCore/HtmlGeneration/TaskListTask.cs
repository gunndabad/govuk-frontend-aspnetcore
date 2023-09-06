using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public class TaskListTask
    {
        public AttributeDictionary Attributes { get; set; }
        public (AttributeDictionary Attributes, HtmlString Content) Name { get; init; }
        public string Href { get; set; }
        public (AttributeDictionary Attributes, TaskListTaskStatus? Status, string Content) Status { get; internal set; }
        public (AttributeDictionary Attributes, HtmlString Content) Hint { get; internal set; }
    }
}
