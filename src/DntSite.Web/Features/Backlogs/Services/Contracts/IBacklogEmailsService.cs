using DntSite.Web.Features.Backlogs.Entities;

namespace DntSite.Web.Features.Backlogs.Services.Contracts;

public interface IBacklogEmailsService : IScopedService
{
    public Task NewBacklogSendEmailToAdminsAsync(Backlog data);
}
