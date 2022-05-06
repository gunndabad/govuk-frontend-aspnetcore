using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using Xunit;

namespace GovUk.Frontend.AspNetCore.IntegrationTests
{
    public class DateInputTests : IClassFixture<DateInputTestsFixture>
    {
        public DateInputTests(DateInputTestsFixture fixture)
        {
            Browser = fixture.Browser;
        }

        public IBrowser Browser { get; }

        [Fact]
        public async Task AspForDateProperty()
        {
            var page = await Browser.NewPageAsync(new BrowserNewPageOptions() { BaseURL = ServerFixture.BaseUrl });

            // Initial values should come from AspFor property
            await page.GotoAsync("/DateInputTests/AspForDate");
            await AssertFields(page, "1", "4", "2020");

            // Change the values and POST them, including some invalid values
            var day = "x";
            var month = "y";
            var year = "2022";

            await page.FillAsync("[name='Date.Day']", day);
            await page.FillAsync("[name='Date.Month']", month);
            await page.FillAsync("[name='Date.Year']", year);

            await page.RunAndWaitForNavigationAsync(() => page.Keyboard.PressAsync("Enter"));

            // Verify POSTed values have been round-tripped, including the invalid values
            await AssertFields(page, day, month, year, expectDayToHaveError: true, expectMonthToHaveError: true, expectYearToHaveError: false, expectedErrorMessage: "Date of birth must be a real date");
        }

        [Fact]
        public async Task AspForCustomDateProperty()
        {
            var page = await Browser.NewPageAsync(new BrowserNewPageOptions() { BaseURL = ServerFixture.BaseUrl });

            // Initial values should come from AspFor property
            await page.GotoAsync("/DateInputTests/AspForCustomDate");
            await AssertFields(page, "1", "4", "2020");

            // Change the values and POST them, including some invalid values
            var day = "x";
            var month = "y";
            var year = "2022";

            await page.FillAsync("[name='CustomDate.Day']", day);
            await page.FillAsync("[name='CustomDate.Month']", month);
            await page.FillAsync("[name='CustomDate.Year']", year);

            await page.RunAndWaitForNavigationAsync(() => page.Keyboard.PressAsync("Enter"));

            // Verify POSTed values have been round-tripped, including the invalid values
            await AssertFields(page, day, month, year, expectDayToHaveError: true, expectMonthToHaveError: true, expectYearToHaveError: false, expectedErrorMessage: "Date of birth must be a real date");
        }

        [Fact]
        public async Task ValueDateProperty()
        {
            var page = await Browser.NewPageAsync(new BrowserNewPageOptions() { BaseURL = ServerFixture.BaseUrl });

            // Initial values should come from AspFor property
            await page.GotoAsync("/DateInputTests/ValueDate");
            await AssertFields(page, "1", "4", "2020");

            // Change the values and POST them
            var day = "3";
            var month = "4";
            var year = "2022";

            await page.FillAsync("[name='Date.Day']", day);
            await page.FillAsync("[name='Date.Month']", month);
            await page.FillAsync("[name='Date.Year']", year);

            await page.RunAndWaitForNavigationAsync(() => page.Keyboard.PressAsync("Enter"));

            // Verify POSTed values have been round-tripped
            await AssertFields(page, day, month, year);
        }

        [Fact]
        public async Task ValueCustomDateProperty()
        {
            var page = await Browser.NewPageAsync(new BrowserNewPageOptions() { BaseURL = ServerFixture.BaseUrl });

            // Initial values should come from AspFor property
            await page.GotoAsync("/DateInputTests/ValueCustomDate");
            await AssertFields(page, "1", "4", "2020");

            // Change the values and POST them
            var day = "3";
            var month = "4";
            var year = "2022";

            await page.FillAsync("[name='CustomDate.Day']", day);
            await page.FillAsync("[name='CustomDate.Month']", month);
            await page.FillAsync("[name='CustomDate.Year']", year);

            await page.RunAndWaitForNavigationAsync(() => page.Keyboard.PressAsync("Enter"));

            // Verify POSTed values have been round-tripped
            await AssertFields(page, day, month, year);
        }

        [Fact]
        public async Task IndividualItemValues()
        {
            var page = await Browser.NewPageAsync(new BrowserNewPageOptions() { BaseURL = ServerFixture.BaseUrl });

            // Initial values should come from item Value properties
            await page.GotoAsync("/DateInputTests/ItemValues");
            await AssertFields(page, "1", "4", "2020");

            // Change the values and POST them
            var day = "3";
            var month = "4";
            var year = "2022";

            await page.FillAsync("[name='Date.Day']", day);
            await page.FillAsync("[name='Date.Month']", month);
            await page.FillAsync("[name='Date.Year']", year);

            await page.RunAndWaitForNavigationAsync(() => page.Keyboard.PressAsync("Enter"));

            // Verify POSTed values have been round-tripped
            await AssertFields(page, day, month, year);
        }

