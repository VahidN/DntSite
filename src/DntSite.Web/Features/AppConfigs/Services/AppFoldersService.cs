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
    private string? _articleImagesPath;
    private string? _avatarsPath;

    private string? _customFontWithPersianDigitsPath;

    private string? _databaseFolderPath;
    private string? _defaultConnectionString;

    private string? _exportsAssetsFolder;

    private string? _exportsPath;
    private string? _luceneIndexFolderPath;
    private StartupSettingsModel _siteSettings;
    private string? _thumbnailsServicePath;
    private string? _wwwRootPath;

    public AppFoldersService(IWebHostEnvironment webHostEnvironment, IOptionsMonitor<StartupSettingsModel> siteSettings)
    {
        ArgumentNullException.ThrowIfNull(siteSettings);

        _webHostEnvironment = webHostEnvironment;
        _siteSettings = siteSettings.CurrentValue;

        _disposableSettings = siteSettings.OnChange(settings => _siteSettings = settings);
    }

    public string DefaultConnectionString => _defaultConnectionString ??= GetDefaultConnectionString();

    public string DatabaseFolderPath => _databaseFolderPath ??= GetWebRootAppDataFolderPath("Database");

    public string WwwRootPath => _wwwRootPath ??= GetWwwRootPath();

    public string ExportsPath => _exportsPath ??= GetWebRootAppDataFolderPath("exports");

    public string ExportsAssetsFolder => _exportsAssetsFolder ??= ExportsPath.SafePathCombine("assets");

    public string AvatarsFolderPath => _avatarsPath ??= GetWebRootAppDataFolderPath(UploadsFolder, "Avatars");

    public string ArticleImagesFolderPath
        => _articleImagesPath ??= GetWebRootAppDataFolderPath(UploadsFolder, "ArticleImages");

    public string ThumbnailsServiceFolderPath
        => _thumbnailsServicePath ??= GetWebRootAppDataFolderPath(UploadsFolder, "ThumbnailsService");

    public string CustomFontWithPersianDigitsPath => _customFontWithPersianDigitsPath ??=
        WwwRootPath.SafePathCombine("fonts", "Samim-FD.ttf");

    public string LuceneIndexFolderPath => _luceneIndexFolderPath ??= GetWebRootAppDataFolderPath("LuceneIndex");

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
