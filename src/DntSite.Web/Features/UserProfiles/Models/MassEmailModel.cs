namespace DntSite.Web.Features.UserProfiles.Models;

public class MassEmailModel
{
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "متن عنوان خالی است")]
    [StringLength(450, MinimumLength = 1, ErrorMessage = "حداکثر طول عنوان پیام 450 حرف و حداقل آن 1 حرف می‌باشد")]
    public string NewsTitle { set; get; } = default!;

    [Display(Name = "متن پیام")]
    [Required(ErrorMessage = "متن خبر خالی است")]
    [MaxLength]
    public string NewsBody { set; get; } = default!;

    [Required(ErrorMessage = "لطفا گروهی را انتخاب کنید")]
    [Display(Name = "ارسال به")]
    public IList<MassEmailGroup>? Groups { set; get; }

    [Display(Name = "حداقل تعداد مطلب نویسنده‌های دریافت کننده‌ی ایمیل")]
    [Required(ErrorMessage = "*")]
    public int MinPostsCount { set; get; } = 1;
}
