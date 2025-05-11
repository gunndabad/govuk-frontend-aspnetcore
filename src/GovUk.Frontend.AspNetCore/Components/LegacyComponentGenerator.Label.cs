namespace GovUk.Frontend.AspNetCore.Components;

partial class LegacyComponentGenerator
{
    internal const string LabelElement = "label";
    internal const bool LabelDefaultIsPageHeading = false;

    /// <inheritdoc/>
    public virtual HtmlTagBuilder GenerateLabel(LabelOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        var label = new HtmlTagBuilder(LabelElement)
            .WithAttributeWhenNotNull(options.For, "for")
            .WithCssClass("govuk-label")
            .WithCssClasses(ExplodeClasses(options.Classes?.ToHtmlString()))
            .WithAttributes(options.Attributes)
            .WithAppendedHtml(GetEncodedTextOrHtml(options.Text, options.Html)!);

        if (options.IsPageHeading ?? LabelDefaultIsPageHeading)
        {
            return new HtmlTagBuilder("h1")
                .WithCssClass("govuk-label-wrapper")
                .WithAppendedHtml(label);
        }
        else
        {
            return label;
        }
    }
}
