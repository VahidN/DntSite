using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Projects.Entities;

public class ProjectRelease : BaseInteractiveEntity<ProjectRelease, ProjectReleaseVisitor, ProjectReleaseBookmark,
    ProjectReleaseReaction, ProjectReleaseTag, ProjectReleaseComment, ProjectReleaseCommentVisitor,
    ProjectReleaseCommentBookmark, ProjectReleaseCommentReaction, ProjectReleaseUserFile, ProjectReleaseUserFileVisitor>
{
    [StringLength(maximumLength: 1000)] public required string FileName { get; set; }

    [MaxLength] public required string FileDescription { get; set; }

    public long FileSize { get; set; }

    [IgnoreAudit] public int NumberOfDownloads { get; set; }

    public virtual Project Project { set; get; } = null!;

    public int ProjectId { set; get; }
}
