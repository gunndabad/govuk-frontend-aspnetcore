using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore
{
    public abstract class RadiosItemBase
    {
        private protected RadiosItemBase() { }
    }

    public class RadiosItem : RadiosItemBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Checked { get; set; }
        public bool Disabled { get; set; }
        public IHtmlContent Content { get; set; }
        public string ConditionalId { get; set; }
        public IHtmlContent ConditionalContent { get; set; }
        public string HintId { get; set; }
        public IHtmlContent HintContent { get; set; }
    }

    public class RadiosItemDivider : RadiosItemBase
    {
        public IHtmlContent Content { get; set; }
    }
}
