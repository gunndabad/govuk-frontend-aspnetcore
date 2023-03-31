using System.Linq;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class ComponentTests
    {
        [Theory]
        [ComponentFixtureData("pagination", typeof(OptionsJson.Pagination))]
        public void Pagination(ComponentTestCaseData<OptionsJson.Pagination> data) =>
            CheckComponentHtmlMatchesExpectedHtml(
                data,
                (generator, options) =>
                {
                    var items = (options.Items ?? Enumerable.Empty<OptionsJson.PaginationItem>()).Select(i =>
                        i.Ellipsis ?
                            (PaginationItemBase)new PaginationItemEllipsis() :
                            new PaginationItem()
                            {
                                Attributes = i.Attributes.ToAttributesDictionary(),
                                Href = i.Href ?? string.Empty,
                                IsCurrent = i.Current,
                                Number = new HtmlString(i.Number),
                                VisuallyHiddenText = i.VisuallyHiddenText
                            });

                    var previous = options.Previous != null ?
                        new PaginationPrevious()
                        {
                            LinkAttributes = options.Previous.Attributes.ToAttributesDictionary(),
                            Href = options.Previous.Href ?? string.Empty,
                            LabelText = options.Previous.LabelText,
                            Text = options.Previous.Text != null ? new HtmlString(options.Previous.Text) : null
                        } :
                        null;

                    var next = options.Next != null ?
                        new PaginationNext()
                        {
                            LinkAttributes = options.Next.Attributes.ToAttributesDictionary(),
                            Href = options.Next.Href ?? string.Empty,
                            LabelText = options.Next.LabelText,
                            Text = options.Next.Text != null ? new HtmlString(options.Next.Text) : null
                        } :
                        null;

                    var attributes = options.Attributes.ToAttributesDictionary()
                        .MergeAttribute("class", options.Classes);

                    return generator.GeneratePagination(
                        items,
                        previous,
                        next,
                        options.LandmarkLabel,
                        attributes).ToHtmlString();
                });
    }
}
