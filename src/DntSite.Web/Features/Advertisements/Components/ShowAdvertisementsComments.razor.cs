using System.Text;
using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Advertisements.RoutingConstants;
using DntSite.Web.Features.Advertisements.Services.Contracts;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;

namespace DntSite.Web.Features.Advertisements.Components;

public partial class ShowAdvertisementsComments
{
    private const int PostItemsPerPage = 10;
    private const string MainPageTitle = "نظرات آگهی‌ها";
    private const string MainPageUrl = AdvertisementsRoutingConstants.AdvertisementsComments;

    private static readonly CompositeFormat PostUrlTemplate =
        CompositeFormat.Parse(AdvertisementsRoutingConstants.PostUrlTemplate);

    private PagedResultModel<AdvertisementComment>? _posts;

    [MemberNotNullWhen(returnValue: true, nameof(UserFriendlyName))]
    private bool HasUserFriendlyName => !string.IsNullOrWhiteSpace(UserFriendlyName);

    private string MainTitle => !HasUserFriendlyName ? MainPageTitle : MainUserPageTitle;

    private string MainUserPageTitle => $"آرشیو نظرات آگهی‌های {UserFriendlyName}";

    private string MainUserPageUrl
        => !HasUserFriendlyName ? MainPageUrl : $"{MainPageUrl}/{Uri.EscapeDataString(UserFriendlyName)}";

    private string BasePath => !HasUserFriendlyName ? MainPageUrl : MainUserPageUrl;

    [Parameter] public string? UserFriendlyName { set; get; }

    [InjectComponentScoped] internal IAdvertisementCommentsService AdvertisementCommentsService { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (HasUserFriendlyName)
        {
            await ShowUserCommentsAsync();
        }
        else
        {
            await ShowCommentsListAsync();
        }
    }

    private async Task ShowCommentsListAsync()
    {
        CurrentPage ??= 1;

        _posts = await AdvertisementCommentsService.GetPagedLastAdvertisementCommentsIncludeBlogPostAndUserAsync(
            CurrentPage.Value - 1, PostItemsPerPage);

        AddCommentsListBreadCrumbs();
    }

    private void AddCommentsListBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..AdvertisementsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
            {
                Title = MainTitle,
                Url = MainPageUrl,
                GlyphIcon = DntBootstrapIcons.BiPerson
            }
        ]);

    private async Task ShowUserCommentsAsync()
    {
        CurrentPage ??= 1;

        _posts = await AdvertisementCommentsService.GetLastPagedAdvertisementsCommentsAsync(UserFriendlyName!,
            CurrentPage.Value - 1, PostItemsPerPage);

        AddUserCommentsBreadCrumbs();
    }

    private void AddUserCommentsBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..AdvertisementsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
            {
                Title = MainPageTitle,
                Url = MainPageUrl,
                GlyphIcon = DntBootstrapIcons.BiPerson
            },
            new BreadCrumb
            {
                Title = MainUserPageTitle,
                Url = MainUserPageUrl,
                GlyphIcon = DntBootstrapIcons.BiPerson
            }
        ]);

    private static string GetPostAbsoluteUrl(AdvertisementComment comment)
        => string.Format(CultureInfo.InvariantCulture, PostUrlTemplate, comment.ParentId);
}
