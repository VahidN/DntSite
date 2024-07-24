namespace DntSite.Web.Features.Advertisements.Models;

public class AddNewProjectAdvertisementModel
{
    [Display(Name = "نام شما جهت تماس")]
    [Required(ErrorMessage = "(*)")]
    public string Name { set; get; } = default!;

    [Display(Name = "شماره تماس")]
    [Required(ErrorMessage = "(*)")]
    public string Tel { set; get; } = default!;

    [Display(Name = "توضیحات عمومی پروژه")]
    [Required(ErrorMessage = "(*)")]
    public string GeneralConditions { set; get; } = default!;

    [Display(Name = "فناوری‌های مدنظر جهت انجام پروژه")]
    [Required(ErrorMessage = "(*)")]
    public string SpecialConditions { set; get; } = default!;

    [Display(Name = "ارسال رزومه به (آدرس ایمیل)")]
    [Required(ErrorMessage = "(*)")]
    [StringLength(450, ErrorMessage = "حداکثر طول ایمیل 450 حرف است.")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
        ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید")]
    public string SendResumeTo { set; get; } = default!;

    [Display(Name = "تاریخ انقضای آگهی")]
    [Required(ErrorMessage = "(*)")]
    public DateTime DueDate { set; get; } = default!;

    [Range(0, 23, ErrorMessage = "ساعت وارد شده باید در بازه 0 تا 23 تعیین شود")]
    public int? Hour { set; get; }

    [Range(1, 59, ErrorMessage = "دقیقه وارد شده باید در بازه 1 تا 59 تعیین شود")]
    public int? Minute { set; get; }

    [Display(Name = "گروه(ها)")]
    [Required(ErrorMessage = "لطفا تگ یا گروهی را وارد کنید")]
    [MinLength(1, ErrorMessage = "لطفا حداقل یک گروه را وارد کنید")]
    public IList<string> Tags { set; get; } = new List<string>();
}
