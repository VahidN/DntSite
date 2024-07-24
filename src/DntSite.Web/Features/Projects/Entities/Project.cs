using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Projects.Entities;

public class Project : BaseInteractiveEntity<Project, ProjectVisitor, ProjectBookmark, ProjectReaction, ProjectTag,
    ProjectComment, ProjectCommentVisitor, ProjectCommentBookmark, ProjectCommentReaction, ProjectUserFile,
    ProjectUserFileVisitor>
{
    [StringLength(maximumLength: 450)] public required string Title { set; get; }

    [MaxLength] public required string Description { set; get; }

    [MaxLength] public string? RequiredDependencies { set; get; }

    [MaxLength] public string? RelatedArticles { set; get; }

    [MaxLength] public string? DevelopersDescription { set; get; }

    [StringLength(maximumLength: 1000)] public string? SourcecodeRepositoryUrl { set; get; }

    [MaxLength] public required string License { set; get; }

    [StringLength(maximumLength: 1000)] public string? Logo { set; get; }

    public virtual ICollection<ProjectIssue> ProjectIssues { set; get; } = new List<ProjectIssue>();

    public virtual ICollection<ProjectRelease> ProjectReleases { set; get; } = new List<ProjectRelease>();

    public virtual ICollection<ProjectFaq> ProjectFaqs { set; get; } = new List<ProjectFaq>();

    [IgnoreAudit] public int NumberOfFaqs { set; get; }

    [IgnoreAudit] public int NumberOfIssues { set; get; }

    [IgnoreAudit] public int NumberOfReleases { set; get; }

    [IgnoreAudit] public int NumberOfIssuesComments { set; get; }
}
