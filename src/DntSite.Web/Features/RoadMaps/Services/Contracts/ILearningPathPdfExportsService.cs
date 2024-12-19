namespace DntSite.Web.Features.RoadMaps.Services.Contracts;

public interface ILearningPathPdfExportsService : IScopedService
{
    public Task CreateMergedPdfOfLearningPathsAsync();
}
