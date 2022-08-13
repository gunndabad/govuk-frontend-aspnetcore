using Microsoft.AspNetCore.Mvc;

namespace Samples.MvcStarter.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
