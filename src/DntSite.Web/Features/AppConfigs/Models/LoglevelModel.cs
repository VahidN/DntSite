namespace DntSite.Web.Features.AppConfigs.Models;

public class LoglevelModel
{
    public LogLevel Default { get; set; } = default!;

    public LogLevel System { get; set; } = default!;

    public LogLevel Microsoft { get; set; } = default!;
}
