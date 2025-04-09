using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Controllers;

[ApiController]
[AllowAnonymous]
[Microsoft.AspNetCore.Mvc.Route(template: "[controller]")]
public class WelcomeController(ISpidersService spidersService) : ControllerBase
{
    private static readonly string[] Welcome = [$"Thanks for visiting today[{DateTime.UtcNow:s}]!"];

    [HttpGet(template: "[action]")]
    [IgnoreAntiforgeryToken]
    public IActionResult Log()
    {
        // A nosy scraper caught!

        spidersService.AddCaughtScraper(HttpContext.GetIP() ?? "::1", HttpContext.GetUserAgent());

        return Ok(Welcome);
    }
}
