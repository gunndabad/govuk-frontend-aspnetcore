using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class DateInputContextTests
{
    [Fact]
    public void OpenFieldset_AlreadyOpen_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, aspFor: null);

        context.OpenFieldset();

        // Act
        var ex = Record.Exception(() => context.OpenFieldset());

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-date-input-fieldset> cannot be nested inside another <govuk-date-input-fieldset>.", ex.Message);
    }

    [Fact]
    public void OpenFieldset_AlreadyGotFieldset_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, aspFor: null);

        context.OpenFieldset();
        context.CloseFieldset(new DateInputFieldsetContext(attributes: null, aspFor: null));

        // Act
        var ex = Record.Exception(() => context.OpenFieldset());

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-date-input-fieldset> element is permitted within each <govuk-date-input>.", ex.Message);
    }

    [Fact]
    public void OpenFieldset_AlreadyGotItem_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, aspFor: null);

        var item = new DateInputContextItem()
        {
            LabelContent = new HtmlString("Day")
        };

        context.SetItem(DateInputItemType.Day, item);

        // Act
        var ex = Record.Exception(() => context.OpenFieldset());

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-date-input-fieldset> must be the only direct child of the <govuk-date-input>.", ex.Message);
    }

    [Fact]
    public void OpenFieldset_AlreadyGotHint_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, aspFor: null);
        context.SetHint(attributes: null, content: new HtmlString("Hint"));

        // Act
        var ex = Record.Exception(() => context.OpenFieldset());

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-date-input-fieldset> must be the only direct child of the <govuk-date-input>.", ex.Message);
    }

    [Fact]
    public void OpenFieldset_AlreadyGotErrorMessage_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, aspFor: null);
        context.SetErrorMessage(errorComponents: null, visuallyHiddenText: null, attributes: null, content: new HtmlString("Error"));

        // Act
        var ex = Record.Exception(() => context.OpenFieldset());

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-date-input-fieldset> must be the only direct child of the <govuk-date-input>.", ex.Message);
    }

    [Fact]
    public void CloseFieldset_FieldsetNotOpened_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, aspFor: null);

        // Act
        var ex = Record.Exception(() => context.CloseFieldset(new DateInputFieldsetContext(attributes: null, aspFor: null)));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Fieldset has not been opened.", ex.Message);
    }

    [Fact]
    public void SetErrorMessage_AlreadyGotItem_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, aspFor: null);

        var item = new DateInputContextItem()
        {
            LabelContent = new HtmlString("Day")
        };

        context.SetItem(DateInputItemType.Day, item);

        // Act
        var ex = Record.Exception(
            () => context.SetErrorMessage(errorComponents: null, visuallyHiddenText: null, attributes: null, new HtmlString("Error")));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-date-input-error-message> must be specified before <govuk-date-input-day>.", ex.Message);
    }

    [Fact]
    public void SetErrorMessage_OutsideOfFieldset_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, aspFor: null);

        var item = new DateInputContextItem()
        {
            LabelContent = new HtmlString("Day")
        };

        context.OpenFieldset();
        var fieldsetContext = new DateInputFieldsetContext(attributes: null, aspFor: null);
        context.CloseFieldset(fieldsetContext);

        // Act
        var ex = Record.Exception(
            () => context.SetErrorMessage(errorComponents: null, visuallyHiddenText: null, attributes: null, new HtmlString("Error")));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-date-input-error-message> must be inside <govuk-date-input-fieldset>.", ex.Message);
    }

    [Fact]
    public void SetHint_AlreadyGotItem_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, aspFor: null);

        var item = new DateInputContextItem()
        {
            LabelContent = new HtmlString("Day")
        };

        context.SetItem(DateInputItemType.Day, item);

        // Act
        var ex = Record.Exception(() => context.SetHint(attributes: null, new HtmlString("Hint")));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-date-input-hint> must be specified before <govuk-date-input-day>.", ex.Message);
    }

    [Fact]
    public void SetHint_OutsideOfFieldset_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, aspFor: null);

        var item = new DateInputContextItem()
        {
            LabelContent = new HtmlString("Day")
        };

        context.OpenFieldset();
        var fieldsetContext = new DateInputFieldsetContext(attributes: null, aspFor: null);
        context.CloseFieldset(fieldsetContext);

        // Act
        var ex = Record.Exception(() => context.SetHint(attributes: null, new HtmlString("Hint")));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-date-input-hint> must be inside <govuk-date-input-fieldset>.", ex.Message);
    }

    [Fact]
    public void SetItem_SetsItemOnContext()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, aspFor: null);

        // Act
        context.SetItem(DateInputItemType.Month, new DateInputContextItem()
        {
            AutoComplete = "off",
            Id = "id",
            InputMode = "im",
            LabelContent = new HtmlString("Month"),
            Name = "name",
            Pattern = "pattern",
            Value = 42,
            ValueSpecified = true
        });

        // Assert
        Assert.Collection(
            context.Items,
            kvp =>
            {
                Assert.Equal(DateInputItemType.Month, kvp.Key);
                var value = kvp.Value;
                Assert.Equal("off", value.AutoComplete);
                Assert.Equal("id", value.Id);
                Assert.Equal("im", value.InputMode);
                Assert.Equal("Month", value.LabelContent?.ToHtmlString());
                Assert.Equal("name", value.Name);
                Assert.Equal("pattern", value.Pattern);
                Assert.Equal(42, value.Value);
                Assert.True(value.ValueSpecified);
            });
    }

    [Fact]
    public void SetItem_WithValueWithTopLevelValue_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: true, aspFor: null);
        context.SetItem(DateInputItemType.Month, new DateInputContextItem());

        // Act
        var ex = Record.Exception(() => context.SetItem(DateInputItemType.Month, new DateInputContextItem() { ValueSpecified = true }));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Value cannot be specified for both <govuk-date-input-month> and the parent <govuk-date-input>.", ex.Message);
    }

    [Fact]
    public void SetItem_AlreadyGotItem_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, aspFor: null);
        context.SetItem(DateInputItemType.Month, new DateInputContextItem());

        // Act
        var ex = Record.Exception(() => context.SetItem(DateInputItemType.Month, new DateInputContextItem()));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-date-input-month> element is permitted within each <govuk-date-input>.", ex.Message);
    }

    [Fact]
    public void SetItem_OutsideOfFieldset_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, aspFor: null);

        context.OpenFieldset();
        var fieldsetContext = new DateInputFieldsetContext(attributes: null, aspFor: null);
        context.CloseFieldset(fieldsetContext);

        // Act
        var ex = Record.Exception(() => context.SetItem(DateInputItemType.Month, new DateInputContextItem()));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-date-input-month> must be inside <govuk-date-input-fieldset>.", ex.Message);
    }

    [Fact]
    public void SetLabel_ThrowsNotSupportedException()
    {
        // Arrange
        var context = new DateInputContext(haveExplicitValue: false, aspFor: null);

        // Act
        var ex = Record.Exception(() => context.SetLabel(isPageHeading: false, attributes: null, content: null));

        // Assert
        Assert.IsType<NotSupportedException>(ex);
    }
}
