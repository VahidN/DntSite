namespace DntSite.Web.Features.AppConfigs.Models;

public class StartupSettingsModel
{
    public required AdminUserSeedModel AdminUserSeed { get; set; }

    public required ConnectionStringsModel ConnectionStrings { get; set; }

    public required DataProtectionSettingsModel DataProtectionOptions { get; set; }

    public required LoggingModel Logging { get; set; }

    public required SiteVerificationModel SiteVerification { get; set; }
}
