namespace DntSite.Web.Features.UserProfiles.Models;

public class UserProfileModel
{
    [Display(Name = "نام مستعار")]
    [Required(ErrorMessage = "نام مستعار خالی است.")]
    [StringLength(maximumLength: 450, MinimumLength = 3, ErrorMessage = "نام مستعار باید بین سه تا 450 کاراکتر باشد.")]
    [RegularExpression(pattern: @"^[\u0600-\u06FF,\u0590-\u05FF,0-9\s]*$",
        ErrorMessage = "لطفا تنها از اعداد و حروف فارسی استفاده نمائید")]
    public string FriendlyName { get; set; } = default!;

    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = "لطفا نام کاربری خود را وارد نمائید")]
    [StringLength(maximumLength: 450, MinimumLength = 3, ErrorMessage = "نام کاربری باید حداقل 3 کاراکتر باشد")]
    [RegularExpression(pattern: "^[a-zA-Z0-9_]*$", ErrorMessage = "لطفا تنها از اعداد و حروف انگلیسی استفاده نمائید")]
    public string UserName { set; get; } = default!;

    [Display(Name = "آدرس ایمیل")]
    [Required(ErrorMessage = "ایمیل خالی است")]
    [RegularExpression(pattern: @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
        ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید")]
    [StringLength(maximumLength: 450, ErrorMessage = "حداکثر طول ایمیل 450 حرف است.")]
    [DataType(DataType.EmailAddress)]
    public string EMail { set; get; } = default!;

    [Display(Name = "دریافت ایمیل")] public bool ReceiveDailyEmails { set; get; }

    [Display(Name = "آدرس صفحه خانگی")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس هاست 1000 حرف است.")]
    [DataType(DataType.Url)]
    public string? HomePageUrl { set; get; }

    public string? Photo { set; get; }

    [UploadFileExtensions(FileExtensions = ".jpg,.gif,.png,.jpeg", ErrorMessage = "لطفا فقط فایل تصویری ارسال کنید.")]
    [AllowUploadOnlyImageFiles(MaxHeight = 150, MaxWidth = 150,
        ErrorMessage = "لطفا یک فایل تصویری معتبر 150 در 150 پیکسل را ارسال کنید")]
    [Display(Name = "تصویر جدید")]
    public IFormFileCollection? PhotoFiles { set; get; }

    [Display(Name = "درباره من")]
    [StringLength(maximumLength: 2000, ErrorMessage = "حداکثر طول توضیحات شخصی 2000 حرف است.")]
    [DataType(DataType.MultilineText)]
    public string? Description { set; get; }

    [Display(Name = "محل اقامت")] public string? Location { set; get; }

    [Display(Name = "نمایش عمومی ایمیل به صورت تصویری")]
    public bool IsEmailPublic { set; get; }

    [Display(Name = "جویای کار")] public bool IsJobsSeeker { set; get; }

    public int? DateOfBirthYear { set; get; }

    public int? DateOfBirthMonth { set; get; }

    public int? DateOfBirthDay { set; get; }

    public bool IsRestricted { set; get; }
}
