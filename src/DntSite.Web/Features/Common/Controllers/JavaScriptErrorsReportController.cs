using DntSite.Web.Features.AppConfigs.Services.Contracts;

namespace DntSite.Web.Features.Common.Controllers;

/// <summary>
///     Logs the global JavaScript errors
/// </summary>
[ApiController]
[AllowAnonymous]
[Microsoft.AspNetCore.Mvc.Route(template: "api/[controller]")]
public class JavaScriptErrorsReportController(
    ILogger<JavaScriptErrorsReportController> logger,
    IAppAntiXssService antiXssService,
    ICacheService cacheService) : ControllerBase
{
    [HttpPost(template: "[action]")]
    [HttpGet(template: "[action]")]
    [IgnoreAntiforgeryToken]
    public IActionResult Log([FromBody] string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
        {
            return BadRequest();
        }

        cacheService.GetOrAdd(errorMessage.Md5Hash(), nameof(JavaScriptErrorsReportController), () =>
        {
            logger.LogError(message: "JavaScript Error -> {Body}, {Request}",
                antiXssService.GetSanitizedHtml(errorMessage), HttpContext.Request.LogRequest(responseCode: 200));

            return errorMessage;
        }, DateTimeOffset.UtcNow.AddMinutes(minutes: 7));

        return Ok();
    }
}
