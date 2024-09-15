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
public class ThumbnailsServiceJob(IDailyNewsScreenshotsService dailyNewsScreenshots) : IScheduledTask
{
    public async Task RunAsync()
    {
        if (IsShuttingDown)
        {
            return;
        }

        if (!NetworkExtensions.IsConnectedToInternet())
        {
            return;
        }

        await dailyNewsScreenshots.DownloadScreenshotsAsync(count: 10);
        await dailyNewsScreenshots.UpdateAllNewsPageThumbnailsAsync();
    }

    public bool IsShuttingDown { get; set; }
}
