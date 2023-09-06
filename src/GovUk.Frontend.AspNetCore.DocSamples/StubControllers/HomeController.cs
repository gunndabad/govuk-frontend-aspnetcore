using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.AspNetCore.DocSamples.StubControllers;

public class HomeController : Controller
{
    [HttpGet("")]
    public IActionResult Index() => Ok();
}
