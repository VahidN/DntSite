namespace DntSite.Web.Features.RoadMaps.Models;

public class LearningPathModel
{
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "متن عنوان خالی است")]
    [StringLength(maximumLength: 450, MinimumLength = 1,
        ErrorMessage = "حداکثر طول عنوان پیام 450 حرف و حداقل آن 1 حرف می‌باشد")]
    public string Title { set; get; } = default!;

    [Display(Name = "مسیر راه (مجموعه‌ای از لینک‌های هم خانواده از سایت جاری)")]
    [Required(ErrorMessage = "متن مسیر راه خالی است")]
    [MaxLength]
    public string Description { set; get; } = default!;

    [Display(Name = "گروه(ها)")]
    [Required(ErrorMessage = "لطفا حداقل یک گروه را وارد نمائید.")]
    [MinLength(length: 1, ErrorMessage = "لطفا حداقل یک گروه را وارد کنید")]
    public IList<string> Tags { set; get; } = new List<string>();
}
