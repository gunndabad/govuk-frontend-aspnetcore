using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson
{
    public record Pagination
    {
        public IEnumerable<PaginationItem> Items { get; set; }
        public PaginationPrevious Previous { get; set; }
        public PaginationNext Next { get; set; }
        public string LandmarkLabel { get; set; }
        public string Classes { get; set; }
        public IDictionary<string, object> Attributes { get; set; }
    }

    public record PaginationItem
    {
        public string Number { get; set; }
        public string VisuallyHiddenText { get; set; }
        public string Href { get; set; }
        public bool Current { get; set; }
        public bool Ellipsis { get; set; }
        public IDictionary<string, object> Attributes { get; set; }
    }

    public record PaginationPrevious
    {
        public string Text { get; set; }
        public string LabelText { get; set; }
        public string Href { get; set; }
        public IDictionary<string, object> Attributes { get; set; }
    }

    public record PaginationNext
    {
        public string Text { get; set; }
        public string LabelText { get; set; }
        public string Href { get; set; }
        public IDictionary<string, object> Attributes { get; set; }
    }
}
