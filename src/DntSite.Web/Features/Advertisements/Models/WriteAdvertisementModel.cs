namespace DntSite.Web.Features.Advertisements.Models;

public class WriteAdvertisementModel
{
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "(*)")]
    [StringLength(450, MinimumLength = 1, ErrorMessage = "حداکثر طول عنوان پیام 450 حرف و حداقل آن 1 حرف می‌باشد")]
    public string Title { set; get; } = default!;

    [Display(Name = "متن آگهی")]
    [Required(ErrorMessage = "(*)")]
    [MaxLength]
    [MinLength(100, ErrorMessage = "متن یک مطلب جدید حداقل 100 کاراکتر باید باشد")]
    [DataType(DataType.MultilineText)]
    public string Body { set; get; } = default!;

    [Display(Name = "گروه(ها)")]
    [Required(ErrorMessage = "لطفا تگ یا گروهی را وارد کنید")]
    [MinLength(1, ErrorMessage = "لطفا حداقل یک گروه را وارد کنید")]
    public IList<string> Tags { set; get; } = new List<string>();

    [Display(Name = "تاریخ انقضای آگهی")]
    [Required(ErrorMessage = "(*)")]
    public DateTime DueDate { set; get; } = default!;

    [Range(0, 23, ErrorMessage = "ساعت وارد شده باید در بازه 0 تا 23 تعیین شود")]
    public int? Hour { set; get; }

    [Range(1, 59, ErrorMessage = "دقیقه وارد شده باید در بازه 1 تا 59 تعیین شود")]
    public int? Minute { set; get; }
}
