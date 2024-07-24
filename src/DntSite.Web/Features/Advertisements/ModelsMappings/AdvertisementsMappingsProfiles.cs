using AutoMapper;
using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Advertisements.Models;

namespace DntSite.Web.Features.Advertisements.ModelsMappings;

public class AdvertisementsMappingsProfiles : Profile
{
    public AdvertisementsMappingsProfiles()
    {
        MapAdvertisementToModel();
        MapModelToAdvertisement();
    }

    private void MapModelToAdvertisement()
        => CreateMap<WriteAdvertisementModel, Advertisement>(MemberList.None)
            .ForMember(item => item.Tags, opt => opt.Ignore())
            .AfterMap<AfterMapWriteAdvertisementModel>();

    private void MapAdvertisementToModel()
        => CreateMap<Advertisement, WriteAdvertisementModel>(MemberList.None)
            .ForMember(model => model.Tags, opt => opt.MapFrom(post => post.Tags.Select(tag => tag.Name).ToList()))
            .ForMember(model => model.Hour, opt => opt.MapFrom(post => GetHour(post.DueDate)))
            .ForMember(model => model.Minute, opt => opt.MapFrom(post => GetMinute(post.DueDate)));

    private static int GetMinute(DateTime? postDueDate) => postDueDate?.Minute ?? 0;

    private static int GetHour(DateTime? postDueDate) => postDueDate?.Hour ?? 0;
}
