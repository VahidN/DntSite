using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;

namespace DntSite.Web.Features.Courses.Services;

public class CourseTagsService(IUnitOfWork uow) : ICourseTagsService
{
    private readonly DbSet<CourseTag> _courseTags = uow.DbSet<CourseTag>();

    public Task<List<CourseTag>> GetAllCourseTagsListAsNoTrackingAsync(int count, bool isActive = true)
        => _courseTags.AsNoTracking()
            .Where(x => x.IsDeleted != isActive)
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name)
            .Take(count)
            .ToListAsync();

    public CourseTag AddCourseTag(CourseTag courseTag) => _courseTags.Add(courseTag).Entity;

    public ValueTask<CourseTag?> FindCourseTagAsync(int tagId) => _courseTags.FindAsync(tagId);
}
