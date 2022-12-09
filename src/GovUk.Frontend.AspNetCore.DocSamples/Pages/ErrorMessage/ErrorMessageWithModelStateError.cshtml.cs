using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GovUk.Frontend.AspNetCore.DocSamples.Pages.ErrorMessage
{
    public class ErrorMessageWithModelStateErrorModel : PageModel
    {
        public string? FullName { get; set; }

        public void OnGet()
        {
            ModelState.AddModelError(nameof(FullName), "Enter your full name");
        }
    }
}
