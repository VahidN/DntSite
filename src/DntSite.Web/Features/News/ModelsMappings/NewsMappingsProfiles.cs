using AutoMapper;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.Models;

namespace DntSite.Web.Features.News.ModelsMappings;

public class NewsMappingsProfiles : Profile
{
    public NewsMappingsProfiles()
    {
        MapDailyNewsItemToModel();
        MapModelToDailyNewsItem();
    }

    private void MapModelToDailyNewsItem()
        => CreateMap<DailyNewsItemModel, DailyNewsItem>(MemberList.None)
            .ForMember(newsItem => newsItem.BriefDescription, opt => opt.MapFrom(model => model.DescriptionText))
            .ForMember(newsItem => newsItem.Tags, opt => opt.Ignore())
            .AfterMap<AfterMapDailyNewsItemModel>();

    private void MapDailyNewsItemToModel()
        => CreateMap<DailyNewsItem, DailyNewsItemModel>(MemberList.None)
            .ForMember(model => model.DescriptionText, opt => opt.MapFrom(draft => draft.BriefDescription))
            .ForMember(model => model.Tags, opt => opt.MapFrom(post => post.Tags.Select(tag => tag.Name).ToList()));
}
