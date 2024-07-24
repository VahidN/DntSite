using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsEmailsService : IScopedService
{
    Task DailyNewsItemsSendEmailAsync(DailyNewsItem result, string friendlyName);

    Task ConvertedDailyNewsItemsSendEmailAsync(int id, string title, int toInt);

    Task PostNewsReplySendEmailToAdminsAsync(DailyNewsItemComment data);

    Task PostNewsReplySendEmailToWritersAsync(DailyNewsItemComment comment);

    Task PostNewsReplySendEmailToPersonAsync(DailyNewsItemComment comment);
}
