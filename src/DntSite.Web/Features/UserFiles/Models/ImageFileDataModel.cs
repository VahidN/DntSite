namespace DntSite.Web.Features.UserFiles.Models;

public class ImageFileDataModel : AdditionalFormDataModelData
{
    [UploadFileExtensions(FileExtensions = ".jpg,.gif,.png,.jpeg",
        ErrorMessage = "لطفا فقط یک فایل تصویری را ارسال کنید")]
    [AllowUploadSafeFiles(ErrorMessage = "پسوند فایل ارسالی، قابل قبول نیست")]
    [AllowUploadOnlyImageFiles(ErrorMessage = "لطفا یک فایل تصویری معتبر را ارسال کنید")]
    public IFormFile? File { set; get; }
}
