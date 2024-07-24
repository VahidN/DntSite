using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.UserProfiles.Entities;

public class UserSocialNetwork : BaseAuditedEntity
{
    [StringLength(maximumLength: 1000)] public string? FacebookName { set; get; }

    [StringLength(maximumLength: 1000)] public string? TwitterName { set; get; }

    [StringLength(maximumLength: 1000)] public string? LinkedInProfileId { set; get; }

    [StringLength(maximumLength: 1000)] public string? GooglePlusProfileId { set; get; }

    [StringLength(maximumLength: 1000)] public string? StackOverflowId { set; get; }

    [StringLength(maximumLength: 1000)] public string? GithubId { set; get; }

    [StringLength(maximumLength: 1000)] public string? NugetId { set; get; }

    [StringLength(maximumLength: 1000)] public string? CodePlexId { set; get; }

    [StringLength(maximumLength: 1000)] public string? CodeProjectId { set; get; }

    [StringLength(maximumLength: 1000)] public string? SourceforgeId { set; get; }

    [StringLength(maximumLength: 1000)] public string? TelegramId { set; get; }

    [StringLength(maximumLength: 1000)] public string? CoffeebedeId { set; get; }

    [StringLength(maximumLength: 1000)] public string? YouTubeId { set; get; }

    [StringLength(maximumLength: 1000)] public string? RedditId { set; get; }
}
