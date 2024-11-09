namespace DntSite.Web.Features.News.Models;

public class DailyNewsItemModel
{
    [Display(Name = "آدرس مطلب")]
    [Required(ErrorMessage = "لطفا آدرس خبر را مشخص کنید")]
    [StringLength(1000, ErrorMessage = "حداکثر طول آدرس پیام 1000 حرف می‌باشد")]
    [DataType(DataType.Url)]
    public string Url { set; get; } = default!;

    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "لطفا عنوان خبر را مشخص کنید")]
    [StringLength(450, ErrorMessage = "حداکثر طول عنوان پیام 450 حرف می‌باشد")]
    [ShouldContainPersianLetters(ErrorMessage = "عنوان ارسالی تا حد امکان باید فارسی باشد")]
    public string Title { set; get; } = default!;

    [Display(Name = "توضیح کوتاه حداکثر 3 سطری")]
    [MaxLength]
    [DataType(DataType.MultilineText)]
    [Required(ErrorMessage = "لطفا حداقل یک سطر توضیح را وارد نمائید.")]
    [RequiredHtmlContent(ErrorMessage = "لطفا حداقل یک سطر توضیح را وارد نمائید.")]
    [MinLength(15, ErrorMessage = "لطفا حداقل یک سطر توضیح را وارد نمائید.")]
    public string DescriptionText { set; get; } = default!;

    [Display(Name = "گروه(ها)")]
    [Required(ErrorMessage = "لطفا حداقل یک گروه را وارد نمائید.")]
    [MinLength(1, ErrorMessage = "لطفا حداقل یک گروه را وارد کنید")]
    public IList<string> Tags { set; get; } = new List<string>();
}
