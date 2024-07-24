using AutoMapper;
using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Backlogs.Models;

namespace DntSite.Web.Features.Backlogs.ModelsMappings;

public class BacklogsMappingsProfiles : Profile
{
    public BacklogsMappingsProfiles()
    {
        MapBacklogToModel();
        MapModelToBacklog();
    }

    private void MapModelToBacklog()
        => CreateMap<BacklogModel, Backlog>(MemberList.None)
            .ForMember(item => item.Tags, opt => opt.Ignore())
            .AfterMap<AfterMapBacklogModel>();

    private void MapBacklogToModel()
        => CreateMap<Backlog, BacklogModel>(MemberList.None)
            .ForMember(model => model.Tags, opt => opt.MapFrom(post => post.Tags.Select(tag => tag.Name).ToList()));
}
