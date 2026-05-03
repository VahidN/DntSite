namespace DntSite.Web.Features.AppConfigs.Models;

public class LoggingModel
{
    public bool IncludeScopes { get; set; }

    public required LoglevelModel LogLevel { get; set; }
}
