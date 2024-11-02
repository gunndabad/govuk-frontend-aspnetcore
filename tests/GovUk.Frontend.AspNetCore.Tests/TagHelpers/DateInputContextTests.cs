using System;
using System.Collections.Immutable;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class DateInputContextTests
{
    [Fact]
    public void OpenFieldset_AlreadyOpen_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, @for: null);

        context.OpenFieldset();

        // Act
        var ex = Record.Exception(() => context.OpenFieldset());

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<fieldset> cannot be nested inside another <fieldset>.", ex.Message);
    }

    [Fact]
    public void OpenFieldset_AlreadyGotFieldset_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, @for: null);

        context.OpenFieldset();
        context.CloseFieldset(new DateInputFieldsetContext(attributes: ImmutableDictionary<string, string?>.Empty, @for: null, describedBy: null));

        // Act
        var ex = Record.Exception(() => context.OpenFieldset());

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <fieldset> element is permitted within each <govuk-date-input>.", ex.Message);
    }

    [Fact]
    public void OpenFieldset_AlreadyGotItem_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, @for: null);

        var item = new DateInputContextItem()
        {
            LabelHtml = "Day"
        };

        context.SetItem(DateInputItemType.Day, item);

        // Act
        var ex = Record.Exception(() => context.OpenFieldset());

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<fieldset> must be the only direct child of the <govuk-date-input>.", ex.Message);
    }

    [Fact]
    public void OpenFieldset_AlreadyGotHint_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, @for: null);
        context.SetHint(attributes: ImmutableDictionary<string, string?>.Empty, html: "Hint");

        // Act
        var ex = Record.Exception(() => context.OpenFieldset());

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<fieldset> must be the only direct child of the <govuk-date-input>.", ex.Message);
    }

    [Fact]
    public void OpenFieldset_AlreadyGotErrorMessage_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, @for: null);
        context.SetErrorMessage(errorFields: null, visuallyHiddenText: null, attributes: ImmutableDictionary<string, string?>.Empty, html: "Error");

        // Act
        var ex = Record.Exception(() => context.OpenFieldset());

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<fieldset> must be the only direct child of the <govuk-date-input>.", ex.Message);
    }

    [Fact]
    public void CloseFieldset_FieldsetNotOpened_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, @for: null);

        // Act
        var ex = Record.Exception(() => context.CloseFieldset(
            new DateInputFieldsetContext(attributes: ImmutableDictionary<string, string?>.Empty, @for: null, describedBy: null)));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Fieldset has not been opened.", ex.Message);
    }

    [Fact]
    public void SetErrorMessage_AlreadyGotItem_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, @for: null);

        var item = new DateInputContextItem()
        {
            LabelHtml = "Day"
        };

        context.SetItem(DateInputItemType.Day, item);

        // Act
        var ex = Record.Exception(
            () => context.SetErrorMessage(errorFields: null, visuallyHiddenText: null, attributes: ImmutableDictionary<string, string?>.Empty, "Error"));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<error-message> must be specified before <day>.", ex.Message);
    }

    [Fact]
    public void SetErrorMessage_OutsideOfFieldset_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, @for: null);

        var item = new DateInputContextItem()
        {
            LabelHtml = "Day"
        };

        context.OpenFieldset();
        var fieldsetContext = new DateInputFieldsetContext(attributes: ImmutableDictionary<string, string?>.Empty, @for: null, describedBy: null);
        context.CloseFieldset(fieldsetContext);

        // Act
        var ex = Record.Exception(
            () => context.SetErrorMessage(errorFields: null, visuallyHiddenText: null, attributes: ImmutableDictionary<string, string?>.Empty, "Error"));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<error-message> must be inside <fieldset>.", ex.Message);
    }

    [Fact]
    public void SetHint_AlreadyGotItem_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, @for: null);

        var item = new DateInputContextItem()
        {
            LabelHtml = "Day"
        };

        context.SetItem(DateInputItemType.Day, item);

        // Act
        var ex = Record.Exception(() => context.SetHint(attributes: ImmutableDictionary<string, string?>.Empty, "Hint"));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<hint> must be specified before <day>.", ex.Message);
    }

    [Fact]
    public void SetHint_OutsideOfFieldset_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, @for: null);

        var item = new DateInputContextItem()
        {
            LabelHtml = "Day"
        };

        context.OpenFieldset();
        var fieldsetContext = new DateInputFieldsetContext(attributes: ImmutableDictionary<string, string?>.Empty, @for: null, describedBy: null);
        context.CloseFieldset(fieldsetContext);

        // Act
        var ex = Record.Exception(() => context.SetHint(attributes: ImmutableDictionary<string, string?>.Empty, "Hint"));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<hint> must be inside <fieldset>.", ex.Message);
    }

    [Fact]
    public void SetItem_SetsItemOnContext()
    {
        // Arrange
        var autocomplete = "off";
        var id = "id";
        var inputMode = "im";
        var labelHtml = "Month";
        var name = "name";
        var pattern = "Pattern";
        var value = 42;

        var context = new DateInputContext(haveExplicitValue: false, @for: null);

        // Act
        context.SetItem(DateInputItemType.Month, new DateInputContextItem()
        {
            Autocomplete = autocomplete,
            Id = id,
            InputMode = inputMode,
            LabelHtml = labelHtml,
            Name = name,
            Pattern = pattern,
            Value = value,
            ValueSpecified = true
        });

        // Assert
        Assert.Collection(
            context.Items,
            kvp =>
            {
                Assert.Equal(DateInputItemType.Month, kvp.Key);
                var item = kvp.Value;
                Assert.Equal(autocomplete, item.Autocomplete);
                Assert.Equal(id, item.Id);
                Assert.Equal(inputMode, item.InputMode);
                Assert.Equal(labelHtml, item.LabelHtml);
                Assert.Equal(name, item.Name);
                Assert.Equal(pattern, item.Pattern);
                Assert.Equal(value, item.Value);
                Assert.True(item.ValueSpecified);
            });
    }

    [Fact]
    public void SetItem_WithValueWithTopLevelValue_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: true, @for: null);
        context.SetItem(DateInputItemType.Month, new DateInputContextItem());

        // Act
        var ex = Record.Exception(() => context.SetItem(DateInputItemType.Month, new DateInputContextItem() { ValueSpecified = true }));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Value cannot be specified for both <month> and the parent <govuk-date-input>.", ex.Message);
    }

    [Fact]
    public void SetItem_AlreadyGotItem_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, @for: null);
        context.SetItem(DateInputItemType.Month, new DateInputContextItem());

        // Act
        var ex = Record.Exception(() => context.SetItem(DateInputItemType.Month, new DateInputContextItem()));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <month> element is permitted within each <govuk-date-input>.", ex.Message);
    }

    [Fact]
    public void SetItem_OutsideOfFieldset_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, @for: null);

        context.OpenFieldset();
        var fieldsetContext = new DateInputFieldsetContext(attributes: ImmutableDictionary<string, string?>.Empty, @for: null, describedBy: null);
        context.CloseFieldset(fieldsetContext);

        // Act
        var ex = Record.Exception(() => context.SetItem(DateInputItemType.Month, new DateInputContextItem()));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<month> must be inside <fieldset>.", ex.Message);
    }

    [Fact]
    public void SetLabel_ThrowsNotSupportedException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, @for: null);

        // Act
        var ex = Record.Exception(() => context.SetLabel(isPageHeading: false, attributes: ImmutableDictionary<string, string?>.Empty, html: null));

        // Assert
        Assert.IsType<NotSupportedException>(ex);
    }
}
