using DntSite.Web.Features.AppConfigs.ScheduledTasks;
using DntSite.Web.Features.Backlogs.ScheduledTasks;
using DntSite.Web.Features.Exports.ScheduledTasks;
using DntSite.Web.Features.News.ScheduledTasks;
using DntSite.Web.Features.Posts.ScheduledTasks;
using DntSite.Web.Features.PrivateMessages.ScheduledTasks;
using DntSite.Web.Features.Searches.ScheduledTasks;
using DntSite.Web.Features.Seo.ScheduledTasks;
using DntSite.Web.Features.Stats.ScheduledTasks;
using DntSite.Web.Features.UserProfiles.ScheduledTasks;

namespace DntSite.Web.Features.ServicesConfigs;

public static class SchedulersConfig
{
    public static void AddSchedulers(this IServiceCollection services)
        => services.AddDNTScheduler(options =>
        {
            options.AddScheduledTask<DotNetVersionCheckJob>(utcNow
                => GetNowIranTime(utcNow) is { Hour: 5, Minute: 30, Second: 1 });

            options.AddScheduledTask<CheckAdminsLastVisitJob>(utcNow =>
            {
                var now = GetNowIranTime(utcNow);

                return now.Minute % 5 == 0 && now.Second == 1;
            });

            options.AddScheduledTask<FreeSpaceCheckJob>(utcNow =>
            {
                var now = GetNowIranTime(utcNow);

                return now.Hour % 6 == 0 && now is { Minute: 10, Second: 1 };
            });

            options.AddScheduledTask<WebReadersListJob>(utcNow
                => GetNowIranTime(utcNow) is { Hour: 3, Minute: 30, Second: 1 });

            options.AddScheduledTask<UpdatePublicNewsHttpStatusCodeJob>(utcNow
                => GetNowIranTime(utcNow) is { Day: 1, Hour: 1, Minute: 1, Second: 1 });

            options.AddScheduledTask<UpdateDeletedNewsHttpStatusCodeJob>(utcNow
                => GetNowIranTime(utcNow) is { Day: 2, Hour: 1, Minute: 1, Second: 1 });

            options.AddScheduledTask<SendActivationEmailsJob>(utcNow
                => GetNowIranTime(utcNow) is { Hour: 11, Minute: 1, Second: 1 });

            options.AddScheduledTask<DisableInactiveUsersJob>(utcNow
                => GetNowIranTime(utcNow) is { Hour: 6, Minute: 1, Second: 1 });

            options.AddScheduledTask<NewPersianYearEmailsJob>(utcNow => GetNowIranTime(utcNow).IsStartOfNewYear());

            options.AddScheduledTask<ManageBacklogsJob>(utcNow =>
            {
                var now = GetNowIranTime(utcNow);

                return now.Hour % 2 == 0 && now is { Minute: 10, Second: 1 };
            });

            options.AddScheduledTask<HumansTxtJob>(utcNow
                => GetNowIranTime(utcNow) is { Hour: 3, Minute: 1, Second: 1 });

            options.AddScheduledTask<DraftsJob>(utcNow =>
            {
                var now = GetNowIranTime(utcNow);

                return now.Minute % 5 == 0 && now.Second == 1;
            });

            options.AddScheduledTask<FullTextSearchWriterJob>(utcNow =>
            {
                var now = GetNowIranTime(utcNow);

                return now.Minute % 5 == 0 && now.Second == 1;
            });

            options.AddScheduledTask<ThumbnailsServiceJob>(utcNow =>
            {
                var now = GetNowIranTime(utcNow);

                return now.Minute % 10 == 0 && now.Second == 1;
            });

            options.AddScheduledTask<ExportToSeparatePdfFilesJob>(utcNow =>
            {
                var now = GetNowIranTime(utcNow);

                return now.Minute % 20 == 0 && now.Second == 1;
            });

            options.AddScheduledTask<ExportToMergedPdfFilesJob>(utcNow
                => GetNowIranTime(utcNow) is { Hour: 5, Minute: 30, Second: 1 });

            options.AddScheduledTask<DeleteOrphansJob>(utcNow
                => GetNowIranTime(utcNow) is { Hour: 3, Minute: 7, Second: 1 });

            options.AddScheduledTask<DailyNewsletterJob>(utcNow
                => GetNowIranTime(utcNow) is { Hour: 0, Minute: 1, Second: 1 });

            options.AddScheduledTask<DailyBirthDatesEmailJob>(utcNow
                => GetNowIranTime(utcNow) is { Hour: 8, Minute: 59, Second: 1 });

            options.AddScheduledTask<EmptyPMsJob>(utcNow
                => GetNowIranTime(utcNow) is { Hour: 3, Minute: 1, Second: 1 });

            options.AddScheduledTask<AIDailyNewsJob>(utcNow =>
            {
                var now = GetNowIranTime(utcNow);

                return now.Minute % 15 == 0 && now.Second == 1;
            });

            options.AddScheduledTask<AIDailyNewsBacklogsJob>(utcNow =>
            {
                var now = GetNowIranTime(utcNow);

                return now.Hour % 2 == 0 && now is { Minute: 5, Second: 1 };
            });
        });

    private static DateTime GetNowIranTime(DateTime utcNow) => utcNow.ToIranTimeZoneDateTime();
}
