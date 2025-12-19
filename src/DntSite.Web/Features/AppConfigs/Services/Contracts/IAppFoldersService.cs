using DntSite.Web.Features.AppConfigs.Models;

namespace DntSite.Web.Features.AppConfigs.Services.Contracts;

public interface IAppFoldersService : ISingletonService, IDisposable
{
    string DefaultConnectionString { get; }

    string DatabaseFolderPath { get; }

    string WwwRootPath { get; }

    string ExportsPath { get; }

    string ExportsAssetsFolder { get; }

    string AvatarsFolderPath { get; }

    string ArticleImagesFolderPath { get; }

    string ThumbnailsServiceFolderPath { get; }

    string CustomFontWithPersianDigitsPath { get; }

    string LuceneIndexFolderPath { get; }

    string GetFolderPath(FileType fileType);

    string GetWebRootAppDataFolderPath(params string[] folders);
}
