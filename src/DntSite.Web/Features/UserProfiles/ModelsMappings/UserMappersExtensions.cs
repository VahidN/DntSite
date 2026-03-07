using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.UserProfiles.ModelsMappings;

public static class UserMappersExtensions
{
    public static UserProfileModel MapUserToUserProfileModel(this User source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var birthDate = source.DateOfBirth?.ToPersianYearMonthDay(convertToIranTimeZone: true);

        return new UserProfileModel
        {
            FriendlyName = source.FriendlyName,
            EMail = source.EMail,
            HomePageUrl = source.HomePageUrl,
            IsEmailPublic = source.IsEmailPublic,
            DateOfBirthYear = birthDate?.Year,
            DateOfBirthMonth = birthDate?.Month,
            DateOfBirthDay = birthDate?.Day
        };
    }

    public static UserSocialNetworkModel MapUserSocialNetworkToUserSocialNetworkModel(this UserSocialNetwork source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new UserSocialNetworkModel
        {
            FacebookName = source.FacebookName,
            TwitterName = source.TwitterName,
            LinkedInProfileId = source.LinkedInProfileId,
            StackOverflowId = source.StackOverflowId,
            GithubId = source.GithubId,
            NugetId = source.NugetId,
            CodeProjectId = source.CodeProjectId,
            SourceforgeId = source.SourceforgeId,
            TelegramId = source.TelegramId,
            CoffeebedeId = source.CoffeebedeId,
            YouTubeId = source.YouTubeId,
            RedditId = source.RedditId
        };
    }

    public static UserSocialNetwork MapUserSocialNetworkModelToUserSocialNetwork(this UserSocialNetworkModel source,
        UserSocialNetwork? destination = null)
    {
        ArgumentNullException.ThrowIfNull(source);

        var userSocialNetwork = new UserSocialNetwork
        {
            FacebookName = source.FacebookName,
            TwitterName = source.TwitterName,
            LinkedInProfileId = source.LinkedInProfileId,
            StackOverflowId = source.StackOverflowId,
            GithubId = source.GithubId,
            NugetId = source.NugetId,
            CodeProjectId = source.CodeProjectId,
            SourceforgeId = source.SourceforgeId,
            TelegramId = source.TelegramId,
            CoffeebedeId = source.CoffeebedeId,
            YouTubeId = source.YouTubeId,
            RedditId = source.RedditId
        };

        if (destination is not null)
        {
            destination.FacebookName = userSocialNetwork.FacebookName;
            destination.TwitterName = userSocialNetwork.TwitterName;
            destination.LinkedInProfileId = userSocialNetwork.LinkedInProfileId;
            destination.GooglePlusProfileId = userSocialNetwork.GooglePlusProfileId;
            destination.StackOverflowId = userSocialNetwork.StackOverflowId;
            destination.GithubId = userSocialNetwork.GithubId;
            destination.NugetId = userSocialNetwork.NugetId;
            destination.CodePlexId = userSocialNetwork.CodePlexId;
            destination.CodeProjectId = userSocialNetwork.CodeProjectId;
            destination.SourceforgeId = userSocialNetwork.SourceforgeId;
            destination.TelegramId = userSocialNetwork.TelegramId;
            destination.CoffeebedeId = userSocialNetwork.CoffeebedeId;
            destination.YouTubeId = userSocialNetwork.YouTubeId;
            destination.RedditId = userSocialNetwork.RedditId;
        }

        return destination ?? userSocialNetwork;
    }
}
