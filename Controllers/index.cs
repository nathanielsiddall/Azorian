using Microsoft.AspNetCore.Mvc;

namespace Azorian.Controllers;


[Route("/")]
public class IndexController : Controller
{
    [HttpGet]
    public string Get() => "Hello World";
}