namespace DntSite.Web.Features.Surveys.Models;

public class VoteModel
{
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "متن عنوان خالی است")]
    [StringLength(450, MinimumLength = 1, ErrorMessage = "حداکثر طول عنوان پیام 450 حرف و حداقل آن 1 حرف می‌باشد")]
    public string Title { set; get; } = default!;

    [Display(Name = "گزینه‌ها")]
    [Required(ErrorMessage = "گزینه‌ها خالی است")]
    [DataType(DataType.MultilineText)]
    public string VoteItems { set; get; } = default!;

    [Display(Name = "گروه(ها)")]
    [Required(ErrorMessage = "لطفا حداقل یک گروه را وارد نمائید.")]
    [MinLength(1, ErrorMessage = "لطفا حداقل یک گروه را وارد کنید")]
    public IList<string> Tags { set; get; } = [];

    [Range(0, 23, ErrorMessage = "ساعت وارد شده باید در بازه 0 تا 23 تعیین شود")]
    public int? Hour { set; get; }

    [Range(1, 59, ErrorMessage = "دقیقه وارد شده باید در بازه 1 تا 59 تعیین شود")]
    public int? Minute { set; get; }

    [Display(Name = "تاریخ پایان اعتبار")] public DateTime? ExpirationDate { set; get; }

    [Display(Name = "امکان انتخاب چندین گزینه با هم وجود داشته باشد")]
    public bool AllowMultipleSelection { set; get; }

    [DataType(DataType.MultilineText)]
    [Display(Name = "توضیحات تکمیلی")]
    public string? Description { set; get; }
}
