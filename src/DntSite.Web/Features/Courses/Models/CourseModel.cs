using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.Models;

public class CourseModel
{
    [Display(Name = "عنوان دوره")]
    [Required(ErrorMessage = "متن عنوان خالی است")]
    [StringLength(450, MinimumLength = 1, ErrorMessage = "حداکثر طول عنوان پیام 450 حرف و حداقل آن 1 حرف می‌باشد")]
    public string Title { set; get; } = default!;

    [Display(Name = "چکیده مباحثی که مطرح خواهد شد")]
    [Required(ErrorMessage = "توضیحات خالی است")]
    [MaxLength]
    public string Description { set; get; } = default!;

    [Display(Name = "سطح دوره")]
    [Required(ErrorMessage = "سطح دوره خالی است")]
    [MaxLength]
    public string TopicsList { set; get; } = default!;

    [Display(Name = "پیشنیازها")]
    [Required(ErrorMessage = "پیشنیازها خالی است")]
    [MaxLength]
    public string Requirements { set; get; } = default!;

    [Display(Name = "هزینه و نحوه پرداخت")]
    [MaxLength]
    public string HowToPay { set; get; } = "-";

    public CourseType Perm { set; get; }

    public int NumberOfPostsRequired { set; get; }

    public int NumberOfMonthsRequired { set; get; }

    public int NumberOfTotalRatingsRequired { set; get; }

    public int NumberOfMonthsTotalRatingsRequired { set; get; }

    [Display(Name = "آیا دوره فعال شود؟")] public bool IsReadyToPublish { set; get; }

    [Display(Name = "گروه(ها)")]
    [Required(ErrorMessage = "لطفا حداقل یک گروه را وارد نمائید.")]
    [MinLength(1, ErrorMessage = "لطفا حداقل یک گروه را وارد کنید")]
    public IList<string> Tags { set; get; } = new List<string>();
}
