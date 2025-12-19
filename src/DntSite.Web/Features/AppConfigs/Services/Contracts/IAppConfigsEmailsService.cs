namespace DntSite.Web.Features.AppConfigs.Services.Contracts;

public interface IAppConfigsEmailsService : IScopedService, IDisposable
{
    Task SendNewDotNetVersionEmailToAdminsAsync(CancellationToken cancellationToken);

    Task SendHasNotRemainingSpaceEmailToAdminsAsync(CancellationToken cancellationToken);
}
