namespace DntSite.Web.Features.UserFiles.Models;

public class NormalFileDataModel : AdditionalFormDataModelData
{
    [AllowUploadSafeFiles(ErrorMessage = "پسوند فایل ارسالی، قابل قبول نیست")]
    public IFormFile? File { set; get; }
}
