using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.UserFiles.Models;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserFiles.Controllers;

[Authorize]
[ApiController]
[Microsoft.AspNetCore.Mvc.Route(template: "api/[controller]")]
public class UploadFileController(
    ILogger<UploadFileController> logger,
    IAppFoldersService appFoldersService,
    IUploadFileService uploadFileService,
    IAdminsEmailsService emailsService,
    IUrlHelper urlHelper) : ControllerBase
{
    [HttpPost(template: "[action]")]
    public Task<IActionResult> ImageUpload([FromForm] ImageFileDataModel? model)
        => UploadFileAsync(model?.File, FileType.Image);

    [HttpPost(template: "[action]")]
    public Task<IActionResult> MessagesImagesUpload([FromForm] ImageFileDataModel? model)
        => UploadFileAsync(model?.File, FileType.MessagesImages);

    [HttpPost(template: "[action]")]
    public Task<IActionResult> CourseImagesUpload([FromForm] ImageFileDataModel? model)
        => UploadFileAsync(model?.File, FileType.CourseImage);

    [HttpPost(template: "[action]")]
    public Task<IActionResult> CourseFileUpload([FromForm] NormalFileDataModel? model)
        => UploadFileAsync(model?.File, FileType.CourseFile);

    [HttpPost(template: "[action]")]
    public Task<IActionResult> FileUpload([FromForm] NormalFileDataModel? model)
        => UploadFileAsync(model?.File, FileType.UserFile);

    [HttpPost(template: "[action]")]
    public Task<IActionResult> CommonFilesUpload([FromForm] NormalFileDataModel? model)
        => UploadFileAsync(model?.File, FileType.CommonFiles);

    [HttpPost(template: "[action]")]
    public Task<IActionResult> MessagesFilesUpload([FromForm] NormalFileDataModel? model)
        => UploadFileAsync(model?.File, FileType.Messages);

    private async Task<IActionResult> UploadFileAsync(IFormFile? file, FileType fileType)
    {
        try
        {
            var savePath = appFoldersService.GetFolderPath(fileType);

            var (isSaved, savedFilePath) =
                await uploadFileService.SavePostedFileAsync(file, savePath, allowOverwrite: false);

            if (!isSaved)
            {
                return BadRequest(new FileUploadResultModel
                {
                    Error = "امکان ذخیره سازی فایل وجود ندارد"
                });
            }

            var fileName = Path.GetFileName(savedFilePath);
            var uploadedFileUrl = GetUploadedFileUrl(fileName, fileType);

            if (string.IsNullOrWhiteSpace(uploadedFileUrl))
            {
                return BadRequest(new FileUploadResultModel
                {
                    Error = "امکان ذخیره سازی فایل وجود ندارد"
                });
            }

            await emailsService.UploadFileSendEmailAsync(fileName, uploadedFileUrl);

            return Ok(new FileUploadResultModel
            {
                Url = uploadedFileUrl,
                FileName = file?.FileName ?? fileName
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: "UploadFileAsync({FileType}) -> {Name}", fileType, file?.FileName);

            return BadRequest(new FileUploadResultModel
            {
                Error = ex.Message
            });
        }
    }

    private string? GetUploadedFileUrl(string name, FileType fileType)
        => fileType switch
        {
            FileType.Avatar => urlHelper.Action(action: "Avatar", controller: "File", new
            {
                name
            }, Request.Scheme),
            FileType.Image => urlHelper.Action(action: "Image", controller: "File", new
            {
                name
            }, Request.Scheme),
            FileType.UserFile => urlHelper.Action(action: "UserFile", controller: "File", new
            {
                name
            }, Request.Scheme),
            FileType.ProjectFiles => urlHelper.Action(action: "ProjectFile", controller: "File", new
            {
                name
            }, Request.Scheme),
            FileType.CommonFiles => urlHelper.Action(action: "CommonFiles", controller: "File", new
            {
                name
            }, Request.Scheme),
            FileType.Messages => urlHelper.Action(action: "Messages", controller: "File", new
            {
                name
            }, Request.Scheme),
            FileType.MessagesImages => urlHelper.Action(action: "MessagesImages", controller: "File", new
            {
                name
            }, Request.Scheme),
            FileType.CourseFile => urlHelper.Action(action: "CourseFiles", controller: "File", new
            {
                name
            }, Request.Scheme),
            FileType.CourseImage => urlHelper.Action(action: "CourseImages", controller: "File", new
            {
                name
            }, Request.Scheme),
            _ => urlHelper.Action(action: "UserFile", controller: "File", new
            {
                name
            }, Request.Scheme)
        };
}
