using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.WebToolkit;
using DntSite.Web.Features.UserProfiles.Services.Contracts;
using Microsoft.AspNetCore.OutputCaching;
using IActionResult = Microsoft.AspNetCore.Mvc.IActionResult;

namespace DntSite.Web.Features.UserFiles.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route(template: "[controller]")]
public class FileController(
    IFileNameSanitizerService fileNameSanitizerService,
    IAppFoldersService appFoldersService,
    IUsersInfoService usersService) : ControllerBase
{
    public IActionResult Index() => NotFound();

    [OutputCache(Duration = OutputCacheDuration.AMonth, VaryByQueryKeys = ["name"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "[action]")]
    public IActionResult Avatar([FromQuery] string name) => FlushFile(FileType.Avatar, name, name.MimeType());

    [OutputCache(Duration = OutputCacheDuration.AMonth, VaryByQueryKeys = ["name"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "[action]")]
    public IActionResult Image([FromQuery] string name) => FlushFile(FileType.Image, name, name.MimeType());

    //تمام قسمت‌های خصوصی سایت نباید کش داشته باشند
    [Microsoft.AspNetCore.Mvc.Route(template: "[action]")]
    public IActionResult MessagesImages(string name) => FlushFile(FileType.MessagesImages, name, name.MimeType());

    [OutputCache(Duration = OutputCacheDuration.AMonth, VaryByQueryKeys = ["name"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "[action]")]
    public IActionResult UserFile([FromQuery] string name) => FlushFile(FileType.UserFile, name);

    [OutputCache(Duration = OutputCacheDuration.AMonth, VaryByQueryKeys = ["name"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "[action]")]
    public IActionResult ProjectFile([FromQuery] string name) => FlushFile(FileType.ProjectFiles, name);

    //تمام قسمت‌های خصوصی سایت نباید کش داشته باشند
    [Microsoft.AspNetCore.Mvc.Route(template: "[action]")]
    public IActionResult Messages([FromQuery] string name) => FlushFile(FileType.Messages, name);

    [OutputCache(Duration = OutputCacheDuration.AMonth, VaryByQueryKeys = ["name"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "[action]")]
    public IActionResult NewsThumb([FromQuery] string name) => FlushFile(FileType.NewsThumb, name);

    //تمام قسمت‌های خصوصی سایت نباید کش داشته باشند
    [Authorize]
    [Microsoft.AspNetCore.Mvc.Route(template: "[action]")]
    public IActionResult CommonFiles([FromQuery] string name) => FlushFile(FileType.CommonFiles, name);

    [OutputCache(Duration = OutputCacheDuration.AMonth, VaryByQueryKeys = ["name"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "[action]")]
    public IActionResult CourseFiles([FromQuery] string name) => FlushFile(FileType.CourseFile, name);

    [OutputCache(Duration = OutputCacheDuration.AMonth, VaryByQueryKeys = ["name"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "[action]")]
    public IActionResult CourseImages([FromQuery] string name)
        => FlushFile(FileType.CourseImage, name, name.MimeType());

    [OutputCache(Duration = OutputCacheDuration.ADay, VaryByQueryKeys = ["id"])]
    [Microsoft.AspNetCore.Mvc.Route(template: "[action]")]
    public async Task<IActionResult> EmailToImage([FromQuery] int? id)
    {
        var emailBytes = await usersService.GetEmailImageAsync(id);

        return File(emailBytes, ImageMimeType.Png);
    }

    private IActionResult FlushFile(FileType fileType, string name, string contentType = "application/octet-stream")
    {
        var safeFile = fileNameSanitizerService.IsSafeToDownload(appFoldersService.GetFolderPath(fileType), name);

        if (!safeFile.IsSafeToDownload)
        {
            return BadRequest();
        }

        var path = safeFile.SafeFilePath;

        return PhysicalFile(path, contentType, Path.GetFileName(path));
    }
}
