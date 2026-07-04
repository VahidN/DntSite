using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using Microsoft.Extensions.Options;

namespace DntSite.Web.Features.AppConfigs.Services;

public sealed class AppFoldersService : IAppFoldersService
{
    public const string AppDataFolder = "App_Data";
    public const string WwwRoot = "wwwroot";
    public const string UploadsFolder = "Uploads";
    private readonly IDisposable? _disposableSettings;

    private readonly IWebHostEnvironment _webHostEnvironment;

    private StartupSettingsModel _siteSettings;

    public AppFoldersService(IWebHostEnvironment webHostEnvironment, IOptionsMonitor<StartupSettingsModel> siteSettings)
    {
        ArgumentNullException.ThrowIfNull(siteSettings);

        _webHostEnvironment = webHostEnvironment;
        _siteSettings = siteSettings.CurrentValue;

        _disposableSettings = siteSettings.OnChange(settings => _siteSettings = settings);
    }

    public string DefaultConnectionString => field ??= GetDefaultConnectionString();

    public string DatabaseFolderPath => field ??= GetWebRootAppDataFolderPath("Database");

    public string WwwRootPath => field ??= GetWwwRootPath();

    public string ExportsPath => field ??= GetWebRootAppDataFolderPath("exports");

    public string ExportsAssetsFolder => field ??= ExportsPath.SafePathCombine("assets");

    public string ExportsEpubDocsFolder => field ??= ExportsPath.SafePathCombine("EpubDocs");

    public string AvatarsFolderPath => field ??= GetWebRootAppDataFolderPath(UploadsFolder, "Avatars");

    public string UploadsFolderPath => field ??= GetWebRootAppDataFolderPath(UploadsFolder);

    public string BackupFolderPath => field ??= GetWebRootAppDataFolderPath("Backup");

    public string ArticleImagesFolderPath => field ??= GetWebRootAppDataFolderPath(UploadsFolder, "ArticleImages");

    public string ThumbnailsServiceFolderPath
        => field ??= GetWebRootAppDataFolderPath(UploadsFolder, "ThumbnailsService");

    public string CustomFontWithPersianDigitsPath => field ??= WwwRootPath.SafePathCombine("fonts", "Samim-FD.ttf");

    public string FontsFolderPath => field ??= WwwRootPath.SafePathCombine("fonts");

    public string LuceneIndexFolderPath => field ??= GetWebRootAppDataFolderPath("LuceneIndex");

    public string GetFolderPath(FileType fileType)
        => fileType switch
        {
            FileType.Avatar => GetWebRootAppDataFolderPath(UploadsFolder, "Avatars"),
            FileType.Image => GetWebRootAppDataFolderPath(UploadsFolder, "ArticleImages"),
            FileType.UserFile => GetWebRootAppDataFolderPath(UploadsFolder, "ArticleFiles"),
            FileType.SiteUpdate => GetWebRootAppDataFolderPath(UploadsFolder, "Updates"),
            FileType.FilesRoot => GetWebRootAppDataFolderPath(UploadsFolder),
            FileType.Messages => GetWebRootAppDataFolderPath(UploadsFolder, "Messages"),
            FileType.CommonFiles => GetWebRootAppDataFolderPath(UploadsFolder, "CommonFiles"),
            FileType.MessagesImages => GetWebRootAppDataFolderPath(UploadsFolder, "MessagesImages"),
            FileType.ForWriters => GetWebRootAppDataFolderPath(UploadsFolder, "ForWriters"),
            FileType.ProjectFiles => GetWebRootAppDataFolderPath(UploadsFolder, "ProjectFiles"),
            FileType.NewsThumb => GetWebRootAppDataFolderPath(UploadsFolder, "ThumbnailsService"),
            FileType.CourseFile => GetWebRootAppDataFolderPath(UploadsFolder, "CourseFiles"),
            FileType.CourseImage => GetWebRootAppDataFolderPath(UploadsFolder, "CourseImages"),
            FileType.Backup => GetWebRootAppDataFolderPath(UploadsFolder, "Backup"),
            _ => GetWebRootAppDataFolderPath(UploadsFolder, "ArticleFiles")
        };

    public string GetWebRootAppDataFolderPath(params string[] folders)
    {
        ArgumentNullException.ThrowIfNull(folders);

        var path = WwwRootPath.SafePathCombine(AppDataFolder);

        foreach (var folder in folders)
        {
            path = path.SafePathCombine(folder);
        }

        path.CheckDirExists();

        return path;
    }

    public void Dispose() => _disposableSettings?.Dispose();

    public string GetTempDirectory()
    {
        var tempDirectory = BackupFolderPath.SafePathCombine("Temp");
        tempDirectory.TryDeleteDirectory();
        tempDirectory.TryCreateDirectory();

        return tempDirectory;
    }

    private string GetWwwRootPath()
    {
        var webRootPath = _webHostEnvironment.WebRootPath;

        if (webRootPath.TrimEnd(Path.DirectorySeparatorChar).EndsWith(WwwRoot, StringComparison.OrdinalIgnoreCase))
        {
            return webRootPath;
        }

        var contentRootPath = webRootPath.Split([$"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}"],
            StringSplitOptions.RemoveEmptyEntries)[0];

        return contentRootPath.SafePathCombine(WwwRoot);
    }

    private string GetDefaultConnectionString()
    {
        var defaultConnection = _siteSettings.ConnectionStrings.DefaultConnection;

        return defaultConnection.Replace(oldValue: "|DataDirectory|", DatabaseFolderPath,
            StringComparison.OrdinalIgnoreCase);
    }
}
