using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.UserFiles.Models;

namespace DntSite.Web.Features.UserFiles.Services.Contracts;

public interface IPagedFilesListService : IScopedService
{
    PagedResultModel<FileModel> GetFilesList(FileType fileType,
        int pageNumber = 0,
        int recordsPerPage = 10,
        string searchPattern = "*.*");

    string GetIcon(string name);

    Task DeleteFileAsync(FileType currentFileType, string? fileNameToDelete);
}
