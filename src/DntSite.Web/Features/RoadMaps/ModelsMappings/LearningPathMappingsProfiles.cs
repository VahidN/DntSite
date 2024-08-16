using AutoMapper;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.RoadMaps.Models;

namespace DntSite.Web.Features.RoadMaps.ModelsMappings;

public class LearningPathMappingsProfiles : Profile
{
    public const string LearningPathTags = $"{nameof(LearningPath)}_Tags";

    public LearningPathMappingsProfiles()
    {
        MapLearningPathToModel();
        MapModelToLearningPath();
    }

    private void MapModelToLearningPath()
        => CreateMap<LearningPathModel, LearningPath>(MemberList.None)
            .ForMember(newsItem => newsItem.Tags, opt => opt.Ignore())
            .AfterMap<AfterMapLearningPathModel>();

    private void MapLearningPathToModel()
        => CreateMap<LearningPath, LearningPathModel>(MemberList.None)
            .ForMember(model => model.Tags, opt => opt.MapFrom(post => post.Tags.Select(tag => tag.Name).ToList()));
}
