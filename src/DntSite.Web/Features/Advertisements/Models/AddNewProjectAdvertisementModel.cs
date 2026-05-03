namespace DntSite.Web.Features.Advertisements.Models;

public class AddNewProjectAdvertisementModel
{
    [Display(Name = "نام شما جهت تماس")]
    [Required(ErrorMessage = "(*)")]
    public string Name { set; get; } = null!;

    [Display(Name = "شماره تماس")]
    [Required(ErrorMessage = "(*)")]
    public string Tel { set; get; } = null!;

    [Display(Name = "توضیحات عمومی پروژه")]
    [Required(ErrorMessage = "(*)")]
    [RequiredHtmlContent(ErrorMessage = "لطفا حداقل یک سطر توضیح را وارد نمائید.")]
    public string GeneralConditions { set; get; } = null!;

    [Display(Name = "فناوری‌های مدنظر جهت انجام پروژه")]
    [Required(ErrorMessage = "(*)")]
    [RequiredHtmlContent(ErrorMessage = "لطفا حداقل یک سطر توضیح را وارد نمائید.")]
    public string SpecialConditions { set; get; } = null!;

    [Display(Name = "ارسال رزومه به (آدرس ایمیل)")]
    [Required(ErrorMessage = "(*)")]
    [StringLength(maximumLength: 450, ErrorMessage = "حداکثر طول ایمیل 450 حرف است.")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(pattern: @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
        ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید")]
    public string SendResumeTo { set; get; } = null!;

    [Display(Name = "تاریخ انقضای آگهی")]
    [Required(ErrorMessage = "(*)")]
    public DateTime DueDate { set; get; }

    [Range(minimum: 0, maximum: 23, ErrorMessage = "ساعت وارد شده باید در بازه 0 تا 23 تعیین شود")]
    public int? Hour { set; get; }

    [Range(minimum: 1, maximum: 59, ErrorMessage = "دقیقه وارد شده باید در بازه 1 تا 59 تعیین شود")]
    public int? Minute { set; get; }

    [Display(Name = "گروه(ها)")]
    [Required(ErrorMessage = "لطفا تگ یا گروهی را وارد کنید")]
    [MinLength(length: 1, ErrorMessage = "لطفا حداقل یک گروه را وارد کنید")]
    public IList<string> Tags { set; get; } = [];
}
