using System;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore
{
    public class TabsItem
    {
        public TabsItem(string id, string label, IHtmlContent content)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Label = label ?? throw new ArgumentNullException(nameof(label));
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public string Id { get; }
        public string Label { get; }
        public IHtmlContent Content { get; }
    }
}
