namespace DntSite.Web.Features.Courses.Models;

public class CourseTopicItemModel
{
    [Required(ErrorMessage = "لطفا عنوان را تکمیل کنید")]
    [Display(Name = "عنوان")]
    public string Title { set; get; } = default!;

    [Required(ErrorMessage = "لطفا متن را تکمیل کنید")]
    [Display(Name = "مقاله")]
    public string Body { set; get; } = default!;

    [Display(Name = "آیا مطلب اصلی است؟")] public bool IsMainTopic { set; get; }
}
