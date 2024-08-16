using AutoMapper;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;

namespace DntSite.Web.Features.Projects.ModelsMappings;

public class ProjectsMappingsProfiles : Profile
{
    public const string ProjectTags = $"{nameof(Project)}_Tags";

    public ProjectsMappingsProfiles()
    {
        MapProjectToModel();
        MapModelToProject();

        MapProjectFaqToModel();
        MapModelToProjectFaq();

        MapProjectReleaseToModel();
        MapModelToProjectRelease();

        MapProjectIssueToModel();
        MapModelToProjectIssue();
    }

    private void MapModelToProjectIssue() => CreateMap<ProjectIssue, IssueModel>(MemberList.None);

    private void MapProjectIssueToModel()
        => CreateMap<IssueModel, ProjectIssue>(MemberList.None).AfterMap<AfterMapIssueModel>();

    private void MapModelToProjectRelease()
        => CreateMap<ProjectPostFileModel, ProjectRelease>(MemberList.None)
            .ForMember(project => project.FileName, opt => opt.Ignore())
            .AfterMap<AfterMapProjectPostFileModel>();

    private void MapProjectReleaseToModel()
        => CreateMap<ProjectRelease, ProjectPostFileModel>(MemberList.None)
            .ForMember(model => model.Description, opt => opt.MapFrom(project => project.FileDescription))
            .ForMember(model => model.ProjectFiles, opt => opt.Ignore());

    private void MapModelToProjectFaq()
        => CreateMap<ProjectFaqFormModel, ProjectFaq>(MemberList.None)
            .ForMember(projectFaq => projectFaq.Tags, opt => opt.Ignore())
            .AfterMap<AfterMapProjectFaqFormModel>();

    private void MapProjectFaqToModel()
        => CreateMap<ProjectFaq, ProjectFaqFormModel>(MemberList.None)
            .ForMember(model => model.DescriptionText, opt => opt.MapFrom(project => project.Description));

    private void MapModelToProject()
        => CreateMap<ProjectModel, Project>(MemberList.None)
            .ForMember(project => project.Tags, opt => opt.Ignore())
            .ForMember(project => project.Logo, opt => opt.Ignore())
            .AfterMap<AfterMapProjectModel>();

    private void MapProjectToModel()
        => CreateMap<Project, ProjectModel>(MemberList.None)
            .ForMember(model => model.DescriptionText, opt => opt.MapFrom(project => project.Description))
            .ForMember(model => model.RequiredDependenciesText,
                opt => opt.MapFrom(project => project.RequiredDependencies))
            .ForMember(model => model.RelatedArticlesText, opt => opt.MapFrom(project => project.RelatedArticles))
            .ForMember(model => model.DevelopersDescriptionText,
                opt => opt.MapFrom(project => project.DevelopersDescription))
            .ForMember(model => model.LicenseText, opt => opt.MapFrom(project => project.License))
            .ForMember(model => model.Tags,
                opt => opt.MapFrom(project => project.Tags.Select(tag => tag.Name).ToList()))
            .ForMember(model => model.PhotoFiles, opt => opt.Ignore());
}
