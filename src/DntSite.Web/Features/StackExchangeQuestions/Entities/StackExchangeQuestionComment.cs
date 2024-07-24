using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.Entities;

public class StackExchangeQuestionComment : BaseCommentsEntity<StackExchangeQuestionComment, StackExchangeQuestion,
    StackExchangeQuestionCommentVisitor, StackExchangeQuestionCommentBookmark, StackExchangeQuestionCommentReaction>
{
    public bool IsAnswer { set; get; }
}
