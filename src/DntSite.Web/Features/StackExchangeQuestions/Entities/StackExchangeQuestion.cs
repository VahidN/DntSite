using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.Entities;

public class StackExchangeQuestion : BaseInteractiveEntity<StackExchangeQuestion, StackExchangeQuestionVisitor,
    StackExchangeQuestionBookmark, StackExchangeQuestionReaction, StackExchangeQuestionTag, StackExchangeQuestionComment
    , StackExchangeQuestionCommentVisitor, StackExchangeQuestionCommentBookmark, StackExchangeQuestionCommentReaction,
    StackExchangeQuestionUserFile, StackExchangeQuestionUserFileVisitor>
{
    [StringLength(maximumLength: 450)] public required string Title { set; get; }

    [MaxLength] public required string Description { set; get; }

    public bool IsAnswered { set; get; }
}
