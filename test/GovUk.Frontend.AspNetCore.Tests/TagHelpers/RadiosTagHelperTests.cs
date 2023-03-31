using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class RadiosTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-radios",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var radiosContext = context.GetContextItem<RadiosContext>();

                    radiosContext.SetHint(attributes: null, content: new HtmlString("The hint"));

                    radiosContext.AddItem(new RadiosItem()
                    {
                        Checked = false,
                        LabelContent = new HtmlString("First"),
                        Disabled = true,
                        Id = "first",
                        Value = "first"
                    });

                    radiosContext.AddItem(new RadiosItem()
                    {
                        Checked = true,
                        LabelContent = new HtmlString("Second"),
                        Disabled = false,
                        Id = "second",
                        Value = "second"
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                IdPrefix = "my-id",
                Name = "testradios"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div class=""govuk-form-group"">
    <div class=""govuk-hint"" id=""my-id-hint"">The hint</div>
    <div class=""govuk-radios"" data-module=""govuk-radios"">
        <div class=""govuk-radios__item"">
            <input class=""govuk-radios__input"" id=""first"" name=""testradios"" type=""radio"" value=""first"" disabled=""disabled"" />
            <label class=""govuk-radios__label govuk-label"" for=""first"">First</label>
        </div>
        <div class=""govuk-radios__item"">
            <input class=""govuk-radios__input"" id=""second"" name=""testradios"" type=""radio"" value=""second"" checked=""checked"" />
            <label class=""govuk-radios__label govuk-label"" for=""second"">Second</label>
        </div>
    </div>
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
        }

        [Fact]
        public async Task ProcessAsync_WithError_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-radios",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var radiosContext = context.GetContextItem<RadiosContext>();

                    radiosContext.SetErrorMessage(visuallyHiddenText: null, attributes: null, content: new HtmlString("A error"));

                    radiosContext.AddItem(new RadiosItem()
                    {
                        Checked = false,
                        LabelContent = new HtmlString("First"),
                        Disabled = true,
                        Id = "first",
                        Value = "first"
                    });

                    radiosContext.AddItem(new RadiosItem()
                    {
                        Checked = true,
                        LabelContent = new HtmlString("Second"),
                        Disabled = false,
                        Id = "second",
                        Value = "second"
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                IdPrefix = "my-id",
                Name = "testradios"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div class=""govuk-form-group govuk-form-group--error"">
    <p class=""govuk-error-message"" id=""my-id-error""><span class=""govuk-visually-hidden"">Error:</span>A error</p>
    <div class=""govuk-radios"" data-module=""govuk-radios"">
        <div class=""govuk-radios__item"">
            <input class=""govuk-radios__input"" id=""first"" name=""testradios"" type=""radio"" value=""first"" disabled=""disabled"" />
            <label class=""govuk-radios__label govuk-label"" for=""first"">First</label>
        </div>
        <div class=""govuk-radios__item"">
            <input class=""govuk-radios__input"" id=""second"" name=""testradios"" type=""radio"" value=""second"" checked=""checked"" />
            <label class=""govuk-radios__label govuk-label"" for=""second"">Second</label>
        </div>
    </div>
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
        }

        [Fact]
        public async Task ProcessAsync_WithItemHint_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-radios",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var radiosContext = context.GetContextItem<RadiosContext>();

                    radiosContext.AddItem(new RadiosItem()
                    {
                        LabelContent = new HtmlString("First"),
                        Hint = new RadiosItemHint()
                        {
                            Content = new HtmlString("First item hint")
                        },
                        Id = "first",
                        Value = "first"
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                IdPrefix = "my-id",
                Name = "testradios"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div class=""govuk-form-group"">
    <div class=""govuk-radios"" data-module=""govuk-radios"">
        <div class=""govuk-radios__item"">
            <input class=""govuk-radios__input"" id=""first"" name=""testradios"" type=""radio"" value=""first"" aria-describedby=""first-item-hint"" />
            <label class=""govuk-radios__label govuk-label"" for=""first"">First</label>
            <div class=""govuk-radios__hint govuk-hint"" id=""first-item-hint"">First item hint</div>
        </div>
    </div>
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
        }

        [Fact]
        public async Task ProcessAsync_WithUncheckedItemConditional_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-radios",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var radiosContext = context.GetContextItem<RadiosContext>();

                    radiosContext.AddItem(new RadiosItem()
                    {
                        LabelContent = new HtmlString("First"),
                        Conditional = new RadiosItemConditional()
                        {
                            Content = new HtmlString("Item 1 conditional")
                        },
                        Id = "first",
                        Value = "first"
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                IdPrefix = "my-id",
                Name = "testradios"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div class=""govuk-form-group"">
    <div class=""govuk-radios"" data-module=""govuk-radios"">
        <div class=""govuk-radios__item"">
            <input class=""govuk-radios__input"" id=""first"" name=""testradios"" type=""radio"" value=""first"" data-aria-controls=""conditional-first"" />
            <label class=""govuk-radios__label govuk-label"" for=""first"">First</label>
        </div>
        <div class=""govuk-radios__conditional--hidden govuk-radios__conditional"" id=""conditional-first"">Item 1 conditional</div>
    </div>
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
        }

        [Fact]
        public async Task ProcessAsync_WithCheckedItemConditional_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-radios",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var radiosContext = context.GetContextItem<RadiosContext>();

                    radiosContext.AddItem(new RadiosItem()
                    {
                        Checked = true,
                        LabelContent = new HtmlString("First"),
                        Conditional = new RadiosItemConditional()
                        {
                            Content = new HtmlString("Item 1 conditional")
                        },
                        Id = "first",
                        Value = "first"
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                IdPrefix = "my-id",
                Name = "testradios"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div class=""govuk-form-group"">
    <div class=""govuk-radios"" data-module=""govuk-radios"">
        <div class=""govuk-radios__item"">
            <input class=""govuk-radios__input"" id=""first"" name=""testradios"" type=""radio"" value=""first"" checked=""checked"" data-aria-controls=""conditional-first"" />
            <label class=""govuk-radios__label govuk-label"" for=""first"">First</label>
        </div>
        <div class=""govuk-radios__conditional"" id=""conditional-first"">Item 1 conditional</div>
    </div>
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
        }

        [Fact]
        public async Task ProcessAsync_WithFieldset_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-radios",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-radios",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var radiosContext = context.GetContextItem<RadiosContext>();

                    radiosContext.OpenFieldset();
                    var radiosFieldsetContext = new RadiosFieldsetContext(attributes: null, aspFor: null);
                    radiosFieldsetContext.SetLegend(isPageHeading: false, attributes: null, content: new HtmlString("Legend"));

                    radiosContext.SetHint(attributes: null, content: new HtmlString("The hint"));

                    radiosContext.AddItem(new RadiosItem()
                    {
                        Checked = false,
                        LabelContent = new HtmlString("First"),
                        Disabled = true,
                        Id = "first",
                        Value = "first"
                    });

                    radiosContext.AddItem(new RadiosItem()
                    {
                        Checked = true,
                        LabelContent = new HtmlString("Second"),
                        Disabled = false,
                        Id = "second",
                        Value = "second"
                    });

                    radiosContext.CloseFieldset(radiosFieldsetContext);

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new RadiosTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                DescribedBy = "describedby",
                IdPrefix = "my-id",
                Name = "testradios"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div class=""govuk-form-group"">
    <fieldset aria-describedby=""describedby my-id-hint"" class=""govuk-fieldset"">
        <legend class=""govuk-fieldset__legend"">Legend</legend>
        <div class=""govuk-hint"" id=""my-id-hint"">The hint</div>
        <div class=""govuk-radios"" data-module=""govuk-radios"">
            <div class=""govuk-radios__item"">
                <input class=""govuk-radios__input"" id=""first"" name=""testradios"" type=""radio"" value=""first"" disabled=""disabled"" />
                <label class=""govuk-radios__label govuk-label"" for=""first"">First</label>
            </div>
            <div class=""govuk-radios__item"">
                <input class=""govuk-radios__input"" id=""second"" name=""testradios"" type=""radio"" value=""second"" checked=""checked"" />
                <label class=""govuk-radios__label govuk-label"" for=""second"">Second</label>
            </div>
        </div>
    </fieldset>
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
        }
    }
}
