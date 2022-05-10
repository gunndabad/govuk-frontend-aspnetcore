using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GovUk.Frontend.AspNetCore.DocSamples.Pages.ErrorSummary
{
    public class ErrorSummaryWithModelStateErrorModel : PageModel
    {
        public string FullName { get; set; }

        public void OnGet()
        {
            ModelState.AddModelError(nameof(FullName), "Enter your full name");
        }
    }
}
