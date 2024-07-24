namespace DntSite.Web.Features.Stats.Models;

public enum AnnualStatisticsItem
{
    [Display(Name = "مقالات")] Articles,

    [Display(Name = "دوره‌ها")] Courses,

    [Display(Name = "پروژه‌ها")] Projects,

    [Display(Name = "اشتراک‌ها")] Links,

    [Display(Name = "نظرسنجی‌ها")] Surveys,

    [Display(Name = "کاربران")] RegisteredUsers
}
