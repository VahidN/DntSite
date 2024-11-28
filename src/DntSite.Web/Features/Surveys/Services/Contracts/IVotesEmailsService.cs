using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.Services.Contracts;

public interface IVotesEmailsService : IScopedService
{
    public Task VoteSendEmailAsync(Survey result);

    public Task VoteCommentSendEmailToAdminsAsync(SurveyComment comment);

    public Task VoteCommentSendEmailToWritersAsync(SurveyComment comment);

    public Task VoteCommentSendEmailToPersonAsync(SurveyComment comment);
}
