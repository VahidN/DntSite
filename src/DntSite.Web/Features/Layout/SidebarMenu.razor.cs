using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Layout;

public partial class SidebarMenu
{
    private RecalculatePostsModel? _model;

    [InjectComponentScoped] internal IStatService StatService { set; get; } = null!;

    protected override async Task OnInitializedAsync() => _model = await StatService.GetStatAsync();
}
