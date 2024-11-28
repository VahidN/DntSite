using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.Services.Contracts;

public interface ICourseTagsService : IScopedService
{
    public Task<List<CourseTag>> GetAllCourseTagsListAsNoTrackingAsync(int count, bool isActive = true);

    public CourseTag AddCourseTag(CourseTag courseTag);

    public ValueTask<CourseTag?> FindCourseTagAsync(int tagId);
}
