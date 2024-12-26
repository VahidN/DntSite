namespace DntSite.Web.Features.AppConfigs.Services.Contracts;

public interface IAppConfigsEmailsService : IScopedService
{
    public Task SendNewDotNetVersionEmailToAdminsAsync();
}
