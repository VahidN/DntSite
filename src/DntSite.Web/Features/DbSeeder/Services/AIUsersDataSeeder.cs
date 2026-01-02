using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.DbSeeder.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.UserProfiles.Entities;
using Microsoft.Extensions.Options;

namespace DntSite.Web.Features.DbSeeder.Services;

public class AIUsersDataSeeder(
    IUnitOfWork uow,
    IPasswordHasherService passwordHasherService,
    IOptionsSnapshot<StartupSettingsModel> siteSettingsRoot) : IDataSeeder
{
    private readonly DbSet<User> _users = uow.DbSet<User>();

    public int Order { get; set; } = 2;

    public void SeedData() => AddNewsLinksAiUser();

    private void AddNewsLinksAiUser()
    {
        var aiUserOptions = siteSettingsRoot.Value.NewsLinksAIUserSeed;

        if (_users.Any(x => x.UserName == aiUserOptions.Username))
        {
            return;
        }

        var newsLinksAIUser = new User
        {
            UserName = aiUserOptions.Username,
            FriendlyName = aiUserOptions.FriendlyName,
            IsActive = true,
            LastVisitDateTime = null,
            HashedPassword =
                passwordHasherService.GetPbkdf2Hash(passwordHasherService.CreateCryptographicallySecureGuid()
                    .ToString(format: "N")),
            SerialNumber = Guid.NewGuid().ToString(format: "N"),
            EMail = aiUserOptions.Email,
            EmailIsValidated = true,
            Description = "کاربر هوش مصنوعی سایت با هدف تهیه‌ی خلاصه‌ای از لینک‌های خبری",
            RegistrationCode = Guid.NewGuid().ToString(format: "N"),
            ReceiveDailyEmails = false
        };

        _users.Add(newsLinksAIUser);
        uow.SaveChanges();
    }
}
