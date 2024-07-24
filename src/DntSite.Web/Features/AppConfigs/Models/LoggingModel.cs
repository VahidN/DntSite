namespace DntSite.Web.Features.AppConfigs.Models;

public class LoggingModel
{
    public bool IncludeScopes { get; set; } = default!;

    public LoglevelModel LogLevel { get; set; } = default!;
}
