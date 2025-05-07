namespace DntSite.Web.Features.AppConfigs.Services.Contracts;

public interface IAppConfigsEmailsService : IScopedService, IDisposable
{
    public Task SendNewDotNetVersionEmailToAdminsAsync(CancellationToken cancellationToken);

    public Task SendHasNotRemainingSpaceEmailToAdminsAsync(CancellationToken cancellationToken);
}
