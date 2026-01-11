using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.News.Services.Contracts;

namespace DntSite.Web.Features.News.ScheduledTasks;

/// <summary>
///     This job uses Google-Chrome to take screenshots of the given URLs.
///     You can install this browser on Windows systems very easily.
///     On linux systems use these commands:
///     https://gist.github.com/Austcool-Walker/3f898ff04e5e0a9644ab487984e5eeec
/// </summary>
/*
wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | sudo apt-key add -
sudo sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list'
sudo apt-get update
sudo apt-get install google-chrome-stable

google-chrome --version
 */
public class ThumbnailsServiceJob(
    IDailyNewsScreenshotsService dailyNewsScreenshots,
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    ILogger<ThumbnailsServiceJob> logger) : IScheduledTask
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        if (!(await cachedAppSettingsProvider.GetAppSettingsAsync()).ShouldCreateNewsScreenshots)
        {
            return;
        }

        if (!NetworkExtensions.IsConnectedToInternet(TimeSpan.FromSeconds(seconds: 2)))
        {
            logger.LogWarning(message: "There is no internet connection to run DownloadScreenshotsAsync().");

            return;
        }

        var numberOfDownloadedFiles = await dailyNewsScreenshots.DownloadScreenshotsAsync(count: 10, cancellationToken);

        if (numberOfDownloadedFiles > 0)
        {
            await dailyNewsScreenshots.UpdateAllNewsPageThumbnailsAsync();
        }
    }
}
