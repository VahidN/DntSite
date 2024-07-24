namespace DntSite.Web.Features.AppConfigs.Models;

public class AppLogItemModel
{
    public int Id { get; set; }

    public DateTime CreatedAt { set; get; }

    public int EventId { get; set; }

    public string Url { get; set; } = default!;

    public string LogLevel { get; set; } = default!;

    public string Logger { get; set; } = default!;

    public string Message { get; set; } = default!;

    public string UserIp { set; get; } = default!;

    public string UserAgent { set; get; } = default!;

    public string UserFriendlyName { set; get; } = default!;

    public int UserId { set; get; }
}
