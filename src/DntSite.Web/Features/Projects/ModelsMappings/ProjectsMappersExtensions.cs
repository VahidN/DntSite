using System.Text;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Projects.ModelsMappings;

public static class ProjectsMappersExtensions
{
    private static readonly CompositeFormat ParsedPostUrlTemplate =
        CompositeFormat.Parse(ProjectsRoutingConstants.PostUrlTemplate);

    public static WhatsNewItemModel MapToProjectIssuesWhatsNewItemModel(this ProjectIssueComment item,
        string siteRootUri,
        int projectId)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = item.Body,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectIssuesReplies.Value}: {item.Parent.Title}",
            OriginalTitle = item.Parent.Title,
            Url = siteRootUri.CombineUrl(Invariant(
                $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{projectId}/{item.ParentId}#comment-{item.Id}")),
            Categories = [WhatsNewItemType.ProjectIssuesReplies.Value],
            ItemType = WhatsNewItemType.ProjectIssuesReplies,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToProjectIssueWhatsNewItemModel(this ProjectIssue item, string siteRootUri)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = item.Description,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectIssues.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(
                Invariant($"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{item.ProjectId}/{item.Id}")),
            Categories = [WhatsNewItemType.ProjectIssues.Value],
            ItemType = WhatsNewItemType.ProjectIssues,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToProjectReleaseWhatsNewItemModel(this ProjectRelease item, string siteRootUri)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = item.FileDescription,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectFiles.Value}: {item.FileName}",
            OriginalTitle = item.FileName,
            Url = siteRootUri.CombineUrl(
                Invariant($"{ProjectsRoutingConstants.ProjectReleasesBase}/{item.ProjectId}/{item.Id}")),
            Categories = [WhatsNewItemType.ProjectFiles.Value],
            ItemType = WhatsNewItemType.ProjectFiles,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToProjectFaqWhatsNewItemModel(this ProjectFaq item, string siteRootUri)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = item.Description,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectFaqs.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(
                Invariant($"{ProjectsRoutingConstants.ProjectFaqsBase}/{item.Project.Id}/{item.Id}")),
            Categories = [WhatsNewItemType.ProjectFaqs.Value],
            ItemType = WhatsNewItemType.ProjectFaqs,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToProjectsFaqsWhatsNewItemModel(this ProjectFaq item, string siteRootUri)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = item.Description,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectsFaqs.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(
                Invariant($"{ProjectsRoutingConstants.ProjectFaqsBase}/{item.Project.Id}/{item.Id}")),
            Categories = [WhatsNewItemType.ProjectsFaqs.Value],
            ItemType = WhatsNewItemType.ProjectsFaqs,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToWhatsNewItemModel(this Project item, string siteRootUri)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = item.Description,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectsNews.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture, ParsedPostUrlTemplate, item.Id)),
            Categories = [WhatsNewItemType.ProjectsNews.Value],
            ItemType = WhatsNewItemType.ProjectsNews,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToProjectsReleasesWhatsNewItemModel(this ProjectRelease item, string siteRootUri)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = item.FileDescription,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectsFiles.Value}: {item.FileName}",
            OriginalTitle = item.FileName,
            Url = siteRootUri.CombineUrl(
                Invariant($"{ProjectsRoutingConstants.ProjectReleasesBase}/{item.ProjectId}/{item.Id}")),
            Categories = [WhatsNewItemType.ProjectsFiles.Value],
            ItemType = WhatsNewItemType.ProjectsFiles,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToProjectsIssuesWhatsNewItemModel(this ProjectIssue item, string siteRootUri)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = item.Description,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectsIssues.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(
                Invariant($"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{item.ProjectId}/{item.Id}")),
            Categories = [WhatsNewItemType.ProjectsIssues.Value],
            ItemType = WhatsNewItemType.ProjectsIssues,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToProjectsIssuesWhatsNewItemModel(this ProjectIssueComment item,
        string siteRootUri)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = item.Body,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectsIssuesReplies.Value}: {item.Parent.Title}",
            OriginalTitle = item.Parent.Title,
            Url = siteRootUri.CombineUrl(Invariant(
                $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{item.Parent.ProjectId}/{item.ParentId}#comment-{item.Id}")),
            Categories = [WhatsNewItemType.ProjectsIssuesReplies.Value],
            ItemType = WhatsNewItemType.ProjectsIssuesReplies,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }
}
