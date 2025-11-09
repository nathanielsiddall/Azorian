using Microsoft.AspNetCore.Mvc;

namespace Azorian.Controllers;

[Route("/")]
[ApiController]
public class IndexController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Get() => "Hello World";
}
