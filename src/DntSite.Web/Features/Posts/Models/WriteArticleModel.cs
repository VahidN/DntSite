namespace DntSite.Web.Features.Posts.Models;

public class WriteArticleModel
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
    public IList<string> ArticleTags { set; get; } = null!;

    [Display(Name = "حداقل امتیاز برای مشاهده مطلب (0 = عمومی)")]
    public int NumberOfRequiredPoints { set; get; }
}
