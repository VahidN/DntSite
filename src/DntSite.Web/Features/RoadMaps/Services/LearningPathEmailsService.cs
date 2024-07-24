using DntSite.Web.Features.Common.Models;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.RoadMaps.EmailLayouts;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.RoadMaps.Models;
using DntSite.Web.Features.RoadMaps.Services.Contracts;

namespace DntSite.Web.Features.RoadMaps.Services;

public class LearningPathEmailsService(IEmailsFactoryService emailsFactoryService) : ILearningPathEmailsService
{
    public Task NewLearningPathSendEmailToAdminsAsync(LearningPath data)
    {
        ArgumentNullException.ThrowIfNull(data);

        return emailsFactoryService.SendEmailToAllAdminsAsync<LearningPathToAdminEmail, LearningPathToAdminEmailModel>(
            messageId: "NewLearningPath", inReplyTo: "", references: "NewLearningPath",
            new LearningPathToAdminEmailModel
            {
                Id = data.Id.ToString(CultureInfo.InvariantCulture),
                Title = data.Title,
                Description = data.Description,
                Author = data.User?.FriendlyName ?? SharedConstants.GuestUserName,
                Stat = "عمومی"
            }, $"نقشه راه : {data.Title}");
    }
}
