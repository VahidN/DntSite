using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.Services.Contracts;

public interface IVotesEmailsService : IScopedService
{
    Task VoteSendEmailAsync(Survey result);

    Task VoteCommentSendEmailToAdminsAsync(SurveyComment comment);

    Task VoteCommentSendEmailToWritersAsync(SurveyComment comment);

    Task VoteCommentSendEmailToPersonAsync(SurveyComment comment);
}
