using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.AspNetCore.IntegrationTests.ViewComponents;

public class InputWithErrorViewComponent : ViewComponent
{
    public IViewComponentResult Invoke() => View();
}
