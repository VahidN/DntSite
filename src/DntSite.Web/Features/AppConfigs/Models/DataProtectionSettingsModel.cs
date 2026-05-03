namespace DntSite.Web.Features.AppConfigs.Models;

public class DataProtectionSettingsModel
{
    public TimeSpan DataProtectionKeyLifetime { get; set; }

    [Required]
    [StringLength(maximumLength: 1000)]
    public string ApplicationName { get; set; } = null!;

    public int LoginCookieExpirationDays { set; get; }
}
