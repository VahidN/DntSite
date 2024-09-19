using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.Common.RoutingConstants;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.UserFiles.Models;
using DntSite.Web.Features.UserFiles.RoutingConstants;
using DntSite.Web.Features.UserFiles.Services.Contracts;

namespace DntSite.Web.Features.UserFiles.Components;

[Authorize]
public partial class FilesManager
{
    private const int ItemsPerPage = 12;

    private PagedResultModel<FileModel>? _files;

    private FileType CurrentFileType => IsUsersFilesList ? FileType.UserFile : FileType.Image;

    private bool IsCurrentUserAdmin => ApplicationState.CurrentUser?.IsAdmin == true;

    private bool IsUsersFilesList => string.Equals(Type, b: "users-files", StringComparison.OrdinalIgnoreCase);

    private string Title => IsUsersFilesList ? "فایل‌های ارسالی" : "تصاویر ارسالی";

    private string BasePath => IsUsersFilesList
        ? UserFilesRoutingConstants.FilesManagerFilesUrl
        : UserFilesRoutingConstants.FilesManagerImagesUrl;

    [Parameter] public string? Type { set; get; }

    [Parameter] public int? CurrentPage { set; get; }

    [InjectComponentScoped] internal IPagedFilesListService PagedFilesListService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [SupplyParameterFromForm] public string? FileNameToDelete { set; get; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        ShowResults();
        AddBreadCrumbs();
    }

    private void ShowResults()
    {
        CurrentPage ??= 1;

        _files = PagedFilesListService.GetFilesList(CurrentFileType, CurrentPage.Value - 1, ItemsPerPage);
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..UserFilesBreadCrumbs.DefaultBreadCrumbs]);

    private string GetFileUrl(FileModel record)
        => $"{ApiUrlsRoutingConstants.File.HttpAny.UserFile}?name={Uri.EscapeDataString(record.Name)}";

    private string GetImageUrl(FileModel record)
        => $"{ApiUrlsRoutingConstants.File.HttpAny.Image}?name={Uri.EscapeDataString(record.Name)}";

    private string GetFormName(FileModel record)
        => string.Create(CultureInfo.InvariantCulture, $"File_{record.Name.GetHashCode(StringComparison.Ordinal)}");

    private async Task OnDeleteFileAsync()
    {
        if (!IsCurrentUserAdmin)
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        await PagedFilesListService.DeleteFileAsync(CurrentFileType, FileNameToDelete);
        ShowResults();
    }
}
