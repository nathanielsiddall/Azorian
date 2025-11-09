using Microsoft.AspNetCore.Mvc;

namespace Azorian.Controllers;

[Route("/")]
public class IndexController : Controller
{
    public string Get() => "Hello World";
}