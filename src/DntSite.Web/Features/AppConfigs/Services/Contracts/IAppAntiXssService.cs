namespace DntSite.Web.Features.AppConfigs.Services.Contracts;

public interface IAppAntiXssService : ISingletonService
{
    public string GetSanitizedHtml(string? html);
}
