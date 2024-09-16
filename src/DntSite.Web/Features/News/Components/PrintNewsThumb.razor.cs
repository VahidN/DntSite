using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.News.RoutingConstants;

namespace DntSite.Web.Features.News.Components;

public partial class PrintNewsThumb
{
    [Parameter] [EditorRequired] public int Id { set; get; }

    [Parameter] [EditorRequired] public required string Title { set; get; }

    [InjectComponentScoped] public IAppFoldersService AppFoldersService { set; get; } = null!;

    private bool FileExists => File.Exists(Path.Combine(AppFoldersService.ThumbnailsServiceFolderPath, FileName));

    private string ImageUrl => Invariant($"{NewsRoutingConstants.NewsRedirectBase}/{Id}");

    private string FileName => Invariant($"news-{Id}.jpg");
}
