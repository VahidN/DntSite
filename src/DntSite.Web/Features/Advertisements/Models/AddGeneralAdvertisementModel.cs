namespace DntSite.Web.Features.Advertisements.Models;

public class AddGeneralAdvertisementModel
{
    [Display(Name = "نام کامل شرکت")]
    [Required(ErrorMessage = "(*)")]
    public string OrganizationName { set; get; } = default!;

    [Display(Name = "آدرس کامل شرکت")]
    [Required(ErrorMessage = "(*)")]
    public string Address { set; get; } = default!;

    [Display(Name = "امتیازات حضور در مجموعه ما")]
    [Required(ErrorMessage = "(*)")]
    [RequiredHtmlContent(ErrorMessage = "لطفا حداقل یک سطر توضیح را وارد نمائید.")]
    public string Benefits { set; get; } = default!;

    [Display(Name = "عنوان شغلی مورد نیاز")]
    [Required(ErrorMessage = "(*)")]
    public string JobTitle { set; get; } = "برنامه نویس";

    [Display(Name = "آدرس وب سایت شرکت")]
    [Required(ErrorMessage = "(*)")]
    [StringLength(maximumLength: 1500, MinimumLength = 1,
        ErrorMessage = "حداکثر طول 1500 حرف و حداقل آن 1 حرف می‌باشد")]
    public string WebSiteUrl { set; get; } = "https://";

    [Display(Name = "شماره تماس")]
    [Required(ErrorMessage = "(*)")]
    public string Tel { set; get; } = default!;

    [Display(Name = "نام شما جهت تماس")]
    [Required(ErrorMessage = "(*)")]
    public string Name { set; get; } = default!;

    [Display(Name = "شرایط عمومی متقاضی")]
    [Required(ErrorMessage = "(*)")]
    [RequiredHtmlContent(ErrorMessage = "لطفا حداقل یک سطر توضیح را وارد نمائید.")]
    public string GeneralConditions { set; get; } = default!;

    [Display(Name = "شرایط تخصصی متقاضی")]
    [Required(ErrorMessage = "(*)")]
    [RequiredHtmlContent(ErrorMessage = "لطفا حداقل یک سطر توضیح را وارد نمائید.")]
    public string SpecialConditions { set; get; } = default!;

    [Display(Name = "اولویت‌ها")]
    [Required(ErrorMessage = "(*)")]
    [RequiredHtmlContent(ErrorMessage = "لطفا حداقل یک سطر توضیح را وارد نمائید.")]
    public string SpecialPoints { set; get; } = default!;

    [Required(ErrorMessage = "لطفا جنسیتی را انتخاب کنید")]
    [Display(Name = "جنسیت متقاضی")]
    public IList<Gender>? Genders { set; get; }

    [Required(ErrorMessage = "لطفا نوع همکاری را انتخاب کنید")]
    [Display(Name = "نوع همکاری مورد نیاز")]
    public IList<JobType>? JobTypes { set; get; }

    [Display(Name = "حداکثر سن متقاضی")]
    [Required(ErrorMessage = "(*)")]
    public int MaxAge { set; get; } = 50;

    [Display(Name = "ارسال رزومه به (آدرس ایمیل)")]
    [Required(ErrorMessage = "(*)")]
    [StringLength(maximumLength: 450, ErrorMessage = "حداکثر طول ایمیل 450 حرف است.")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(pattern: @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
        ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید")]
    public string SendResumeTo { set; get; } = default!;

    [Display(Name = "تاریخ انقضای آگهی")]
    [Required(ErrorMessage = "(*)")]
    public DateTime DueDate { set; get; } = default!;

    [Range(minimum: 0, maximum: 23, ErrorMessage = "ساعت وارد شده باید در بازه 0 تا 23 تعیین شود")]
    public int? Hour { set; get; }

    [Range(minimum: 1, maximum: 59, ErrorMessage = "دقیقه وارد شده باید در بازه 1 تا 59 تعیین شود")]
    public int? Minute { set; get; }

    [Display(Name = "گروه(ها)")]
    [Required(ErrorMessage = "لطفا تگ یا گروهی را وارد کنید")]
    [MinLength(length: 1, ErrorMessage = "لطفا حداقل یک گروه را وارد کنید")]
    public IList<string> Tags { set; get; } = [];
}
