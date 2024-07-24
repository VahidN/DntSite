namespace DntSite.Web.Features.Posts.Models;

public class WriteDraftModel
{
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "متن عنوان خالی است")]
    [StringLength(450, MinimumLength = 1, ErrorMessage = "حداکثر طول عنوان پیام 450 حرف و حداقل آن 1 حرف می‌باشد")]
    public string Title { set; get; } = null!;

    [Display(Name = "مطلب")]
    [Required(ErrorMessage = "متن اصلی خالی است")]
    [MaxLength]
    [MinLength(100, ErrorMessage = "متن یک مطلب جدید حداقل 100 کاراکتر باید باشد")]
    [DataType(DataType.MultilineText)]
    [ShouldContainPersianLetters(ErrorMessage =
        "به نظر می‌رسد بهتر باشد این مطلب تمام انگلیسی، به صورت یک لینک جدید در «قسمت اشتراک‌ها» مطرح شود")]
    public string ArticleBody { set; get; } = null!;

    [Display(Name = "گروه(ها)")]
    [Required(ErrorMessage = "لطفا تگ یا گروهی را وارد کنید")]
    [MinLength(1, ErrorMessage = "لطفا حداقل یک گروه را وارد کنید")]
    public IList<string> Tags { set; get; } = null!;

    [Display(Name = "مطلب تکمیل شده‌ و آماده‌ی انتشار عمومی است")]
    public bool IsReady { set; get; }

    [Range(0, 23, ErrorMessage = "ساعت وارد شده باید در بازه 0 تا 23 تعیین شود")]
    public int? Hour { set; get; }

    [Range(1, 59, ErrorMessage = "دقیقه وارد شده باید در بازه 1 تا 59 تعیین شود")]
    public int? Minute { set; get; }

    public int? PersianDateYear { set; get; }

    public int? PersianDateMonth { set; get; }

    public int? PersianDateDay { set; get; }

    [Display(Name = "حداقل امتیاز برای مشاهده مطلب (0 = عمومی)")]
    public int NumberOfRequiredPoints { set; get; }

    public int ReadingTimeMinutes { set; get; }
}
