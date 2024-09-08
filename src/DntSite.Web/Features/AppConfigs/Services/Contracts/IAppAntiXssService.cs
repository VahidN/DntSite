namespace DntSite.Web.Features.AppConfigs.Services.Contracts;

public interface IAppAntiXssService : ISingletonService
{
    string GetSanitizedHtml(string? html);
}
