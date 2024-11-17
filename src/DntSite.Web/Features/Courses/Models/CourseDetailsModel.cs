using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.Models;

public class CourseDetailsModel
{
    public Course? ThisCourse { set; get; }

    public IList<CourseTopic> CourseTopics { set; get; } = [];

    public IList<CourseTopic> CourseAdditionalTopics { set; get; } = [];

    public string Type => ThisCourse?.Perm switch
    {
        CourseType.FreeForAll => "عمومی",
        CourseType.FreeForWriters =>
            $"نویسندگان سایت با حداقل {ThisCourse.NumberOfPostsRequired.ToPersianNumbers()} مطلب ارسالی {(ThisCourse.NumberOfMonthsRequired == 0
                ? ""
                : $"در {ThisCourse.NumberOfMonthsRequired.ToPersianNumbers()} ماه قبل")}",
        CourseType.FreeForActiveUsers =>
            $"کاربران فعال با حداقل {ThisCourse.NumberOfTotalRatingsRequired.ToPersianNumbers()} امتیاز دریافتی {(ThisCourse.NumberOfMonthsTotalRatingsRequired == 0
                ? ""
                : $"در {ThisCourse.NumberOfMonthsTotalRatingsRequired.ToPersianNumbers()} ماه قبل")}",
        CourseType.IsNotFree => "محدود",
        _ => ""
    };

    public string Comments => (ThisCourse?.NumberOfComments + ThisCourse?.NumberOfQuestionsComments).ToPersianNumbers();
}
