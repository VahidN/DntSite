namespace DntSite.Web.Features.Projects.Models;

public class ProjectPostFileModel
{
    [Display(Name = "توضیحات")]
    [Required(ErrorMessage = "لطفا توضیحاتی را وارد کنید")]
    [RequiredHtmlContent(ErrorMessage = "لطفا حداقل یک سطر توضیح را وارد نمائید.")]
    public string Description { set; get; } = default!;

    public string? FileName { set; get; }

    [Display(Name = "فایل فشرده شده")]
    [UploadFileExtensions(FileExtensions = ".zip,.rar,.7z,.gz,.tar",
        ErrorMessage = "لطفا فقط یک فایل فشرده شده را ارسال کنید.")]
    public IFormFileCollection? ProjectFiles { set; get; }
}
