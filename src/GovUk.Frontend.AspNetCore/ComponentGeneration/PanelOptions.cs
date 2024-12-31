using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class PanelOptions
{
    public string? TitleText { get; set; }
    public IHtmlContent? TitleHtml { get; set; }
    public int? HeadingLevel { get; set; }
    public string? Text { get; set; }
    public IHtmlContent? Html { get; set; }
    public IHtmlContent? Classes { get; set; }
    public EncodedAttributesDictionary? Attributes { get; set; }

    [NonStandardParameter]
    internal EncodedAttributesDictionary? TitleAttributes { get; set; }

    [NonStandardParameter]
    internal EncodedAttributesDictionary? BodyAttributes { get; set; }

    internal void Validate()
    {
        if (TitleHtml.NormalizeEmptyString() is null && TitleText.NormalizeEmptyString() is null)
        {
            throw new InvalidOptionsException(GetType(), $"{nameof(TitleHtml)} or {nameof(TitleText)} must be specified.");
        }
    }
}
