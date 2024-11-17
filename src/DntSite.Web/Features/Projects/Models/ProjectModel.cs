namespace DntSite.Web.Features.Projects.Models;

public class ProjectModel
{
    [Display(Name = "عنوان پروژه")]
    [Required(ErrorMessage = "متن عنوان خالی است")]
    [StringLength(maximumLength: 450, MinimumLength = 1,
        ErrorMessage = "حداکثر طول عنوان پیام 450 حرف و حداقل آن 1 حرف می‌باشد")]
    public string Title { set; get; } = default!;

    [Display(Name = "آیکون یا لوگوی پروژه")]
    [UploadFileExtensions(fileExtensions: ".jpg,.gif,.png,.jpeg", ErrorMessage = "لطفا فقط فایل تصویری ارسال کنید.")]
    [AllowUploadOnlyImageFiles(maxWidth: 200, maxHeight: 200,
        ErrorMessage = "لطفا یک فایل تصویری معتبر 200 در 200 پیکسل را ارسال کنید")]
    public IFormFileCollection? PhotoFiles { set; get; }

    public string? Logo { set; get; }

    [Display(Name = "توضیحات")]
    [Required(ErrorMessage = "توضیحات خالی است")]
    [RequiredHtmlContent(ErrorMessage = "لطفا حداقل یک سطر توضیح را وارد نمائید.")]
    [MaxLength]
    [DataType(DataType.MultilineText)]
    public string DescriptionText { set; get; } = default!;

    [Display(Name = "آدرس مخزن کد پروژه")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس مخزن کد 1000 حرف می‌باشد")]
    public string SourcecodeRepositoryUrl { set; get; } = default!;

    [Display(Name = "نوع مجوز استفاده از پروژه")]
    [Required(ErrorMessage = "مجوز خالی است")]
    [RequiredHtmlContent(ErrorMessage = "لطفا حداقل یک سطر توضیح را وارد نمائید.")]
    [MaxLength]
    [DataType(DataType.MultilineText)]
    public string LicenseText { set; get; } = default!;

    [Display(Name = "لیست وابستگی‌های پروژه")]
    [MaxLength]
    [DataType(DataType.MultilineText)]
    public string? RequiredDependenciesText { set; get; }

    [Display(Name = "لیست مقالات و مطالب مرتبط")]
    [MaxLength]
    [DataType(DataType.MultilineText)]
    public string? RelatedArticlesText { set; get; }

    [Display(Name = "لیست همکاران")]
    [MaxLength]
    public string? DevelopersDescriptionText { set; get; }

    [Display(Name = "گروه(ها)")]
    [Required(ErrorMessage = "لطفا تگ یا گروهی را وارد کنید")]
    [MinLength(length: 1, ErrorMessage = "لطفا حداقل یک گروه را وارد کنید")]
    public IList<string> Tags { set; get; } = [];
}
