using DntSite.Web.Features.StackExchangeQuestions.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

public interface IQuestionsEmailsService : IScopedService
{
    public Task StackExchangeQuestionSendEmailAsync(StackExchangeQuestion result, string friendlyName);

    public Task PostQuestionCommentReplySendEmailToAdminsAsync(StackExchangeQuestionComment data);

    public Task PostQuestionCommentsReplySendEmailToWritersAsync(StackExchangeQuestionComment comment);

    public Task PostQuestionCommentsReplySendEmailToPersonAsync(StackExchangeQuestionComment comment);

    public Task QuestionCommentIsApprovedSendEmailToWritersAsync(StackExchangeQuestionComment? comment);
}
