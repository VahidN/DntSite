namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsletter : IScopedService
{
    Task<string> GetEmailContentAsync(string url, DateTime yesterday);
}
