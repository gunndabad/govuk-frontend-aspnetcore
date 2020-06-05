using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GovUk.Frontend.AspNetCore.ModelBinding
{
    public class DateInputModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var modelType = context.Metadata.ModelType;
            if (modelType == typeof(Date) || modelType == typeof(Date?))
            {
                return new DateInputModelBinder();
            }

            return null;
        }
    }

    public class DateInputModelBinder : IModelBinder
    {
        internal const string DayComponentName = "Day";
        internal const string MonthComponentName = "Month";
        internal const string YearComponentName = "Year";

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelType = bindingContext.ModelType;
            if (modelType != typeof(Date) && modelType != typeof(Date?))
            {
                throw new InvalidOperationException($"Cannot bind {modelType.Name}.");
            }

            var dayModelName = $"{bindingContext.ModelName}.{DayComponentName}";
            var monthModelName = $"{bindingContext.ModelName}.{MonthComponentName}";
            var yearModelName = $"{bindingContext.ModelName}.{YearComponentName}";

            var dayValueProviderResult = bindingContext.ValueProvider.GetValue(dayModelName);
            var monthValueProviderResult = bindingContext.ValueProvider.GetValue(monthModelName);
            var yearValueProviderResult = bindingContext.ValueProvider.GetValue(yearModelName);

            // If all components are empty and ModelType is nullable then bind null to result
            if (dayValueProviderResult == ValueProviderResult.None &&
                monthValueProviderResult == ValueProviderResult.None &&
                yearValueProviderResult == ValueProviderResult.None)
            {
                if (modelType == typeof(Date?))
                {
                    bindingContext.Result = ModelBindingResult.Success(null);
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                }

                return Task.CompletedTask;
            }

            int day = -1;
            int month = -1;
            int year = -1;

            if (TryParseYear() && TryParseMonth() && TryParseDay())
            {
                var date = new Date(year, month, day);
                bindingContext.Result = ModelBindingResult.Success(date);
            }
            else
            {
                bindingContext.ModelState.SetModelValue(dayModelName, dayValueProviderResult);
                bindingContext.ModelState.SetModelValue(monthModelName, monthValueProviderResult);
                bindingContext.ModelState.SetModelValue(yearModelName, yearValueProviderResult);

                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    "Invalid date specified.");

                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;

            bool TryParseDay()
            {
                if (dayValueProviderResult != ValueProviderResult.None &&
                    dayValueProviderResult.FirstValue != string.Empty)
                {
                    if (!int.TryParse(dayValueProviderResult.FirstValue, out day) ||
                        day < 1 ||
                        (month != -1 && year != -1 && day > DateTime.DaysInMonth(year, month)))
                    {
                        bindingContext.ModelState.TryAddModelError(
                            dayModelName,
                            "Day is not valid.");

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    bindingContext.ModelState.TryAddModelError(
                        dayModelName,
                        "Day is missing.");

                    return false;
                }
            }

            bool TryParseMonth()
            {
                if (monthValueProviderResult != ValueProviderResult.None &&
                    monthValueProviderResult.FirstValue != string.Empty)
                {
                    if (!int.TryParse(monthValueProviderResult.FirstValue, out month) ||
                        month < 1 ||
                        month > 12)
                    {
                        bindingContext.ModelState.TryAddModelError(
                            monthModelName,
                            "Month is not valid.");

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    bindingContext.ModelState.TryAddModelError(
                        monthModelName,
                        "Month is missing.");

                    return false;
                }
            }

            bool TryParseYear()
            {
                if (yearValueProviderResult != ValueProviderResult.None &&
                    yearValueProviderResult.FirstValue != string.Empty)
                {
                    if (!int.TryParse(yearValueProviderResult.FirstValue, out year) ||
                        year < 1 ||
                        year > 9999)
                    {
                        bindingContext.ModelState.TryAddModelError(
                            yearModelName,
                            "Year is not valid.");

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    bindingContext.ModelState.TryAddModelError(
                        yearModelName,
                        "Year is missing.");

                    return false;
                }
            }
        }
    }
}
