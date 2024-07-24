using AutoMapper;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;

namespace DntSite.Web.Features.Courses.ModelsMappings;

public class AfterMapCourseModel(IAntiXssService antiXssService) : IMappingAction<CourseModel, Course>
{
    public void Process(CourseModel source, Course destination, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.Description = antiXssService.GetSanitizedHtml(source.Description);
        destination.Requirements = antiXssService.GetSanitizedHtml(source.Requirements);

        destination.IsFree = IsFree(source);
        CheckFreeForAll(source, destination);
        CheckFreeForActiveUsers(source, destination);
        CheckFreeForWriters(source, destination);
    }

    private static void CheckFreeForActiveUsers(CourseModel formData, Course item)
    {
        if (formData.Perm != CourseType.FreeForActiveUsers)
        {
            return;
        }

        item.NumberOfPostsRequired = 0;
        item.NumberOfMonthsRequired = 0;
    }

    private static void CheckFreeForAll(CourseModel formData, Course item)
    {
        if (formData.Perm is not (CourseType.FreeForAll or CourseType.IsNotFree))
        {
            return;
        }

        item.NumberOfPostsRequired = 0;
        item.NumberOfMonthsRequired = 0;
        item.NumberOfTotalRatingsRequired = 0;
        item.NumberOfMonthsTotalRatingsRequired = 0;
    }

    private static void CheckFreeForWriters(CourseModel formData, Course item)
    {
        if (formData.Perm != CourseType.FreeForWriters)
        {
            return;
        }

        item.NumberOfTotalRatingsRequired = 0;
        item.NumberOfMonthsTotalRatingsRequired = 0;
    }

    private static bool IsFree(CourseModel formData) => formData.Perm != CourseType.IsNotFree;
}
