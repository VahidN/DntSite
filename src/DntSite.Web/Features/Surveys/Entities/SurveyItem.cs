using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Surveys.Entities;

[IgnoreSoftDelete]
public class SurveyItem : BaseAuditedEntity
{
    [StringLength(maximumLength: 1000)] public required string Title { set; get; }

    [IgnoreAudit] public int TotalSurveys { set; get; }

    public int Order { set; get; }

    public virtual ICollection<User> Users { set; get; } = [];

    public virtual Survey Survey { set; get; } = null!;

    public int SurveyId { set; get; }
}
