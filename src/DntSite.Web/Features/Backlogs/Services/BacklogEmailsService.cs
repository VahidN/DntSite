using DntSite.Web.Features.Backlogs.EmailLayouts;
using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Backlogs.Models;
using DntSite.Web.Features.Backlogs.Services.Contracts;
using DntSite.Web.Features.Common.Services.Contracts;

namespace DntSite.Web.Features.Backlogs.Services;

public class BacklogEmailsService(ICommonService commonService, IEmailsFactoryService emailsFactoryService)
    : IBacklogEmailsService
{
    public async Task NewBacklogSendEmailToAdminsAsync(Backlog data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var name = data.User is null || string.IsNullOrWhiteSpace(data.User.FriendlyName)
            ? (await commonService.FindUserAsync(data.UserId))?.FriendlyName
            : data.User.FriendlyName;

        await emailsFactoryService.SendEmailToAllAdminsAsync<BacklogToAdminEmail, BacklogToAdminEmailModel>(
            "NewBacklog", "", "NewBacklog", new BacklogToAdminEmailModel
            {
                Id = data.Id.ToString(CultureInfo.InvariantCulture),
                Title = data.Title,
                Description = data.Description,
                Author = name ?? "Guest"
            }, $"پیشنهاد جدید : {data.Title}");
    }
}
