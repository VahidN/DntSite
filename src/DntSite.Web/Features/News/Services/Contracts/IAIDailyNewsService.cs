namespace DntSite.Web.Features.News.Services.Contracts;

public interface IAIDailyNewsService : IScopedService
{
    Task StartProcessingNewsFeedsAsync(CancellationToken ct = default);
}
