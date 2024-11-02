using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GovUk.Frontend.AspNetCore.DocSamples.Pages.TextInput;

[BindProperties]
public class TextInputWithModelBindingModel : PageModel
{
    [Display(Name = "What is the name of the event?")]
    [Required(ErrorMessage = "Enter the name of the event")]
    public string? EventName { get; set; }

    public void OnGet()
    {
    }
}
