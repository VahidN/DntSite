namespace DntSite.Web.Features.AppConfigs.Entities;

[ComplexType]
public class MinimumRequiredPosts
{
    [Display(Name = "حداقل تعداد ماه حضور تا پیش از غیرفعال شدن")]
    public int MinMonthToStayActive { set; get; }

    [Display(Name = "حداکثر تعداد روز قابل ویرایش بودن یک مطلب")]
    public int MaxDaysToCloseATopic { set; get; }

    [Display(Name = "حداقل تعداد مطلب ضروری برای ایجاد یک مسیر راه")]
    public int MinPostsToCreateALearningPath { set; get; }

    [Display(Name = "حداقل تعداد لینک ضروری برای ایجاد یک مسیر راه")]
    public int MinNumberOfLinksToCreateALearningPath { set; get; }

    [Display(Name = "حداقل تعداد لینک ضروری برای ایجاد یک پیشنهاد")]
    public int MinNumberOfLinksToCreateANewBacklog { set; get; }

    [Display(Name = "حداقل تعداد لینک ضروری برای ایجاد یک نظرسنجی")]
    public int MinNumberOfLinksCreateANewSurvey { set; get; }

    [Display(Name = "حداقل تعداد مطلب ضروری برای ایجاد یک دوره")]
    public int MinPostsToCreateANewCourse { set; get; }

    [Display(Name = "حداقل تعداد مطلب ضروری برای ایجاد یک پروژه")]
    public int MinPostsToCreateANewProject { set; get; }
}
