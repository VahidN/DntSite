using DntSite.Web.Features.RoadMaps.Entities;

namespace DntSite.Web.Features.RoadMaps.Services.Contracts;

public interface ILearningPathEmailsService : IScopedService
{
    Task NewLearningPathSendEmailToAdminsAsync(LearningPath data);
}
