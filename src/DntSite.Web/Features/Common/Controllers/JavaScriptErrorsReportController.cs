namespace DntSite.Web.Features.Common.Controllers;

/// <summary>
///     Logs the global JavaScript errors
/// </summary>
[ApiController]
[AllowAnonymous]
[Microsoft.AspNetCore.Mvc.Route(template: "api/[controller]")]
public class JavaScriptErrorsReportController(
    ILogger<JavaScriptErrorsReportController> logger,
    IAntiXssService antiXssService) : ControllerBase
{
    [HttpPost(template: "[action]")]
    [IgnoreAntiforgeryToken]
    public IActionResult Log([FromBody] string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
        {
            return BadRequest();
        }

        logger.LogError(message: "{ErrorMessage}", antiXssService.GetSanitizedHtml(errorMessage));

        return Ok();
    }
}
