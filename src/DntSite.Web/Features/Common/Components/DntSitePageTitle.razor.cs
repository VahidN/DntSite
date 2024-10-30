using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Common.Components;

public partial class DntSitePageTitle
{
    private string? _localTitle;

    private string? _publicTitle;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] [EditorRequired] public required string PageTitle { set; get; }

    [Parameter] [EditorRequired] public required string Group { set; get; }

    [Parameter] [EditorRequired] public int? CurrentPage { set; get; }

    [Inject] internal IBackgroundQueueService BackgroundQueueService { set; get; } = null!;

    [InjectComponentScoped] internal ISiteUrlsService SiteUrlsService { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        (_publicTitle, _localTitle) = GetCurrentPageTitle();
        await AddToSiteUrlsBackgroundQueueAsync();
    }

    private (string PulicTitle, string LocalTitle) GetCurrentPageTitle()
    {
        if (PageTitle.IsEmpty())
        {
            return ("", "");
        }

        var page = "";

        if (CurrentPage is > 0)
        {
            page = string.Create(CultureInfo.InvariantCulture, $"، صفحه {CurrentPage.Value}");
        }

        var localTitle = $"{Group}: {PageTitle}{page}".ToPersianNumbers();
        var publicTitle = $"{ApplicationState.AppSetting?.BlogName} | {localTitle}".ToPersianNumbers();

        return (publicTitle, localTitle);
    }

    private async Task AddToSiteUrlsBackgroundQueueAsync()
    {
        var context = ApplicationState.HttpContext;
        var baseUrl = context.GetBaseUrl();
        var referrerUrl = context.GetReferrerUrl();
        var destinationUrl = context.GetRawUrl();
        var isProtectedPage = ApplicationState.DoNotLogPageReferrer || context.IsProtectedRoute();
        var title = isProtectedPage ? "" : _localTitle;
        var lastVisitorStat = await SiteUrlsService.GetLastSiteUrlVisitorStatAsync(context);

        BackgroundQueueService.QueueBackgroundWorkItem(async (_, serviceProvider) =>
        {
            UpdateOnlineVisitorsInfo(serviceProvider);
            await UpdateSiteUrlAsync(serviceProvider);
            await UpdateReferrerAsync(serviceProvider);
        });

        void UpdateOnlineVisitorsInfo(IServiceProvider serviceProvider)
            => serviceProvider.GetRequiredService<IOnlineVisitorsService>().ProcessNewVisitor(lastVisitorStat);

        Task UpdateSiteUrlAsync(IServiceProvider serviceProvider)
            => serviceProvider.GetRequiredService<ISiteUrlsService>()
                .GetOrAddOrUpdateSiteUrlAsync(destinationUrl, title, isProtectedPage, updateVisitsCount: true,
                    lastVisitorStat);

        Task UpdateReferrerAsync(IServiceProvider serviceProvider)
            => serviceProvider.GetRequiredService<ISiteReferrersService>()
                .TryAddOrUpdateReferrerAsync(referrerUrl, destinationUrl, baseUrl, lastVisitorStat, isProtectedPage);
    }
}
