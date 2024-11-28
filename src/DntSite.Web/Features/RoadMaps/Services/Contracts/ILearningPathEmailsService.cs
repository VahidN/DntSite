using DntSite.Web.Features.RoadMaps.Entities;

namespace DntSite.Web.Features.RoadMaps.Services.Contracts;

public interface ILearningPathEmailsService : IScopedService
{
    public Task NewLearningPathSendEmailToAdminsAsync(LearningPath data);
}
