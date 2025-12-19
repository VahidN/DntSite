using DntSite.Web.Features.StackExchangeQuestions.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

public interface IQuestionsEmailsService : IScopedService
{
    Task StackExchangeQuestionSendEmailAsync(StackExchangeQuestion result, string friendlyName);

    Task PostQuestionCommentReplySendEmailToAdminsAsync(StackExchangeQuestionComment data);

    Task PostQuestionCommentsReplySendEmailToWritersAsync(StackExchangeQuestionComment comment);

    Task PostQuestionCommentsReplySendEmailToPersonAsync(StackExchangeQuestionComment comment);

    Task QuestionCommentIsApprovedSendEmailToWritersAsync(StackExchangeQuestionComment? comment);
}
