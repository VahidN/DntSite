using AutoMapper;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;

namespace DntSite.Web.Features.Courses.ModelsMappings;

public class AfterMapCourseTopicModel(IAppAntiXssService antiXssService)
    : IMappingAction<CourseTopicItemModel, CourseTopic>
{
    public void Process(CourseTopicItemModel source, CourseTopic destination, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        var body = antiXssService.GetSanitizedHtml(source.Body);
        destination.Body = body;
        destination.ReadingTimeMinutes = body.MinReadTime();
        destination.DisplayId = Guid.NewGuid();
    }
}
