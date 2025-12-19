namespace DntSite.Web.Features.RoadMaps.Services.Contracts;

public interface ILearningPathPdfExportsService : IScopedService
{
    Task CreateMergedPdfOfLearningPathsAsync(CancellationToken cancellationToken);
}
