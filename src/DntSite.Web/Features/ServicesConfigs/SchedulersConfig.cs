using DntSite.Web.Features.ScheduledTasks.Services;

namespace DntSite.Web.Features.ServicesConfigs;

public static class SchedulersConfig
{
    public static void AddSchedulers(this IServiceCollection services)
        => services.AddDNTScheduler(options =>
        {
            options.AddScheduledTask<WebReadersListJob>(utcNow =>
            {
                var now = utcNow.AddHours(value: 3.5);

                return now is { Hour: 3, Minute: 30, Second: 1 };
            });

            options.AddScheduledTask<NewsHttpStatusCodeJob>(utcNow =>
            {
                var now = utcNow.AddHours(value: 3.5);

                return now is { DayOfWeek: DayOfWeek.Friday, Hour: 1, Minute: 1, Second: 1 };
            });

            options.AddScheduledTask<NewPersianYearEmailsJob>(utcNow =>
            {
                var now = utcNow.AddHours(value: 3.5);

                return now.IsStartOfNewYear();
            });

            options.AddScheduledTask<ManageBacklogsJob>(utcNow =>
            {
                var now = utcNow.AddHours(value: 3.5);

                return now.Hour % 2 == 0 && now is { Minute: 10, Second: 1 };
            });

            options.AddScheduledTask<HumansTxtJob>(utcNow =>
            {
                var now = utcNow.AddHours(value: 3.5);

                return now is { Hour: 3, Minute: 1, Second: 1 };
            });

            options.AddScheduledTask<DraftsJob>(utcNow =>
            {
                var now = utcNow.AddHours(value: 3.5);

                return now.Minute % 5 == 0 && now.Second == 1;
            });

            options.AddScheduledTask<DeleteOrphans>(utcNow =>
            {
                var now = utcNow.AddHours(value: 3.5);

                return now is { Hour: 3, Minute: 7, Second: 1 };
            });

            options.AddScheduledTask<DailyNewsletterJob>(utcNow =>
            {
                var now = utcNow.AddHours(value: 3.5);

                return now is { Hour: 0, Minute: 1, Second: 1 };
            });

            options.AddScheduledTask<DailyBirthDatesEmailJob>(utcNow =>
            {
                var now = utcNow.AddHours(value: 3.5);

                return now is { Hour: 8, Minute: 59, Second: 1 };
            });

            options.AddScheduledTask<EmptyPMsJob>(utcNow =>
            {
                var now = utcNow.AddHours(value: 3.5);

                return now is { DayOfWeek: DayOfWeek.Friday, Hour: 3, Minute: 1, Second: 1 };
            });
        });
}
