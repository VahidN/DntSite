namespace DntSite.Web.Features.Common.Controllers;

/// <summary>
///     Logs the global JavaScript errors
/// </summary>
[ApiController]
[AllowAnonymous]
[Microsoft.AspNetCore.Mvc.Route(template: "api/[controller]")]
public class JavaScriptErrorsReportController(ILogger<JavaScriptErrorsReportController> logger) : ControllerBase
{
    [HttpPost(template: "[action]")]
    [IgnoreAntiforgeryToken]
    public IActionResult Log([FromBody] string errorMessage)
    {
        logger.LogError(message: "{ErrorMessage}", errorMessage);

        return Ok();
    }
}
