using DntSite.Web.Features.ScheduledTasks.Services;

namespace DntSite.Web.Features.ServicesConfigs;

public static class SchedulersConfig
{
    public static void AddSchedulers(this IServiceCollection services)
        => services.AddDNTScheduler(options =>
        {
            options.AddScheduledTask<WebReadersListJob>(utcNow
                => GetNowIranTime(utcNow) is { Hour: 3, Minute: 30, Second: 1 });

            options.AddScheduledTask<NewsHttpStatusCodeJob>(utcNow
                => GetNowIranTime(utcNow) is { DayOfWeek: DayOfWeek.Friday, Hour: 1, Minute: 1, Second: 1 });

            options.AddScheduledTask<SendActivationEmailsJob>(utcNow
                => GetNowIranTime(utcNow) is { DayOfWeek: DayOfWeek.Friday, Hour: 7, Minute: 1, Second: 1 });

            options.AddScheduledTask<NewPersianYearEmailsJob>(utcNow => GetNowIranTime(utcNow).IsStartOfNewYear());

            options.AddScheduledTask<ManageBacklogsJob>(utcNow =>
            {
                var now = GetNowIranTime(utcNow);

                return now.Hour % 2 == 0 && now is { Minute: 10, Second: 1 };
            });

            options.AddScheduledTask<HumansTxtJob>(
                utcNow => GetNowIranTime(utcNow) is { Hour: 3, Minute: 1, Second: 1 });

            options.AddScheduledTask<DraftsJob>(utcNow =>
            {
                var now = GetNowIranTime(utcNow);

                return now.Minute % 5 == 0 && now.Second == 1;
            });

            options.AddScheduledTask<DeleteOrphans>(utcNow
                => GetNowIranTime(utcNow) is { Hour: 3, Minute: 7, Second: 1 });

            options.AddScheduledTask<DailyNewsletterJob>(utcNow
                => GetNowIranTime(utcNow) is { Hour: 0, Minute: 1, Second: 1 });

            options.AddScheduledTask<DailyBirthDatesEmailJob>(utcNow
                => GetNowIranTime(utcNow) is { Hour: 8, Minute: 59, Second: 1 });

            options.AddScheduledTask<EmptyPMsJob>(utcNow
                => GetNowIranTime(utcNow) is { Hour: 3, Minute: 1, Second: 1 });
        });

    private static DateTime GetNowIranTime(DateTime utcNow) => utcNow.AddHours(value: 3.5);
}
