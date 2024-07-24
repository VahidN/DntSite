using DntSite.Web.Features.Advertisements.Models;
using DntSite.Web.Features.AppConfigs.Components;

namespace DntSite.Web.Features.Advertisements.Components;

public partial class AddAdvertisementForm
{
    [Parameter] [EditorRequired] public IList<string>? AutoCompleteDataList { get; set; }

    [Parameter] [EditorRequired] public EventCallback<WriteAdvertisementModel> OnValidSubmit { get; set; }

    [SupplyParameterFromForm(FormName = nameof(AddAdvertisementForm))]
    public WriteAdvertisementModel Model { set; get; } = new();

    [Parameter] public WriteAdvertisementModel? InitialModel { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (InitialModel is not null && ApplicationState.HttpContext.IsGetRequest())
        {
            Model = InitialModel;
        }
    }

    private async Task PerformAsync()
    {
        if (!OnValidSubmit.HasDelegate)
        {
            return;
        }

        await OnValidSubmit.InvokeAsync(new WriteAdvertisementModel
        {
            Title = Model.Title,
            Body = Model.Body,
            Tags = Model.Tags,
            DueDate = Model.DueDate.AddHours(Model.Hour ?? 0).AddMinutes(Model.Minute ?? 0)
        });
    }
}
