using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Projects.Entities;

public class ProjectIssue : BaseInteractiveEntity<ProjectIssue, ProjectIssueVisitor, ProjectIssueBookmark,
    ProjectIssueReaction, ProjectIssueTag, ProjectIssueComment, ProjectIssueCommentVisitor, ProjectIssueCommentBookmark,
    ProjectIssueCommentReaction, ProjectIssueUserFile, ProjectIssueUserFileVisitor>
{
    [StringLength(maximumLength: 450)] public required string Title { set; get; }

    [MaxLength] public required string Description { set; get; }

    [StringLength(maximumLength: 450)] public required string RevisionNumber { set; get; }

    public virtual Project Project { set; get; } = null!;

    public int ProjectId { set; get; }

    public virtual ProjectIssuePriority IssuePriority { set; get; } = null!;

    public int IssuePriorityId { set; get; }

    public virtual ProjectIssueType? IssueType { set; get; }

    public int? IssueTypeId { set; get; }

    public virtual ProjectIssueStatus? IssueStatus { set; get; }

    public int? IssueStatusId { set; get; }
}
