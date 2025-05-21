using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.ModelBinding;

internal class DateInputModelConverterModelBinder : IModelBinder
{
    private const string DayInputName = "Day";
    private const string MonthInputName = "Month";
    private const string YearInputName = "Year";

    private readonly DateInputModelConverter _dateInputModelConverter;
    private readonly IOptions<GovUkFrontendAspNetCoreOptions> _optionsAccessor;

    public DateInputModelConverterModelBinder(
        DateInputModelConverter dateInputModelConverter,
        IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
    {
        _dateInputModelConverter = Guard.ArgumentNotNull(nameof(dateInputModelConverter), dateInputModelConverter);
        _optionsAccessor = Guard.ArgumentNotNull(nameof(optionsAccessor), optionsAccessor);
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        Guard.ArgumentNotNull(nameof(bindingContext), bindingContext);

        var modelType = bindingContext.ModelMetadata.UnderlyingOrModelType;
        if (!_dateInputModelConverter.CanConvertModelType(modelType))
        {
            throw new InvalidOperationException($"Cannot bind {modelType.Name}.");
        }

        var dayModelName = ModelNames.CreatePropertyModelName(bindingContext.ModelName, DayInputName);
        var monthModelName = ModelNames.CreatePropertyModelName(bindingContext.ModelName, MonthInputName);
        var yearModelName = ModelNames.CreatePropertyModelName(bindingContext.ModelName, YearInputName);

        var dayValueProviderResult = bindingContext.ValueProvider.GetValue(dayModelName);
        var monthValueProviderResult = bindingContext.ValueProvider.GetValue(monthModelName);
        var yearValueProviderResult = bindingContext.ValueProvider.GetValue(yearModelName);

        if ((dayValueProviderResult == ValueProviderResult.None || dayValueProviderResult.FirstValue == string.Empty) &&
            (monthValueProviderResult == ValueProviderResult.None || monthValueProviderResult.FirstValue == string.Empty) &&
            (yearValueProviderResult == ValueProviderResult.None || yearValueProviderResult.FirstValue == string.Empty))
        {
            return Task.CompletedTask;
        }

        var parseErrors = Parse(
            dayValueProviderResult.FirstValue,
            monthValueProviderResult.FirstValue,
            yearValueProviderResult.FirstValue,
            _optionsAccessor.Value.AcceptMonthNamesInDateInputs,
            out var date);

        if (parseErrors == DateInputParseErrors.None)
        {
            Debug.Assert(date.HasValue);
            var model = _dateInputModelConverter.CreateModelFromDate(modelType, date!.Value);
            bindingContext.Result = ModelBindingResult.Success(model);
        }
        else
        {
            var parseErrorsProvider = bindingContext.HttpContext.RequestServices.GetRequiredService<DateInputParseErrorsProvider>();
            parseErrorsProvider.SetErrorsForModel(bindingContext.ModelName, parseErrors);

            bindingContext.ModelState.SetModelValue(dayModelName, dayValueProviderResult);
            bindingContext.ModelState.SetModelValue(monthModelName, monthValueProviderResult);
            bindingContext.ModelState.SetModelValue(yearModelName, yearValueProviderResult);

            var errorMessage = GetModelStateErrorMessage(parseErrors, bindingContext.ModelMetadata);
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, errorMessage);

            bindingContext.Result = ModelBindingResult.Failed();
        }

        return Task.CompletedTask;
    }

    // internal for testing
    internal static string GetModelStateErrorMessage(DateInputParseErrors parseErrors, ModelMetadata modelMetadata)
    {
        // TODO Make these messages configurable
        // (see Microsoft.AspNetCore.Mvc.ModelBinding.Metadata.ModelBindingMessageProvider)

        Debug.Assert(parseErrors != DateInputParseErrors.None);
        Debug.Assert(parseErrors != (DateInputParseErrors.MissingDay | DateInputParseErrors.MissingMonth | DateInputParseErrors.MissingYear));

        var dateInputModelMetadata = modelMetadata.AdditionalValues.TryGetValue(typeof(DateInputModelMetadata), out var metadataObj) &&
            metadataObj is DateInputModelMetadata dimm ? dimm :
            null;

        var displayName = dateInputModelMetadata?.ErrorMessagePrefix ?? modelMetadata.DisplayName ?? modelMetadata.PropertyName;

        var missingFields = new List<string>();

        if ((parseErrors & DateInputParseErrors.MissingDay) != 0)
        {
            missingFields.Add("day");
        }
        if ((parseErrors & DateInputParseErrors.MissingMonth) != 0)
        {
            missingFields.Add("month");
        }
        if ((parseErrors & DateInputParseErrors.MissingYear) != 0)
        {
            missingFields.Add("year");
        }

        if (missingFields.Count > 0)
        {
            Debug.Assert(missingFields.Count <= 2);
            return $"{displayName} must include a {string.Join(" and ", missingFields)}";
        }

        return $"{displayName} must be a real date";
    }

    // internal for testing
    internal static DateInputParseErrors Parse(string? day, string? month, string? year, bool acceptMonthNames, out DateOnly? date)
    {
        day ??= string.Empty;
        month ??= string.Empty;
        year ??= string.Empty;

        var errors = DateInputParseErrors.None;
        int parsedYear = 0, parsedMonth = 0, parsedDay = 0;

        if (string.IsNullOrEmpty(year))
        {
            errors |= DateInputParseErrors.MissingYear;
        }
        else if (year.Length != 4)
        {
            errors |= DateInputParseErrors.InvalidYear;
        }
        else if (!TryParseYear(year, out parsedYear))
        {
            errors |= DateInputParseErrors.InvalidYear;
        }

        if (string.IsNullOrEmpty(month))
        {
            errors |= DateInputParseErrors.MissingMonth;
        }
        else if (!TryParseMonth(month, out parsedMonth) || parsedMonth < 1 || parsedMonth > 12)
        {
            errors |= DateInputParseErrors.InvalidMonth;
        }

        if (string.IsNullOrEmpty(day))
        {
            errors |= DateInputParseErrors.MissingDay;
        }
        else if (!TryParseDay(day, out parsedDay) || parsedDay < 1 || parsedDay > 31 ||
            (errors == DateInputParseErrors.None && parsedDay > DateTime.DaysInMonth(parsedYear, parsedMonth)))
        {
            errors |= DateInputParseErrors.InvalidDay;
        }

        date = errors == DateInputParseErrors.None ? new(parsedYear, parsedMonth, parsedDay) : default;
        return errors;

        bool TryParseDay(string value, out int result) => int.TryParse(value, out result);

        bool TryParseMonth(string value, out int result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = 0;
                return false;
            }

            if (!int.TryParse(value, out result) && acceptMonthNames)
            {
                result = value.ToLowerInvariant() switch
                {
                    "jan" => 1,
                    "january" => 1,
                    "feb" => 2,
                    "february" => 2,
                    "mar" => 3,
                    "march" => 3,
                    "apr" => 4,
                    "april" => 4,
                    "may" => 5,
                    "jun" => 6,
                    "june" => 6,
                    "jul" => 7,
                    "july" => 7,
                    "aug" => 8,
                    "august" => 8,
                    "sep" => 9,
                    "september" => 9,
                    "oct" => 10,
                    "october" => 10,
                    "nov" => 11,
                    "november" => 11,
                    "dec" => 12,
                    "december" => 12,
                    _ => 0
                };
            }

            return result != 0;
        }

        bool TryParseYear(string value, out int result) => int.TryParse(value, out result);
    }
}
