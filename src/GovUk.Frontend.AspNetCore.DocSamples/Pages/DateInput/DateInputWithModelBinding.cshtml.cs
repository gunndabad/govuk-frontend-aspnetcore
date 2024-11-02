using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GovUk.Frontend.AspNetCore.DocSamples.Pages.DateInput;

[BindProperties]
public class DateInputWithModelBindingModel : PageModel
{
    [Display(Name = "When was your passport issued?")]
    [Required(ErrorMessage = "Enter your passport issue date")]
    [DateInput(ErrorMessagePrefix = "Your passport issue date")]
    public DateOnly? PassportIssueDate { get; set; }

    public void OnGet()
    {
    }
}
