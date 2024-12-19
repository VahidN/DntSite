using DntSite.Web.Features.AppConfigs.Models;

namespace DntSite.Web.Features.AppConfigs.Services.Contracts;

public interface IAppFoldersService : ISingletonService
{
    public string DefaultConnectionString { get; }

    public string DatabaseFolderPath { get; }

    public string WwwRootPath { get; }

    public string ExportsPath { get; }

    public string ExportsAssetsFolder { get; }

    public string AvatarsFolderPath { get; }

    public string ArticleImagesFolderPath { get; }

    public string ThumbnailsServiceFolderPath { get; }

    public string CustomFontWithPersianDigitsPath { get; }

    public string LuceneIndexFolderPath { get; }

    public string GetFolderPath(FileType fileType);

    public string GetWebRootAppDataFolderPath(params string[] folders);
}
