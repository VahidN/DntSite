using AutoMapper;
using DNTPersianUtils.Core.Normalizer;
using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Services;

public class UserProfilesManagerService(
    IUnitOfWork uow,
    IMapper mapper,
    IUploadFileService uploadFileService,
    IAppAntiXssService antiXssService,
    IUsersInfoService usersInfoService,
    ICurrentUserService currentUserService,
    BaseHttpClient baseHttpClient,
    IUsedPasswordsService usedPasswordsService,
    IUsersManagerEmailsService usersManagerEmailsService,
    IAppFoldersService appFoldersService,
    IPasswordHasherService passwordHasherService,
    IProtectionProviderService protectionProviderService,
    ILogger<UserProfilesManagerService> logger) : IUserProfilesManagerService
{
    public string UsersCantRegisterErrorMessage => "سایت جاری در حال حاضر کاربر جدیدی را نمی‌پذیرد.";

    public async Task ResetRegistrationCodeAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        user.RegistrationCode = passwordHasherService.CreateCryptographicallySecureGuid()
            .ToString(format: "N", CultureInfo.InvariantCulture);

        user.SerialNumber = user.RegistrationCode;
        await uow.SaveChangesAsync();
    }

    public async Task UpdateUserLastActivityDateAsync(int userId)
    {
        var user = await usersInfoService.FindUserAsync(userId);

        if (user is null)
        {
            return;
        }

        if (user.LastVisitDateTime is not null)
        {
            var updateLastActivityDate = TimeSpan.FromMinutes(value: 2);
            var currentUtc = DateTimeOffset.UtcNow;
            var timeElapsed = currentUtc.Subtract(user.LastVisitDateTime.Value.ToDateTimeOffset());

            if (timeElapsed < updateLastActivityDate)
            {
                return;
            }
        }

        user.LastVisitDateTime = DateTime.UtcNow;
        await uow.SaveChangesAsync();
    }

    public async Task<User> AddUserAsync(RegisterModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var user = new User
        {
            UserName = model.Username,
            HashedPassword = passwordHasherService.GetPbkdf2Hash(model.Password1),
            SerialNumber = Guid.NewGuid().ToString(format: "N"),
            EMail = model.EMail.FixGmailDots(),
            FriendlyName = model.FriendlyName.RemoveDiacritics().NormalizeUnderLines().RemovePunctuation(),
            ReceiveDailyEmails = true,
            RegistrationCode = Guid.NewGuid().ToString(format: "N", CultureInfo.InvariantCulture),
            IsActive = true,
            IsRestricted = true
        };

        user = usersInfoService.AddUser(user);
        await uow.SaveChangesAsync();

        await usedPasswordsService.AddToUsedPasswordsListAsync(user.Id, user.HashedPassword);

        return user;
    }

    public async Task<OperationResult> ActivateEmailAsync(string name)
    {
        var data = protectionProviderService.Decrypt(name);

        if (string.IsNullOrWhiteSpace(data))
        {
            return ("اطلاعات وارد شده منقضی شده‌است.", OperationStat.Failed);
        }

        var parts = data.Split(separator: '-');
        var code = new Guid(parts[0]);
        var userId = parts[1].ToInt();
        var user = await usersInfoService.FindUserAsync(userId);

        if (user is null)
        {
            return ("چنین کاربری در بانک اطلاعاتی وجود ندارد", OperationStat.Failed);
        }

        if (!user.IsActive)
        {
            return ("این کاربر غیرفعال شده است", OperationStat.Failed);
        }

        if (user.EmailIsValidated)
        {
            return ("این اکانت پیشتر فعال شده است", OperationStat.Failed);
        }

        if (!string.Equals(user.RegistrationCode, code.ToString(format: "N"), StringComparison.OrdinalIgnoreCase))
        {
            return ("کد فعال سازی معتبر نیست. لطفا جهت دریافت مجدد آن اقدام نمائید.", OperationStat.Failed);
        }

        user.EmailIsValidated = true;
        user.RegistrationCode = Guid.NewGuid().ToString(format: "N", CultureInfo.InvariantCulture);
        user.SerialNumber = user.RegistrationCode;
        await uow.SaveChangesAsync();

        return ($"با تشکر. حساب کاربری شما ({user.FriendlyName}) فعال گردید و اکنون می\u200cتوانید به سایت وارد شوید.",
            OperationStat.Succeeded);
    }

    public async Task<int> GetGeneralAdvertisementUserIdAsync()
    {
        const string generalAdvertisementUser = "آگهی‌های عمومی";

        var user = await usersInfoService.FindUserByFriendlyNameAsync(generalAdvertisementUser);

        if (user is not null)
        {
            return user.Id;
        }

        var password = $"{passwordHasherService.CreateCryptographicallySecureGuid()}";

        user = usersInfoService.AddUser(new User
        {
            UserName = "GeneralAdvertisementUser",
            HashedPassword = passwordHasherService.GetPbkdf2Hash(password),
            EMail = "GeneralAdvertisementUser@site.com",
            FriendlyName = generalAdvertisementUser,
            ReceiveDailyEmails = false,
            RegistrationCode = Guid.NewGuid().ToString(format: "N", CultureInfo.InvariantCulture),
            IsActive = true,
            IsRestricted = false,
            EmailIsValidated = true
        });

        await uow.SaveChangesAsync();

        return user.Id;
    }

    public async Task UpdateUserImageFromGravatarAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        try
        {
            if (!string.IsNullOrWhiteSpace(user.Photo))
            {
                return;
            }

            var emailHash = user.EMail.ToLowerInvariant().Trim().Md5Hash().ToLowerInvariant();
            var imageUrl = $"https://www.gravatar.com/avatar/{emailHash}.jpg?s=100&d=identicon&r=PG";

            var imageData = await baseHttpClient.HttpClient.GetByteArrayAsync(imageUrl);

            if (imageData is null || imageData.Length == 0)
            {
                return;
            }

            var dir = appFoldersService.AvatarsFolderPath;
            var fileName = $"{Guid.NewGuid():N}.jpg";
            var fileNamePath = Path.Combine(dir, fileName);
            await File.WriteAllBytesAsync(fileNamePath, imageData);

            user.Photo = fileName;
            await uow.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            //این مورد چون ضروری و حیاتی نیست، ضرورتی به توقف سایر اعمال ندارد
            logger.LogError(ex.Demystify(), message: "UpdateUserImageFromGravatar failed!");
        }
    }

    public async Task<OperationResult<(string Password, User? User)>> ResetPasswordAsync(string name)
    {
        var data = protectionProviderService.Decrypt(name);

        if (string.IsNullOrWhiteSpace(data))
        {
            return ("اطلاعات وارد شده منقضی شده‌است.", OperationStat.Failed, (string.Empty, null));
        }

        var parts = data.Split(separator: '-');
        var code = new Guid(parts[0]);
        var userId = parts[1].ToInt();
        var user = await usersInfoService.FindUserAsync(userId);

        if (user is null)
        {
            return ("چنین کاربری در بانک اطلاعاتی وجود ندارد", OperationStat.Failed, (string.Empty, null));
        }

        if (!user.IsActive)
        {
            return ("این کاربر غیرفعال شده است", OperationStat.Failed, (string.Empty, null));
        }

        if (!user.EmailIsValidated)
        {
            return ("این ایمیل غبرفعال است", OperationStat.Failed, (string.Empty, null));
        }

        if (!string.Equals(user.RegistrationCode, code.ToString(format: "N"), StringComparison.OrdinalIgnoreCase))
        {
            return ("کد یادآوری معتبر نیست. لطفا جهت دریافت مجدد آن اقدام نمائید.", OperationStat.Failed,
                (string.Empty, null));
        }

        var originalPassword = RandomNumberGenerator.GetInt32(fromInclusive: 500000, toExclusive: 1000000)
            .ToString(CultureInfo.InvariantCulture);

        user.HashedPassword = passwordHasherService.GetPbkdf2Hash(originalPassword);
        user.RegistrationCode = Guid.NewGuid().ToString(format: "N", CultureInfo.InvariantCulture);
        user.SerialNumber = user.RegistrationCode;
        await uow.SaveChangesAsync();

        await usedPasswordsService.AddToUsedPasswordsListAsync(user.Id, user.HashedPassword);

        return ("با تشکر. کلمه عبور شما " + $"({user.FriendlyName})" + " ریست گردید و به آدرس ایمیل شما ارسال شد.",
            OperationStat.Succeeded, (originalPassword, user));
    }

    public async Task<OperationResult> ChangeUserPasswordAsync(int? userId, string newPassword)
    {
        ArgumentNullException.ThrowIfNull(newPassword);

        var user = await usersInfoService.FindUserAsync(userId);

        if (user is null)
        {
            return ("کاربر مدنظر یافت نشد.", OperationStat.Failed);
        }

        user.HashedPassword = passwordHasherService.GetPbkdf2Hash(newPassword);
        user.SerialNumber = Guid.NewGuid().ToString(format: "N");

        if (await usedPasswordsService.IsPreviouslyUsedPasswordAsync(userId, newPassword))
        {
            return ("لطفا از کلمات عبور قدیمی خود در این سایت، استفاده نکنید.", OperationStat.Failed);
        }

        await uow.SaveChangesAsync();

        await usedPasswordsService.AddToUsedPasswordsListAsync(user.Id, user.HashedPassword);

        return ("تغییر کلمه‌ی عبور با موفقیت انجام شد.", OperationStat.Succeeded);
    }

    public async Task SendActivateYourAccountEmailAsync(int userId)
    {
        var user = await usersInfoService.FindUserAsync(userId);

        if (user is null || user.EmailIsValidated)
        {
            return;
        }

        user.RegistrationCode = Guid.NewGuid().ToString(format: "N", CultureInfo.InvariantCulture);
        user.SerialNumber = user.RegistrationCode;
        await uow.SaveChangesAsync();

        await usersManagerEmailsService.SendActivateYourAccountEmailAsync(user);
    }

    public async Task UserIsNotRestrictedAsync(int userId)
    {
        var user = await usersInfoService.FindUserAsync(userId);

        if (user is null)
        {
            return;
        }

        user.IsRestricted = false;
        await uow.SaveChangesAsync();

        await usersManagerEmailsService.UsersManagerSendEmailAsync(user.UserName, user.FriendlyName,
            message: "کاربر غیرمحدود شد");
    }

    public async Task UserIsRestrictedAsync(int userId)
    {
        var user = await usersInfoService.FindUserAsync(userId);

        if (user is null)
        {
            return;
        }

        user.IsRestricted = true;
        await uow.SaveChangesAsync();

        await usersManagerEmailsService.UsersManagerSendEmailAsync(user.UserName, user.FriendlyName,
            message: "کاربر محدود شد");
    }

    public async Task DisableUserAsync(int userId)
    {
        var user = await usersInfoService.FindUserAsync(userId);

        if (user is null)
        {
            return;
        }

        user.IsActive = false;
        await uow.SaveChangesAsync();

        await usersManagerEmailsService.UsersManagerSendEmailAsync(user.UserName, user.FriendlyName,
            message: "کاربر غیرفعال شد");
    }

    public async Task ActivateUserAsync(int userId)
    {
        var user = await usersInfoService.FindUserAsync(userId);

        if (user is null)
        {
            return;
        }

        user.IsActive = true;
        await uow.SaveChangesAsync();

        await usersManagerEmailsService.UsersManagerSendEmailAsync(user.UserName, user.FriendlyName,
            message: "کاربر فعال شد");

        if (!user.EmailIsValidated)
        {
            user.RegistrationCode = Guid.NewGuid().ToString(format: "N", CultureInfo.InvariantCulture);
            user.SerialNumber = user.RegistrationCode;
            await uow.SaveChangesAsync();

            await usersManagerEmailsService.SendActivateYourAccountEmailAsync(user);
        }
    }

    public async Task<OperationResult> RegisterUserAsync(RegisterModel? model,
        bool canUsersRegister,
        int? currentUserId)
    {
        if (model is null)
        {
            return ("اطلاعات وارد شده معتبر نیستند. لطفا مجددا سعی کنید.", OperationStat.Failed);
        }

        if (!canUsersRegister)
        {
            return (UsersCantRegisterErrorMessage, OperationStat.Failed);
        }

        if (string.Equals(model.FriendlyName, model.Username, StringComparison.OrdinalIgnoreCase))
        {
            return ("لطفا نام کاربری و نام مستعار یکسانی را انتخاب نکنید", OperationStat.Failed);
        }

        if (!await usersInfoService.CheckEMailAsync(model.EMail, currentUserId))
        {
            return (
                "ایمیل وارد شده هم اکنون توسط یکی از کاربران مورد استفاده‌است و یا امکان ارسال ایمیل به آن وجود ندارد",
                OperationStat.Failed);
        }

        var (message, stat) = await usersInfoService.CheckFriendlyNameAsync(model.FriendlyName, currentUserId);

        if (stat == OperationStat.Failed)
        {
            return (message, OperationStat.Failed);
        }

        if (!await usersInfoService.CheckUsernameAsync(model.Username, currentUserId))
        {
            return ("نام کاربری وارد شده هم اکنون توسط یکی از کاربران مورد استفاده‌است", OperationStat.Failed);
        }

        var user = await AddUserAsync(model);
        await UpdateUserImageFromGravatarAsync(user);

        await usersManagerEmailsService.UserProfileEditedEmailToAdminAsync(user);

        return (
            " ثبت نام شما با موفقیت انجام شد! " + "ایمیل فعال سازی اکانت شما پس از بررسی مدیریت سایت، ارسال می‌گردد. " +
            "لطفا در طی روزهای آینده، قسمت بالک و اسپم صندوق پست الکترونیکی خود را نیز بررسی نمائید.",
            OperationStat.Succeeded);
    }

    public async Task<OperationResult> ProcessForgottenPasswordAsync(ForgottenPasswordModel? model)
    {
        if (model is null)
        {
            return ("اطلاعات وارد شده معتبر نیستند. لطفا مجددا سعی کنید.", OperationStat.Failed);
        }

        var user = await usersInfoService.FindUserByEMailAsync(model.EMail);

        if (user is null)
        {
            return ("چنین ایمیلی در بانک اطلاعاتی سیستم ثبت نشده است", OperationStat.Failed);
        }

        if (!user.IsActive)
        {
            return ("این کاربر غیرفعال شده‌است", OperationStat.Failed);
        }

        if (!user.EmailIsValidated)
        {
            return (
                "ابتدا نیاز است آدرس ایمیل خود را فعال کنید. بنابراین لطفا اندکی صبر نمائید تا ایمیل آن توسط مدیریت سایت ارسال گردد. همچنین میل باکس خود را نیز بررسی کنید. خصوصا قسمت اسپم آن‌را.",
                OperationStat.Failed);
        }

        await ResetRegistrationCodeAsync(user);
        await usersManagerEmailsService.SendForgottenPasswordConfirmEmailAsync(user);

        return (
            "لطفا تا لحظاتی بعد صندوق پست الکترونیکی خود را بررسی نمائید. اگر تا 5 دقیقه بعد ایمیلی را دریافت نکردید، لطفا قسمت بالک و اسپم صندوق پست الکترونیکی خود را نیز بررسی کنید.",
            OperationStat.Succeeded);
    }

    public async Task<OperationResult> EditUserSocialNetworksAsync(int? editUserId, UserSocialNetworkModel? model)
    {
        if (model is null)
        {
            return ("اطلاعات وارد شده معتبر نیستند. لطفا مجددا سعی کنید.", OperationStat.Failed);
        }

        var currentUser = await currentUserService.GetCurrentImpersonatedUserAsync(editUserId);

        if (currentUser is null)
        {
            return ("لطفا مجددا وارد سیستم شوید.", OperationStat.Failed);
        }

        var userSocialNetwork = await usersInfoService.FindUserSocialNetworkAsync(currentUser.Id);

        if (userSocialNetwork is null)
        {
            return ("اطلاعات این کاربر قابل ویرایش نیست.", OperationStat.Failed);
        }

        mapper.Map(model, userSocialNetwork);
        await uow.SaveChangesAsync();

        await usersManagerEmailsService.UserProfileSendEmailAsync(new UserProfileEmailModel
        {
            Username = currentUser.UserName,
            FriendlyName = currentUser.FriendlyName,
            OriginalPassword = ""
        }, currentUser); // sends to the new address

        await usersManagerEmailsService.UserProfileEditedEmailToAdminAsync(currentUser);

        return OperationStat.Succeeded;
    }

    public async Task<OperationResult> EditUserProfileAsync(UserProfileModel? model, int? editUserId)
    {
        if (model is null)
        {
            return ("اطلاعات وارد شده معتبر نیستند. لطفا مجددا سعی کنید.", OperationStat.Failed);
        }

        var currentUser = await currentUserService.GetCurrentImpersonatedUserAsync(editUserId);

        if (currentUser is null)
        {
            return ("لطفا مجددا وارد سیستم شوید.", OperationStat.Failed);
        }

        if (!string.IsNullOrWhiteSpace(model.HomePageUrl) && !model.HomePageUrl.IsValidUrl())
        {
            return ("لطفا آدرس سایت خود را کامل و به همراه پروتکل آدرس آن مانند اچ تی تی پی وارد نمائید.",
                OperationStat.Failed);
        }

        if (string.Equals(model.FriendlyName, model.UserName, StringComparison.OrdinalIgnoreCase))
        {
            return ("لطفا نام کاربری و نام مستعار یکسانی را انتخاب نکنید.", OperationStat.Failed);
        }

        var user = await usersInfoService.FindUserAsync(currentUser.Id);

        if (user is null)
        {
            return ("اطلاعات این کاربر قابل ویرایش نیست.", OperationStat.Failed);
        }

        await SavePostedPhotoAsync(user, model);

        user.Description = antiXssService.GetSanitizedHtml(model.Description ?? "");

        if (!string.Equals(user.EMail, model.EMail, StringComparison.OrdinalIgnoreCase))
        {
            if (!await usersInfoService.CheckEMailAsync(model.EMail, currentUser.Id))
            {
                return (
                    "ایمیل وارد شده هم اکنون توسط یکی از کاربران مورد استفاده است و یا امکان ارسال ایمیل به آن وجود ندارد",
                    OperationStat.Failed);
            }

            await usersManagerEmailsService.UserProfileSendEmailAsync(new UserProfileEmailModel
            {
                Username = currentUser.UserName,
                FriendlyName = currentUser.FriendlyName,
                OriginalPassword = ""
            }, user); // sends to old one

            user.EMail = model.EMail.FixGmailDots();
            user.EmailIsValidated = false;
        }

        user.HomePageUrl = model.HomePageUrl;
        user.ReceiveDailyEmails = model.ReceiveDailyEmails;

        if (!string.Equals(user.UserName, model.UserName, StringComparison.OrdinalIgnoreCase))
        {
            if (!await usersInfoService.CheckUsernameAsync(model.UserName, currentUser.Id))
            {
                return ("نام کاربری وارد شده هم اکنون توسط یکی از کاربران مورد استفاده است", OperationStat.Failed);
            }

            user.UserName = model.UserName;
        }

        if (!string.Equals(user.FriendlyName, model.FriendlyName, StringComparison.OrdinalIgnoreCase))
        {
            var operationResult = await usersInfoService.CheckFriendlyNameAsync(model.FriendlyName, currentUser.Id);

            if (operationResult.Stat != OperationStat.Succeeded)
            {
                return (operationResult.Message, OperationStat.Failed);
            }

            user.FriendlyName = model.FriendlyName.RemoveDiacritics().NormalizeUnderLines().RemovePunctuation();
        }

        user.Location = model.Location;
        user.IsEmailPublic = model.IsEmailPublic;

        if (!model.IsRestricted)
        {
            user.IsJobsSeeker = model.IsJobsSeeker;
        }

        if (model is { DateOfBirthYear: not null, DateOfBirthMonth: not null, DateOfBirthDay: not null })
        {
            var date = string.Create(CultureInfo.InvariantCulture,
                $"{model.DateOfBirthYear.Value}/{model.DateOfBirthMonth.Value:00}/{model.DateOfBirthDay.Value:00}");

            user.DateOfBirth = date.ToGregorianDateTime();
        }
        else
        {
            user.DateOfBirth = null;
        }

        await uow.SaveChangesAsync();

        await usersManagerEmailsService.UserProfileSendEmailAsync(new UserProfileEmailModel
        {
            Username = currentUser.UserName,
            FriendlyName = currentUser.FriendlyName,
            OriginalPassword = ""
        }, user); // sends to the new address

        await SendActivateYourAccountEmailAgainAsync(user);
        await UpdateUserImageFromGravatarAsync(user);

        await usersManagerEmailsService.UserProfileEditedEmailToAdminAsync(user);

        return OperationStat.Succeeded;
    }

    private async Task SavePostedPhotoAsync(User user, UserProfileModel model)
    {
        if (model.PhotoFiles is { Count: > 0 })
        {
            var photoFile = model.PhotoFiles[index: 0];
            var savePath = appFoldersService.GetFolderPath(FileType.Avatar);

            var (isSaved, savedFilePath) =
                await uploadFileService.SavePostedFileAsync(photoFile, savePath, allowOverwrite: false);

            if (isSaved)
            {
                user.Photo = Path.GetFileName(savedFilePath);
            }
        }
    }

    private async Task SendActivateYourAccountEmailAgainAsync(User user)
    {
        if (user.EmailIsValidated)
        {
            return;
        }

        user.RegistrationCode = Guid.NewGuid().ToString(format: "N", CultureInfo.InvariantCulture);
        user.SerialNumber = user.RegistrationCode;
        await uow.SaveChangesAsync();

        await usersManagerEmailsService.SendActivateYourAccountEmailAsync(user);
    }
}