        private async Task AssertFields(
            IPage page,
            string expectedDay,
            string expectedMonth,
            string expectedYear,
            string expectedErrorMessage = null,
            bool? expectDayToHaveError = null,
            bool? expectMonthToHaveError = null,
            bool? expectYearToHaveError = null)
        {
            var inputs = await page.QuerySelectorAllAsync("input[type='text']");

            await AssertEx.CollectionAsync(
                inputs,
                input => AssertInput(input, expectedDay, expectDayToHaveError),
                input => AssertInput(input, expectedMonth, expectMonthToHaveError),
                input => AssertInput(input, expectedYear, expectYearToHaveError));

            if (expectedErrorMessage != null)
            {
                var error = (await page.TextContentAsync(".govuk-error-message")).TrimStart("Error:".ToCharArray());
                Assert.Equal(expectedErrorMessage, error);
            }

            static async Task AssertInput(IElementHandle input, string expectedValue, bool? expectError)
            {
                var value = await input.GetAttributeAsync("value");
                Assert.Equal(expectedValue, value);

                var classes = await input.GetClassListAsync();

                if (expectError == true)
                {
                    Assert.Contains("govuk-input--error", classes);
                }
                else if (expectError == false)
                {
                    Assert.DoesNotContain("govuk-input--error", classes);
                }
            }
        }
    }

    public class DateInputTestsFixture : ServerFixture
    {
        protected override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddGovUkFrontend(options => options.DateInputModelConverters.Add(new CustomDateTypeConverter()));

            services
                .AddMvc()
                .AddRazorOptions(options =>
                {
                    options.ViewLocationFormats.Clear();
                    options.ViewLocationFormats.Add("/DateInputTestsViews/{0}.cshtml");
                });
        }
    }

    [Route("DateInputTests")]
    public class DateInputsTestController : Controller
    {
        [HttpGet("AspForDate")]
        public IActionResult GetAspForDate() => View("AspForDate", new DateInputsTestsModel() { Date = new(2020, 4, 1) });

        [HttpPost("AspForDate")]
        public IActionResult PostAspForDate(DateInputsTestsModel model) => View("AspForDate", model);

        [HttpGet("ValueDate")]
        public IActionResult GetValueDate() => View("ValueDate", new DateInputsTestsModel() { Date = new(2020, 4, 1) });

        [HttpPost("ValueDate")]
        public IActionResult PostValueDate(DateInputsTestsModel model) => View("ValueDate", model);

        [HttpGet("AspForCustomDate")]
        public IActionResult GetAspForCustomDate() => View(
            "AspForCustomDate",
            new DateInputsTestsModel() { CustomDate = new CustomDateType(2020, 4, 1) });

        [HttpPost("AspForCustomDate")]
        public IActionResult PostAspForCustomDate(DateInputsTestsModel model) => View("AspForCustomDate", model);

        [HttpGet("ValueCustomDate")]
        public IActionResult GetValueCustomDate() => View(
            "ValueCustomDate",
            new DateInputsTestsModel() { CustomDate = new CustomDateType(2020, 4, 1) });

        [HttpPost("ValueCustomDate")]
        public IActionResult PostValueCustomDate(DateInputsTestsModel model) => View("ValueCustomDate", model);

        [HttpGet("ItemValues")]
        public IActionResult GetItemValues() => View("ItemValues", new DateInputsTestsModel() { Date = new(2020, 4, 1) });

        [HttpPost("ItemValues")]
        public IActionResult PostItemValues(DateInputsTestsModel model) => View("ItemValues", model);
    }

    public class DateInputsTestsModel
    {
        [Display(Name = "Date of birth")]
        [Required(ErrorMessage = "Enter your date of birth")]
        public Date? Date { get; set; }

        [Display(Name = "Date of birth")]
        [Required(ErrorMessage = "Enter your date of birth")]
        public CustomDateType CustomDate { get; set; }
    }

    public class CustomDateType
    {
        public CustomDateType(int year, int month, int day)
        {
            Y = year;
            M = month;
            D = day;
        }

        public int D { get; }
        public int M { get; }
        public int Y { get; }
    }

    public class CustomDateTypeConverter : DateInputModelConverter
    {
        public override bool CanConvertModelType(Type modelType) => modelType == typeof(CustomDateType);

        public override object CreateModelFromDate(Type modelType, Date date) => new CustomDateType(date.Year, date.Month, date.Day);

        public override Date? GetDateFromModel(Type modelType, object model)
        {
            if (model is null)
            {
                return null;
            }

            var cdt = (CustomDateType)model;
            return new Date(cdt.Y, cdt.M, cdt.D);
        }
    }
}
