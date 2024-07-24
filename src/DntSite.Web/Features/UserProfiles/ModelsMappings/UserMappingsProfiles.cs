using AutoMapper;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.UserProfiles.ModelsMappings;

public class UserMappingsProfiles : Profile
{
    public UserMappingsProfiles()
    {
        MapUserProfileModel();
        MapUserSocialNetworkModel();
    }

    private void MapUserSocialNetworkModel()
    {
        CreateMap<UserSocialNetwork, UserSocialNetworkModel>(MemberList.Destination);
        CreateMap<UserSocialNetworkModel, UserSocialNetwork>(MemberList.Source);
    }

    private void MapUserProfileModel()
        => CreateMap<User, UserProfileModel>()
            .ForMember(model => model.DateOfBirthYear,
                opt => opt.MapFrom(user
                    => user.DateOfBirth == null ? (int?)null : user.DateOfBirth.ToPersianYearMonthDay(true)!.Year))
            .ForMember(model => model.DateOfBirthMonth,
                opt => opt.MapFrom(user
                    => user.DateOfBirth == null ? (int?)null : user.DateOfBirth.ToPersianYearMonthDay(true)!.Month))
            .ForMember(model => model.DateOfBirthDay,
                opt => opt.MapFrom(user
                    => user.DateOfBirth == null ? (int?)null : user.DateOfBirth.ToPersianYearMonthDay(true)!.Day))
            .ForMember(model => model.PhotoFiles, opt => opt.Ignore());
}
