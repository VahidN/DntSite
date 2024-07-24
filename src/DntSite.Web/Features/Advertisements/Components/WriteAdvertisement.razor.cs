using System.Text;
using AutoMapper;
using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Advertisements.Models;
using DntSite.Web.Features.Advertisements.RoutingConstants;
using DntSite.Web.Features.Advertisements.Services.Contracts;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Common.Services.Contracts;

namespace DntSite.Web.Features.Advertisements.Components;

[Authorize]
public partial class WriteAdvertisement
{
    private static readonly CompositeFormat PostUrlTemplate =
        CompositeFormat.Parse(AdvertisementsRoutingConstants.PostUrlTemplate);

    private AdvertisementType _advertisementType = AdvertisementType.Special;
    private WriteAdvertisementModel? _initialModel;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    public IList<string>? AutoCompleteDataList { get; set; }

    [InjectComponentScoped] internal ITagsService TagsService { set; get; } = null!;

    [Parameter] public int? EditId { set; get; }

    [Parameter] public string? AdvertisementKind { set; get; }

    [Parameter] public int? DeleteId { set; get; }

    [InjectComponentScoped] internal IAdvertisementsService AdvertisementsService { set; get; } = null!;

    [Inject] internal IMapper Mapper { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        SetAdvertisementType();
        AutoCompleteDataList = await TagsService.GetTagNamesArrayAsync(count: 2000);
        AddBreadCrumbs();

        if (!ApplicationState.HttpContext.IsGetRequest())
        {
            return;
        }

        await PerformPossibleDeleteAsync();

        await FillPossibleEditFormAsync();
    }

    private void SetAdvertisementType()
    {
        if (string.IsNullOrWhiteSpace(AdvertisementKind))
        {
            _advertisementType = AdvertisementType.Special;
        }
        else
        {
            Enum.TryParse(AdvertisementKind, ignoreCase: true, out _advertisementType);
        }
    }

    private async Task PerformPossibleDeleteAsync()
    {
        if (!DeleteId.HasValue)
        {
            return;
        }

        var advertisement = await GetUserAdvertisementAsync(DeleteId.Value);
        await AdvertisementsService.MarkAsDeletedAsync(advertisement);
        await AdvertisementsService.NotifyDeleteChangesAsync(advertisement);

        ApplicationState.NavigateTo(AdvertisementsRoutingConstants.Advertisements);
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([..AdvertisementsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task FillPossibleEditFormAsync()
    {
        if (!EditId.HasValue)
        {
            return;
        }

        var item = await GetUserAdvertisementAsync(EditId.Value);

        if (item is null)
        {
            return;
        }

        _initialModel = Mapper.Map<Advertisement, WriteAdvertisementModel>(item);
    }

    private async Task<Advertisement?> GetUserAdvertisementAsync(int id)
    {
        var advertisement = await AdvertisementsService.GetAdvertisementAsync(id);

        if (advertisement is null || !ApplicationState.CanCurrentUserEditThisItem(advertisement.UserId,
                advertisement.Audit.CreatedAt))
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return null;
        }

        return advertisement;
    }

    private async Task PerformAsync(WriteAdvertisementModel model)
    {
        var user = ApplicationState.CurrentUser?.User;

        Advertisement? advertisement;

        if (EditId.HasValue)
        {
            advertisement = await GetUserAdvertisementAsync(EditId.Value);
            await AdvertisementsService.UpdateAdvertisementAsync(advertisement, model);
        }
        else
        {
            advertisement = await AdvertisementsService.AddAdvertisementAsync(model, user);
        }

        await AdvertisementsService.NotifyAddOrUpdateChangesAsync(advertisement);

        ApplicationState.NavigateTo(string.Format(CultureInfo.InvariantCulture, PostUrlTemplate, advertisement?.Id));
    }
}
