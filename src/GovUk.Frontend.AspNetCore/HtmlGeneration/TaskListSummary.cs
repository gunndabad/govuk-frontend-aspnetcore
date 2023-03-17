using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public class TaskListSummary
    {
        public AttributeDictionary Attributes { get; set; }
        public string IncompleteStatus { get; set; }
        public string CompletedStatus { get; set; }
        public string Tracker { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int HeadingLevel { get; set; }
    }
}
