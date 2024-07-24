using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.Models;

public class CourseDetailsModel
{
    public Course? ThisCourse { set; get; }

    public IList<CourseTopic> CourseTopics { set; get; } = new List<CourseTopic>();

    public IList<CourseTopic> CourseAdditionalTopics { set; get; } = new List<CourseTopic>();

    public string Type
    {
        get
        {
            switch (ThisCourse?.Perm)
            {
                case CourseType.FreeForAll:
                    return "عمومی";
                case CourseType.FreeForWriters:
                    return
                        $"نویسندگان سایت با حداقل {ThisCourse.NumberOfPostsRequired.ToPersianNumbers()} مطلب ارسالی {(ThisCourse.NumberOfMonthsRequired == 0
                            ? ""
                            : $"در {ThisCourse.NumberOfMonthsRequired.ToPersianNumbers()} ماه قبل")}";
                case CourseType.FreeForActiveUsers:
                    return
                        $"کاربران فعال با حداقل {ThisCourse.NumberOfTotalRatingsRequired.ToPersianNumbers()} امتیاز دریافتی {(ThisCourse.NumberOfMonthsTotalRatingsRequired == 0
                            ? ""
                            : $"در {ThisCourse.NumberOfMonthsTotalRatingsRequired.ToPersianNumbers()} ماه قبل")}";
                case CourseType.IsNotFree:
                    return "محدود";
            }

            return "";
        }
    }

    public string Comments => (ThisCourse?.NumberOfComments + ThisCourse?.NumberOfQuestionsComments).ToPersianNumbers();
}
