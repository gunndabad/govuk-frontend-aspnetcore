#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace GovUk.Frontend.AspNetCore.ModelBinding
{
    internal class DateInputModelBinder : IModelBinder
    {
        private const string DayComponentName = "Day";
        private const string MonthComponentName = "Month";
        private const string YearComponentName = "Year";

        private readonly DateInputModelConverter _dateInputModelConverter;

        public DateInputModelBinder(DateInputModelConverter dateInputModelConverter)
        {
            _dateInputModelConverter = Guard.ArgumentNotNull(nameof(dateInputModelConverter), dateInputModelConverter);
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Guard.ArgumentNotNull(nameof(bindingContext), bindingContext);

            var modelType = bindingContext.ModelType;
            if (!_dateInputModelConverter.CanConvertModelType(modelType))
            {
                throw new InvalidOperationException($"Cannot bind {modelType.Name}.");
            }

            var dayModelName = $"{bindingContext.ModelName}.{DayComponentName}";
            var monthModelName = $"{bindingContext.ModelName}.{MonthComponentName}";
            var yearModelName = $"{bindingContext.ModelName}.{YearComponentName}";

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

                if (_dateInputModelConverter.TryCreateModelFromErrors(modelType, parseErrors, out var model))
                {
                    bindingContext.Result = ModelBindingResult.Success(model);
                }
                else
                {
                    var errorMessage = GetModelStateErrorMessage(parseErrors, bindingContext.ModelMetadata);
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, errorMessage);

                    bindingContext.Result = ModelBindingResult.Failed();
                }
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

            var displayName = modelMetadata.DisplayName ?? modelMetadata.PropertyName;

            var missingComponents = new List<string>();

            if ((parseErrors & DateInputParseErrors.MissingDay) != 0)
            {
                missingComponents.Add("day");
            }
            if ((parseErrors & DateInputParseErrors.MissingMonth) != 0)
            {
                missingComponents.Add("month");
            }
            if ((parseErrors & DateInputParseErrors.MissingYear) != 0)
            {
                missingComponents.Add("year");
            }

            if (missingComponents.Count > 0)
            {
                Debug.Assert(missingComponents.Count <= 2);
                return $"{displayName} must include a {string.Join(" and ", missingComponents)}";
            }

            return $"{displayName} must be a real date";
        }

        // internal for testing
        internal static DateInputParseErrors Parse(string? day, string? month, string? year, out Date? date)
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
            else if (!int.TryParse(year, out parsedYear) || parsedYear < 1 || parsedYear > 9999)
            {
                errors |= DateInputParseErrors.InvalidYear;
            }

            if (string.IsNullOrEmpty(month))
            {
                errors |= DateInputParseErrors.MissingMonth;
            }
            else if (!int.TryParse(month, out parsedMonth) || parsedMonth < 1 || parsedMonth > 12)
            {
                errors |= DateInputParseErrors.InvalidMonth;
            }

            if (string.IsNullOrEmpty(day))
            {
                errors |= DateInputParseErrors.MissingDay;
            }
            else if (!int.TryParse(day, out parsedDay) || parsedDay < 1 || parsedDay > 31 ||
                (errors == DateInputParseErrors.None && parsedDay > DateTime.DaysInMonth(parsedYear, parsedMonth)))
            {
                errors |= DateInputParseErrors.InvalidDay;
            }

            date = errors == DateInputParseErrors.None ? new(parsedYear, parsedMonth, parsedDay) : default;
            return errors;
        }
    }
}
