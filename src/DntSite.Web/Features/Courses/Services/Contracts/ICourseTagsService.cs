using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.Services.Contracts;

public interface ICourseTagsService : IScopedService
{
    Task<List<CourseTag>> GetAllCourseTagsListAsNoTrackingAsync(int count, bool isActive = true);

    CourseTag AddCourseTag(CourseTag courseTag);

    ValueTask<CourseTag?> FindCourseTagAsync(int tagId);
}
