using System.Net.Mime;
using DntSite.Web.Features.Exports.Services.Contracts;

namespace DntSite.Web.Features.Exports.Controllers;

[ApiController]
[AllowAnonymous]
[Microsoft.AspNetCore.Mvc.Route(template: "[controller]")]
public class ExportsController(IPdfExportService pdfExportService) : ControllerBase
{
    public IActionResult Index() => NotFound();

    [Microsoft.AspNetCore.Mvc.Route(template: "{type}/{name}.pdf")]
    public IActionResult Get(string? type = null, string? name = null)
    {
        var filePath = pdfExportService.GetPhysicalFilePath(type, name);

        return filePath.FileExists()
            ? PhysicalFile(filePath, MediaTypeNames.Application.Pdf, Path.GetFileName(filePath))
            : NotFound();
    }
}
