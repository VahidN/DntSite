using DntSite.Web.Features.Backlogs.Entities;

namespace DntSite.Web.Features.Backlogs.Services.Contracts;

public interface IBacklogEmailsService : IScopedService
{
    Task NewBacklogSendEmailToAdminsAsync(Backlog data);
}
