using System.Text;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Projects.ModelsMappings;

public static class ProjectsMappersExtensions
{
    public const string ProjectTags = $"{nameof(Project)}_Tags";

    private static readonly CompositeFormat ParsedPostUrlTemplate =
        CompositeFormat.Parse(ProjectsRoutingConstants.PostUrlTemplate);

    public static WhatsNewItemModel MapToProjectIssuesWhatsNewItemModel(this ProjectIssueComment item,
        string siteRootUri,
        int projectId,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = showBriefDescription ? item.Body.GetBriefDescription(charLength: 450) : item.Body,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectIssuesReplies.Value}: {item.Parent.Title}",
            OriginalTitle = item.Parent.Title,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture,
                    $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{projectId}/{item.ParentId}#comment-{item.Id}"),
                escapeRelativeUrl: false),
            Categories = [],
            ItemType = WhatsNewItemType.ProjectIssuesReplies,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToProjectIssueWhatsNewItemModel(this ProjectIssue item,
        string siteRootUri,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = showBriefDescription ? item.Description.GetBriefDescription(charLength: 450) : item.Description,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectIssues.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture,
                    $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{item.ProjectId}/{item.Id}"),
                escapeRelativeUrl: false),
            Categories = [],
            ItemType = WhatsNewItemType.ProjectIssues,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToProjectReleaseWhatsNewItemModel(this ProjectRelease item,
        string siteRootUri,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content =
                showBriefDescription ? item.FileDescription.GetBriefDescription(charLength: 450) : item.FileDescription,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectFiles.Value}: {item.FileName}",
            OriginalTitle = item.FileName,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture,
                    $"{ProjectsRoutingConstants.ProjectReleasesBase}/{item.ProjectId}/{item.Id}"),
                escapeRelativeUrl: false),
            Categories = [],
            ItemType = WhatsNewItemType.ProjectFiles,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToProjectFaqWhatsNewItemModel(this ProjectFaq item,
        string siteRootUri,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = showBriefDescription ? item.Description.GetBriefDescription(charLength: 450) : item.Description,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectFaqs.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture,
                    $"{ProjectsRoutingConstants.ProjectFaqsBase}/{item.Project.Id}/{item.Id}"),
                escapeRelativeUrl: false),
            Categories = [],
            ItemType = WhatsNewItemType.ProjectFaqs,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToProjectsFaqsWhatsNewItemModel(this ProjectFaq item,
        string siteRootUri,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = showBriefDescription ? item.Description.GetBriefDescription(charLength: 450) : item.Description,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectsFaqs.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture,
                    $"{ProjectsRoutingConstants.ProjectFaqsBase}/{item.Project.Id}/{item.Id}"),
                escapeRelativeUrl: false),
            Categories = [],
            ItemType = WhatsNewItemType.ProjectsFaqs,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToWhatsNewItemModel(this Project item,
        string siteRootUri,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = showBriefDescription ? item.Description.GetBriefDescription(charLength: 450) : item.Description,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectsNews.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture, ParsedPostUrlTemplate, item.Id),
                escapeRelativeUrl: false),
            Categories = item.Tags.Select(x => x.Name),
            ItemType = WhatsNewItemType.ProjectsNews,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToProjectsReleasesWhatsNewItemModel(this ProjectRelease item,
        string siteRootUri,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content =
                showBriefDescription ? item.FileDescription.GetBriefDescription(charLength: 450) : item.FileDescription,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectsFiles.Value}: {item.FileName}",
            OriginalTitle = item.FileName,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture,
                    $"{ProjectsRoutingConstants.ProjectReleasesBase}/{item.ProjectId}/{item.Id}"),
                escapeRelativeUrl: false),
            Categories = [],
            ItemType = WhatsNewItemType.ProjectsFiles,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToProjectsIssuesWhatsNewItemModel(this ProjectIssue item,
        string siteRootUri,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = showBriefDescription ? item.Description.GetBriefDescription(charLength: 450) : item.Description,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectsIssues.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture,
                    $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{item.ProjectId}/{item.Id}"),
                escapeRelativeUrl: false),
            Categories = [],
            ItemType = WhatsNewItemType.ProjectsIssues,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToProjectsIssuesWhatsNewItemModel(this ProjectIssueComment item,
        string siteRootUri,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = showBriefDescription ? item.Body.GetBriefDescription(charLength: 450) : item.Body,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.ProjectsIssuesReplies.Value}: {item.Parent.Title}",
            OriginalTitle = item.Parent.Title,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture,
                    $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{item.Parent.ProjectId}/{item.ParentId}#comment-{item.Id}"),
                escapeRelativeUrl: false),
            Categories = [],
            ItemType = WhatsNewItemType.ProjectsIssuesReplies,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static Project MapProjectModelToProject(this ProjectModel source,
        IAppAntiXssService antiXssService,
        Project? destination = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(antiXssService);

        var project = new Project
        {
            Title = source.Title,
            Description = antiXssService.GetSanitizedHtml(source.DescriptionText),
            RequiredDependencies = antiXssService.GetSanitizedHtml(source.RequiredDependenciesText),
            RelatedArticles = antiXssService.GetSanitizedHtml(source.RelatedArticlesText),
            DevelopersDescription = antiXssService.GetSanitizedHtml(source.DevelopersDescriptionText),
            License = antiXssService.GetSanitizedHtml(source.LicenseText)
        };

        if (destination is not null)
        {
            destination.Title = project.Title;
            destination.Description = project.Description;
            destination.RequiredDependencies = project.RequiredDependencies;
            destination.RelatedArticles = project.RelatedArticles;
            destination.DevelopersDescription = project.DevelopersDescription;
            destination.License = project.License;
        }

        return destination ?? project;
    }

    public static ProjectModel MapProjectToProjectModel(this Project source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new ProjectModel
        {
            Title = source.Title,
            DescriptionText = source.Description,
            RequiredDependenciesText = source.RequiredDependencies,
            RelatedArticlesText = source.RelatedArticles,
            DevelopersDescriptionText = source.DevelopersDescription,
            LicenseText = source.License,
            Tags = [..source.Tags?.Select(tag => tag.Name) ?? []]
        };
    }

    public static ProjectFaq MapProjectFaqFormModelToProjectFaq(this ProjectFaqFormModel source,
        IAppAntiXssService antiXssService,
        ProjectFaq? destination = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(antiXssService);

        var projectFaq = new ProjectFaq
        {
            Title = source.Title,
            Description = antiXssService.GetSanitizedHtml(source.DescriptionText)
        };

        if (destination is not null)
        {
            destination.Title = projectFaq.Title;
            destination.Description = projectFaq.Description;
        }

        return destination ?? projectFaq;
    }

    public static ProjectFaqFormModel MapProjectFaqToProjectFaqFormModel(this ProjectFaq source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new ProjectFaqFormModel
        {
            Title = source.Title,
            DescriptionText = source.Description
        };
    }

    public static ProjectRelease MapProjectPostFileModelToProjectRelease(this ProjectPostFileModel source,
        IAppAntiXssService antiXssService,
        ProjectRelease? destination = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(antiXssService);

        var projectRelease = new ProjectRelease
        {
            FileName = source.FileName ?? "",
            FileDescription = antiXssService.GetSanitizedHtml(source.Description)
        };

        if (destination is not null)
        {
            destination.FileName = projectRelease.FileName;
            destination.FileDescription = projectRelease.FileDescription;
        }

        return destination ?? projectRelease;
    }

    public static ProjectPostFileModel MapProjectReleaseToProjectPostFileModel(this ProjectRelease source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new ProjectPostFileModel
        {
            FileName = source.FileName,
            Description = source.FileDescription
        };
    }

    public static ProjectIssue MapIssueModelToProjectIssue(this IssueModel source,
        IAppAntiXssService antiXssService,
        ProjectIssue? destination = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(antiXssService);

        var projectIssue = new ProjectIssue
        {
            Title = source.Title,
            Description = antiXssService.GetSanitizedHtml(source.Description),
            IssueTypeId = source.IssueTypeId,
            RevisionNumber = source.RevisionNumber,
            IssuePriorityId = source.IssuePriorityId
        };

        if (destination is not null)
        {
            destination.Title = projectIssue.Title;
            destination.Description = projectIssue.Description;
            destination.IssueTypeId = source.IssueTypeId;
            destination.RevisionNumber = source.RevisionNumber;
            destination.IssuePriorityId = source.IssuePriorityId;
        }

        return destination ?? projectIssue;
    }

    public static IssueModel MapProjectIssueToIssueModel(this ProjectIssue source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new IssueModel
        {
            Title = source.Title,
            Description = source.Description,
            IssueTypeId = source.IssueTypeId ?? 0,
            RevisionNumber = source.RevisionNumber,
            IssuePriorityId = source.IssuePriorityId
        };
    }
}
