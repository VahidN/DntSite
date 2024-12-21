using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using Microsoft.Extensions.Options;

namespace DntSite.Web.Features.AppConfigs.Services;

public class AppFoldersService : IAppFoldersService
{
    private const string AppDataFolder = "App_Data";
    private const string WwwRoot = "wwwroot";
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

        siteSettings.OnChange(settings => _siteSettings = settings);
    }

    public string DefaultConnectionString => _defaultConnectionString ??= GetDefaultConnectionString();

    public string DatabaseFolderPath => _databaseFolderPath ??= GetWebRootAppDataFolderPath("Database");

    public string WwwRootPath => _wwwRootPath ??= GetWwwRootPath();

    public string ExportsPath => _exportsPath ??= GetWebRootAppDataFolderPath("exports");

    public string ExportsAssetsFolder => _exportsAssetsFolder ??= Path.Combine(ExportsPath, path2: "assets");

    public string AvatarsFolderPath => _avatarsPath ??= GetWebRootAppDataFolderPath("Uploads", "Avatars");

    public string ArticleImagesFolderPath
        => _articleImagesPath ??= GetWebRootAppDataFolderPath("Uploads", "ArticleImages");

    public string ThumbnailsServiceFolderPath
        => _thumbnailsServicePath ??= GetWebRootAppDataFolderPath("Uploads", "ThumbnailsService");

    public string CustomFontWithPersianDigitsPath => _customFontWithPersianDigitsPath ??=
        Path.Combine(WwwRootPath, path2: "fonts", path3: "Samim-FD.ttf");

    public string LuceneIndexFolderPath => _luceneIndexFolderPath ??= GetWebRootAppDataFolderPath("LuceneIndex");

    public string GetFolderPath(FileType fileType)
        => fileType switch
        {
            FileType.Avatar => GetWebRootAppDataFolderPath("Uploads", "Avatars"),
            FileType.Image => GetWebRootAppDataFolderPath("Uploads", "ArticleImages"),
            FileType.UserFile => GetWebRootAppDataFolderPath("Uploads", "ArticleFiles"),
            FileType.SiteUpdate => GetWebRootAppDataFolderPath("Uploads", "Updates"),
            FileType.FilesRoot => GetWebRootAppDataFolderPath("Uploads"),
            FileType.Messages => GetWebRootAppDataFolderPath("Uploads", "Messages"),
            FileType.CommonFiles => GetWebRootAppDataFolderPath("Uploads", "CommonFiles"),
            FileType.MessagesImages => GetWebRootAppDataFolderPath("Uploads", "MessagesImages"),
            FileType.ForWriters => GetWebRootAppDataFolderPath("Uploads", "ForWriters"),
            FileType.ProjectFiles => GetWebRootAppDataFolderPath("Uploads", "ProjectFiles"),
            FileType.NewsThumb => GetWebRootAppDataFolderPath("Uploads", "ThumbnailsService"),
            FileType.CourseFile => GetWebRootAppDataFolderPath("Uploads", "CourseFiles"),
            FileType.CourseImage => GetWebRootAppDataFolderPath("Uploads", "CourseImages"),
            FileType.Backup => GetWebRootAppDataFolderPath("Uploads", "Backup"),
            _ => GetWebRootAppDataFolderPath("Uploads", "ArticleFiles")
        };

    public string GetWebRootAppDataFolderPath(params string[] folders)
    {
        ArgumentNullException.ThrowIfNull(folders);

        var path = Path.Combine(WwwRootPath, AppDataFolder);

        foreach (var folder in folders)
        {
            path = Path.Combine(path, folder);
        }

        path.CheckDirExists();

        return path;
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

        return Path.Combine(contentRootPath, WwwRoot);
    }

    private string GetDefaultConnectionString()
    {
        var defaultConnection = _siteSettings.ConnectionStrings.DefaultConnection;

        return defaultConnection.Replace(oldValue: "|DataDirectory|", DatabaseFolderPath,
            StringComparison.OrdinalIgnoreCase);
    }
}
