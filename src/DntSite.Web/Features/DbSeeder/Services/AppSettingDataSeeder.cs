using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.DbSeeder.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;

namespace DntSite.Web.Features.DbSeeder.Services;

public class AppSettingDataSeeder(IUnitOfWork uow) : IDataSeeder
{
    private readonly DbSet<AppSetting> _appSettings = uow.DbSet<AppSetting>();
    private readonly IUnitOfWork _uow = uow ?? throw new ArgumentNullException(nameof(uow));

    public int Order { get; set; }

    public void SeedData()
    {
        if (_appSettings.Any())
        {
            return;
        }

        _appSettings.Add(new AppSetting
        {
            BlogName = "DNT",
            SiteIsActive = true,
            SiteEmailsSig = "DNT",
            SmtpServerSetting = new SmtpServerSetting
            {
                Address = "127.0.0.1",
                Port = 25,
                UsePickupFolder = true
            },
            SiteFromEmail = "donotreply@site.ir",
            CanUsersRegister = true,
            SiteRootUri = "https://www.site.ir/",
            UsedPasswords = new UsedPasswordsSetting
            {
                ChangePasswordReminderDays = 120,
                NotAllowedPreviouslyUsedPasswords = 3
            }
        });

        _uow.SaveChanges();
    }
}
