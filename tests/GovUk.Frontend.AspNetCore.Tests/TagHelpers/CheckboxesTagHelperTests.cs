using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class CheckboxesTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_GeneratesExpectedOutput()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-checkboxes",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-checkboxes",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var checkboxesContext = context.GetContextItem<CheckboxesContext>();

                checkboxesContext.SetHint(attributes: null, content: new HtmlString("The hint"));

                checkboxesContext.AddItem(
                    new CheckboxesItem()
                    {
                        Checked = false,
                        LabelContent = new HtmlString("First"),
                        Disabled = true,
                        Id = "first",
                        Value = "first",
                    }
                );

                checkboxesContext.AddItem(
                    new CheckboxesItem()
                    {
                        Checked = true,
                        LabelContent = new HtmlString("Second"),
                        Disabled = false,
                        Id = "second",
                        Value = "second",
                    }
                );

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new CheckboxesTagHelper(new ComponentGenerator(), new DefaultModelHelper())
        {
            IdPrefix = "my-id",
            Name = "testcheckboxes",
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml =
            @"
<div class=""govuk-form-group"">
    <div class=""govuk-hint"" id=""my-id-hint"">The hint</div>
    <div class=""govuk-checkboxes"" data-module=""govuk-checkboxes"">
        <div class=""govuk-checkboxes__item"">
            <input class=""govuk-checkboxes__input"" id=""first"" name=""testcheckboxes"" type=""checkbox"" value=""first"" aria-describedby=""my-id-hint"" disabled=""disabled"" />
            <label class=""govuk-checkboxes__label govuk-label"" for=""first"">First</label>
        </div>
        <div class=""govuk-checkboxes__item"">
            <input class=""govuk-checkboxes__input"" id=""second"" name=""testcheckboxes"" type=""checkbox"" value=""second"" aria-describedby=""my-id-hint"" checked=""checked"" />
            <label class=""govuk-checkboxes__label govuk-label"" for=""second"">Second</label>
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
            tagName: "govuk-checkboxes",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-checkboxes",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var checkboxesContext = context.GetContextItem<CheckboxesContext>();

                checkboxesContext.SetErrorMessage(
                    visuallyHiddenText: null,
                    attributes: null,
                    content: new HtmlString("A error")
                );

                checkboxesContext.AddItem(
                    new CheckboxesItem()
                    {
                        Checked = false,
                        LabelContent = new HtmlString("First"),
                        Disabled = true,
                        Id = "first",
                        Value = "first",
                    }
                );

                checkboxesContext.AddItem(
                    new CheckboxesItem()
                    {
                        Checked = true,
                        LabelContent = new HtmlString("Second"),
                        Disabled = false,
                        Id = "second",
                        Value = "second",
                    }
                );

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new CheckboxesTagHelper(new ComponentGenerator(), new DefaultModelHelper())
        {
            IdPrefix = "my-id",
            Name = "testcheckboxes",
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml =
            @"
<div class=""govuk-form-group govuk-form-group--error"">
    <p class=""govuk-error-message"" id=""my-id-error""><span class=""govuk-visually-hidden"">Error:</span>A error</p>
    <div class=""govuk-checkboxes"" data-module=""govuk-checkboxes"">
        <div class=""govuk-checkboxes__item"">
            <input class=""govuk-checkboxes__input"" id=""first"" name=""testcheckboxes"" type=""checkbox"" value=""first"" aria-describedby=""my-id-error"" disabled=""disabled"" />
            <label class=""govuk-checkboxes__label govuk-label"" for=""first"">First</label>
        </div>
        <div class=""govuk-checkboxes__item"">
            <input class=""govuk-checkboxes__input"" id=""second"" name=""testcheckboxes"" type=""checkbox"" value=""second"" aria-describedby=""my-id-error"" checked=""checked"" />
            <label class=""govuk-checkboxes__label govuk-label"" for=""second"">Second</label>
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
            tagName: "govuk-checkboxes",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-checkboxes",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var checkboxesContext = context.GetContextItem<CheckboxesContext>();

                checkboxesContext.AddItem(
                    new CheckboxesItem()
                    {
                        LabelContent = new HtmlString("First"),
                        Hint = new CheckboxesItemHint() { Content = new HtmlString("First item hint") },
                        Id = "first",
                        Value = "first",
                    }
                );

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new CheckboxesTagHelper(new ComponentGenerator(), new DefaultModelHelper())
        {
            IdPrefix = "my-id",
            Name = "testcheckboxes",
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml =
            @"
<div class=""govuk-form-group"">
    <div class=""govuk-checkboxes"" data-module=""govuk-checkboxes"">
        <div class=""govuk-checkboxes__item"">
            <input class=""govuk-checkboxes__input"" id=""first"" name=""testcheckboxes"" type=""checkbox"" value=""first"" aria-describedby=""first-item-hint"" />
            <label class=""govuk-checkboxes__label govuk-label"" for=""first"">First</label>
            <div class=""govuk-checkboxes__hint govuk-hint"" id=""first-item-hint"">First item hint</div>
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
            tagName: "govuk-checkboxes",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-checkboxes",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var checkboxesContext = context.GetContextItem<CheckboxesContext>();

                checkboxesContext.AddItem(
                    new CheckboxesItem()
                    {
                        LabelContent = new HtmlString("First"),
                        Conditional = new CheckboxesItemConditional()
                        {
                            Content = new HtmlString("Item 1 conditional"),
                        },
                        Id = "first",
                        Value = "first",
                    }
                );

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new CheckboxesTagHelper(new ComponentGenerator(), new DefaultModelHelper())
        {
            IdPrefix = "my-id",
            Name = "testcheckboxes",
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml =
            @"
<div class=""govuk-form-group"">
    <div class=""govuk-checkboxes"" data-module=""govuk-checkboxes"">
        <div class=""govuk-checkboxes__item"">
            <input class=""govuk-checkboxes__input"" id=""first"" name=""testcheckboxes"" type=""checkbox"" value=""first"" data-aria-controls=""conditional-first"" />
            <label class=""govuk-checkboxes__label govuk-label"" for=""first"">First</label>
        </div>
        <div class=""govuk-checkboxes__conditional--hidden govuk-checkboxes__conditional"" id=""conditional-first"">Item 1 conditional</div>
    </div>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithCheckedItemConditional_GeneratesExpectedOutput()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-checkboxes",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-checkboxes",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var checkboxesContext = context.GetContextItem<CheckboxesContext>();

                checkboxesContext.AddItem(
                    new CheckboxesItem()
                    {
                        Checked = true,
                        LabelContent = new HtmlString("First"),
                        Conditional = new CheckboxesItemConditional()
                        {
                            Content = new HtmlString("Item 1 conditional"),
                        },
                        Id = "first",
                        Value = "first",
                    }
                );

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new CheckboxesTagHelper(new ComponentGenerator(), new DefaultModelHelper())
        {
            IdPrefix = "my-id",
            Name = "testcheckboxes",
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml =
            @"
<div class=""govuk-form-group"">
    <div class=""govuk-checkboxes"" data-module=""govuk-checkboxes"">
        <div class=""govuk-checkboxes__item"">
            <input class=""govuk-checkboxes__input"" id=""first"" name=""testcheckboxes"" type=""checkbox"" value=""first"" checked=""checked"" data-aria-controls=""conditional-first"" />
            <label class=""govuk-checkboxes__label govuk-label"" for=""first"">First</label>
        </div>
        <div class=""govuk-checkboxes__conditional"" id=""conditional-first"">Item 1 conditional</div>
    </div>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithFieldset_GeneratesExpectedOutput()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-checkboxes",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-checkboxes",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var checkboxesContext = context.GetContextItem<CheckboxesContext>();

                checkboxesContext.OpenFieldset();
                var checkboxesFieldsetContext = new CheckboxesFieldsetContext(attributes: null, aspFor: null);
                checkboxesFieldsetContext.SetLegend(
                    isPageHeading: false,
                    attributes: null,
                    content: new HtmlString("Legend")
                );

                checkboxesContext.SetHint(attributes: null, content: new HtmlString("The hint"));

                checkboxesContext.AddItem(
                    new CheckboxesItem()
                    {
                        Checked = false,
                        LabelContent = new HtmlString("First"),
                        Disabled = true,
                        Id = "first",
                        Value = "first",
                    }
                );

                checkboxesContext.AddItem(
                    new CheckboxesItem()
                    {
                        Checked = true,
                        LabelContent = new HtmlString("Second"),
                        Disabled = false,
                        Id = "second",
                        Value = "second",
                    }
                );

                checkboxesContext.CloseFieldset(checkboxesFieldsetContext);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new CheckboxesTagHelper(new ComponentGenerator(), new DefaultModelHelper())
        {
            DescribedBy = "describedby",
            IdPrefix = "my-id",
            Name = "testcheckboxes",
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml =
            @"
<div class=""govuk-form-group"">
    <fieldset aria-describedby=""describedby my-id-hint"" class=""govuk-fieldset"">
        <legend class=""govuk-fieldset__legend"">Legend</legend>
        <div class=""govuk-hint"" id=""my-id-hint"">The hint</div>
        <div class=""govuk-checkboxes"" data-module=""govuk-checkboxes"">
            <div class=""govuk-checkboxes__item"">
                <input class=""govuk-checkboxes__input"" id=""first"" name=""testcheckboxes"" type=""checkbox"" value=""first"" disabled=""disabled"" />
                <label class=""govuk-checkboxes__label govuk-label"" for=""first"">First</label>
            </div>
            <div class=""govuk-checkboxes__item"">
                <input class=""govuk-checkboxes__input"" id=""second"" name=""testcheckboxes"" type=""checkbox"" value=""second"" checked=""checked"" />
                <label class=""govuk-checkboxes__label govuk-label"" for=""second"">Second</label>
            </div>
        </div>
    </fieldset>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }
}
