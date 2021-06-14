using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NodaTime;

namespace Samples.DateInput.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        [Display(Name = "Date of birth")]
        [Required(ErrorMessage = "Enter your date of birth")]
        public LocalDate? DateOfBirth { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
        }
    }
}
