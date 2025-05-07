namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsletter : IScopedService
{
    public Task<string> GetEmailContentAsync(DateTime fromDateTime, bool showBriefDescription);
}
