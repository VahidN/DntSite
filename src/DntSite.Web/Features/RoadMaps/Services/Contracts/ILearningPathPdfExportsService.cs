using DntSite.Web.Features.Exports.Models;

namespace DntSite.Web.Features.RoadMaps.Services.Contracts;

public interface ILearningPathPdfExportsService : IScopedService
{
    Task CreateMergedPdfOfLearningPathsAsync(ExportType exportType, CancellationToken cancellationToken);

    IList<int> GetNewsIds(IList<string> links);

    IList<int> GetPostIds(IList<string> links);

    Task<IList<Guid>> GetCourseTopicIdsAsync(IList<string> links);
}
