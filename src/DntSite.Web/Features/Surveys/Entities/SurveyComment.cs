using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Surveys.Entities;

public class SurveyComment : BaseCommentsEntity<SurveyComment, Survey, SurveyCommentVisitor, SurveyCommentBookmark,
    SurveyCommentReaction>
{
    public bool EmailsSent { set; get; }
}
