namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsletter : IScopedService
{
    Task<string> GetEmailContentAsync(DateTime fromDateTime);
}
