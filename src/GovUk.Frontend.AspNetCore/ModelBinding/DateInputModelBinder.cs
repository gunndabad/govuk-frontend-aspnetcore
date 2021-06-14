using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace GovUk.Frontend.AspNetCore.ModelBinding
{
    public class DateInputModelBinderProvider : IModelBinderProvider
    {
        private readonly DateInputModelConverter[] _dateInputModelConverters;

        public DateInputModelBinderProvider(GovUkFrontendAspNetCoreOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _dateInputModelConverters = options.DateInputModelConverters.ToArray();
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var modelType = context.Metadata.ModelType;

            foreach (var converter in _dateInputModelConverters)
            {
                if (converter.CanConvertModelType(modelType))
                {
                    return new DateInputModelBinder(converter);
                }
            }

            return null;
        }
    }

    public class DateInputModelBinder : IModelBinder
    {
        internal const string DayComponentName = "Day";
        internal const string MonthComponentName = "Month";
        internal const string YearComponentName = "Year";

        private readonly DateInputModelConverter _dateInputModelConverter;

        public DateInputModelBinder(DateInputModelConverter dateInputModelConverter)
        {
            _dateInputModelConverter = dateInputModelConverter ?? throw new ArgumentNullException(nameof(dateInputModelConverter));
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

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
                out var dateComponents);

            if (parseErrors == DateInputParseErrors.None)
            {
                var model = _dateInputModelConverter.CreateModelFromElements(modelType, dateComponents);
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

            var displayName = modelMetadata.DisplayName;

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
        internal static DateInputParseErrors Parse(
            string day,
            string month,
            string year,
            out (int Day, int Month, int Year) dateComponents)
        {
            if (year is null)
            {
                throw new ArgumentNullException(nameof(year));
            }

            if (month is null)
            {
                throw new ArgumentNullException(nameof(month));
            }

            if (day is null)
            {
                throw new ArgumentNullException(nameof(day));
            }

            var errors = DateInputParseErrors.None;

            if (!int.TryParse(year, out int parsedYear))
            {
                errors |= DateInputParseErrors.MissingYear;
            }
            else if (parsedYear < 1 || parsedYear > 9999)
            {
                errors |= DateInputParseErrors.InvalidYear;
            }

            if (!int.TryParse(month, out int parsedMonth))
            {
                errors |= DateInputParseErrors.MissingMonth;
            }
            else if (parsedMonth < 1 || parsedMonth > 12)
            {
                errors |= DateInputParseErrors.InvalidMonth;
            }

            if (!int.TryParse(day, out int parsedDay))
            {
                errors |= DateInputParseErrors.MissingDay;
            }
            else if (parsedDay < 1 || parsedDay > 31 ||
                (errors == DateInputParseErrors.None && parsedDay > DateTime.DaysInMonth(parsedYear, parsedMonth)))
            {
                errors |= DateInputParseErrors.InvalidDay;
            }

            dateComponents = errors == DateInputParseErrors.None ? (parsedDay, parsedMonth, parsedYear) : default;
            return errors;
        }
    }
}
