namespace DntSite.Web.Features.AppConfigs.Models;

public class DataProtectionSettingsModel
{
    public TimeSpan DataProtectionKeyLifetime { get; set; } = default!;

    [Required] [StringLength(1000)] public string ApplicationName { get; set; } = default!;

    public int LoginCookieExpirationDays { set; get; }
}
