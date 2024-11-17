using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Surveys.Entities;

public class Survey : BaseInteractiveEntity<Survey, SurveyVisitor, SurveyBookmark, SurveyReaction, SurveyTag,
    SurveyComment, SurveyCommentVisitor, SurveyCommentBookmark, SurveyCommentReaction, SurveyUserFile,
    SurveyUserFileVisitor>
{
    [StringLength(maximumLength: 450)] public required string Title { set; get; }

    [IgnoreAudit] public int TotalRaters { get; set; }

    public bool AllowMultipleSelection { set; get; }

    public DateTime? DueDate { set; get; }

    [MaxLength] public string? Description { set; get; }

    public virtual ICollection<SurveyItem> SurveyItems { set; get; } = [];
}
