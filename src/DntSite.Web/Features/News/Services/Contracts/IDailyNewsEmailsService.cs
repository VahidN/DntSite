using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsEmailsService : IScopedService
{
    public Task DailyNewsItemsSendEmailAsync(DailyNewsItem result, string friendlyName);

    public Task ConvertedDailyNewsItemsSendEmailAsync(int id, string title, int toInt);

    public Task PostNewsReplySendEmailToAdminsAsync(DailyNewsItemComment data);

    public Task PostNewsReplySendEmailToWritersAsync(DailyNewsItemComment comment);

    public Task PostNewsReplySendEmailToPersonAsync(DailyNewsItemComment comment);
}
