using AutoMapper;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;

namespace DntSite.Web.Features.Courses.ModelsMappings;

public class CoursesProfiles : Profile
{
    public CoursesProfiles()
    {
        MapCourseToModel();
        MapModelToCourse();

        MapCourseTopicToModel();
        MapModelToCourseTopic();
    }

    private void MapModelToCourseTopic()
        => CreateMap<CourseTopicItemModel, CourseTopic>(MemberList.None).AfterMap<AfterMapCourseTopicModel>();

    private void MapCourseTopicToModel() => CreateMap<CourseTopic, CourseTopicItemModel>(MemberList.None);

    private void MapModelToCourse()
        => CreateMap<CourseModel, Course>(MemberList.None)
            .ForMember(course => course.Tags, opt => opt.Ignore())
            .AfterMap<AfterMapCourseModel>();

    private void MapCourseToModel()
        => CreateMap<Course, CourseModel>(MemberList.None)
            .ForMember(model => model.Tags, opt => opt.MapFrom(course => course.Tags.Select(tag => tag.Name).ToList()));
}
