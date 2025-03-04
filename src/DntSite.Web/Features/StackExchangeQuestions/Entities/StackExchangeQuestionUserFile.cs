using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.Entities;

public class StackExchangeQuestionUserFile : BaseUserFileEntity<StackExchangeQuestionUserFile, StackExchangeQuestion,
    StackExchangeQuestionUserFileVisitor>;
