using DntSite.Web.Features.AppConfigs.Models;

namespace DntSite.Web.Features.AppConfigs.Services.Contracts;

public interface IAppFoldersService : ISingletonService
{
    string DefaultConnectionString { get; }

    string DatabaseFolderPath { get; }

    string WwwRootPath { get; }

    string AvatarsFolderPath { get; }

    string ArticleImagesFolderPath { get; }

    string ThumbnailsServiceFolderPath { get; }

    string CustomFontWithPersianDigitsPath { get; }

    string GetFolderPath(FileType fileType);

    string GetWebRootAppDataFolderPath(params string[] folders);
}
