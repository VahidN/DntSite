using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.UserFiles.Models;
using DntSite.Web.Features.UserFiles.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserFiles.Services;

public class PagedFilesListService(IAppFoldersService appFoldersService, IAdminsEmailsService adminsEmailsService)
    : IPagedFilesListService
{
    public PagedResultModel<FileModel> GetFilesList(FileType fileType,
        int pageNumber = 0,
        int recordsPerPage = 10,
        string searchPattern = "*.*")
    {
        ArgumentNullException.ThrowIfNull(searchPattern);

        var dir = appFoldersService.GetFolderPath(fileType);

        if (!Directory.Exists(dir))
        {
            return new PagedResultModel<FileModel>
            {
                TotalItems = 0,
                Data = []
            };
        }

        var skipRecords = pageNumber * recordsPerPage;
        var allFilesList = GetFilesByExtensions(new DirectoryInfo(dir), searchPattern.Split(separator: ';')).ToList();

        var pagedFilesList = allFilesList.OrderByDescending(x => x.LastWriteTime)
            .Skip(skipRecords)
            .Take(recordsPerPage)
            .Select(x => new FileModel
            {
                LastWriteTime = x.LastWriteTime,
                Name = x.Name,
                Size = x.Length,
                Icon = GetIcon(x.Name)
            })
            .ToList();

        return new PagedResultModel<FileModel>
        {
            TotalItems = allFilesList.Count,
            Data = pagedFilesList
        };
    }

    public string GetIcon(string name)
    {
        var root = "/images/filestypes/";
        var ext = Path.GetExtension(name).ToLowerInvariant().TrimStart(trimChar: '.');
        var iconName = $"{ext}.gif";
        var iconPath = Path.Combine(appFoldersService.WwwRootPath, path2: "images", path3: "filestypes", iconName);

        return File.Exists(iconPath) ? $"{root}{iconName}" : $"{root}file.gif";
    }

    public async Task DeleteFileAsync(FileType currentFileType, string? fileNameToDelete)
    {
        if (string.IsNullOrWhiteSpace(fileNameToDelete))
        {
            return;
        }

        var dir = appFoldersService.GetFolderPath(currentFileType);
        var filePath = Path.Combine(dir, fileNameToDelete);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            await adminsEmailsService.CommonFileEditedSendEmailAsync(fileNameToDelete, description: "حذف فایل");
        }
    }

    private IEnumerable<FileInfo> GetFilesByExtensions(DirectoryInfo dir, params string[] extensions)
    {
        ArgumentNullException.ThrowIfNull(extensions);

        var fileInfos = dir.EnumerateFiles();

        if (extensions.Length == 1 && string.Equals(extensions[0].Trim(), b: "*.*", StringComparison.Ordinal))
        {
            return fileInfos;
        }

        var pathExtensions = extensions.Select(Path.GetExtension).Distinct().ToArray();

        return fileInfos.Where(f => pathExtensions.Contains(f.Extension, StringComparer.OrdinalIgnoreCase));
    }
}
