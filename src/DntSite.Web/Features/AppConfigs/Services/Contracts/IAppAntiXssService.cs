using DntSite.Web.Features.Common.RoutingConstants;

namespace DntSite.Web.Features.AppConfigs.Services.Contracts;

public interface IAppAntiXssService : ISingletonService
{
    string GetSanitizedHtml(string? html,
        string? outputImageFolder = null,
        string? imageApiUrlPattern = $"{ApiUrlsRoutingConstants.File.HttpAny.Image}?name=");
}
