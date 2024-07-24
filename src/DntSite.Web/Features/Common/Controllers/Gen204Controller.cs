namespace DntSite.Web.Features.Common.Controllers;

/// <summary>
///     Just to stop `/gen204?invalidResponse` errors!
/// </summary>
[ApiController]
[AllowAnonymous]
[Microsoft.AspNetCore.Mvc.Route(template: "[controller]")]
public class Gen204Controller : ControllerBase
{
    public IActionResult Get() => Content(content: "ok");
}
