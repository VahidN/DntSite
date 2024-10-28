using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Common.Components;

public partial class DntSitePageTitle
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] [EditorRequired] public required string PageTitle { set; get; }

    [Parameter] [EditorRequired] public required string Group { set; get; }

    [Parameter] [EditorRequired] public int? CurrentPage { set; get; }

    [Inject] internal IBackgroundQueueService BackgroundQueueService { set; get; } = null!;

    [InjectComponentScoped] internal ISiteUrlsService SiteUrlsService { set; get; } = null!;

    protected override Task OnInitializedAsync() => AddToSiteUrlsBackgroundQueueAsync();

    private string GetTitle()
    {
        var page = "";

        if (CurrentPage is > 0)
        {
            page = string.Create(CultureInfo.InvariantCulture, $"، صفحه {CurrentPage.Value}");
        }

        return $"{ApplicationState.AppSetting?.BlogName} | {Group}: {PageTitle}{page}".ToPersianNumbers();
    }

    private async Task AddToSiteUrlsBackgroundQueueAsync()
    {
        if (PageTitle.IsEmpty())
        {
            return;
        }

        var context = ApplicationState.HttpContext;
        var url = context.GetRawUrl();
        var isProtectedPage = ApplicationState.DoNotLogPageReferrer || context.IsProtectedRoute();
        var title = isProtectedPage ? "" : $"{Group}: {PageTitle.ToPersianNumbers()}";
        var lastVisitorStat = await SiteUrlsService.GetLastSiteUrlVisitorStatAsync(context);

        BackgroundQueueService.QueueBackgroundWorkItem(async (_, serviceProvider) =>
        {
            serviceProvider.GetRequiredService<IOnlineVisitorsService>().ProcessNewVisitor(lastVisitorStat);

            await serviceProvider.GetRequiredService<ISiteUrlsService>()
                .GetOrAddOrUpdateSiteUrlAsync(url, title, isProtectedPage, updateVisitsCount: true, lastVisitorStat);
        });
    }
}
