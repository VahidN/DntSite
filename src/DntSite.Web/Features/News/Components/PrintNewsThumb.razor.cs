using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.RoutingConstants;

namespace DntSite.Web.Features.News.Components;

public partial class PrintNewsThumb
{
    [Parameter] [EditorRequired] public DailyNewsItem? DailyNewsItem { set; get; }

    [InjectComponentScoped] public IAppFoldersService AppFoldersService { set; get; } = null!;

    [MemberNotNullWhen(returnValue: true, nameof(DailyNewsItem))]
    private bool FileExists => DailyNewsItem is not null && !string.IsNullOrWhiteSpace(DailyNewsItem.PageThumbnail) &&
                               File.Exists(Path.Combine(AppFoldersService.GetFolderPath(FileType.NewsThumb), FileName));

    private string ImageUrl => Invariant($"{NewsRoutingConstants.NewsRedirectBase}/{DailyNewsItem?.Id}");

    private string FileName => Invariant($"news-{DailyNewsItem?.Id}.jpg");
}
