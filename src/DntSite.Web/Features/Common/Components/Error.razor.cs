using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Common.Components;

public partial class Error
{
    [CascadingParameter] public HttpContext? HttpContext { get; set; }

    [Parameter] public int? ResponseCode { get; set; }

    [Inject] internal ILogger<Error> Logger { get; set; } = null!;

    [Inject] internal ISpidersService SpidersService { get; set; } = null!;

    [Inject] public NavigationManager NavigationManager { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private bool IsThisPageCalledDirectly => string.Equals(HttpContext?.GetCurrentUrl(), b: "/error/404",
        StringComparison.OrdinalIgnoreCase);

    protected override async Task OnInitializedAsync()
    {
        ApplicationState.DoNotLogPageReferrer = true;

        var httpRequest = HttpContext?.Request;

        if (httpRequest is null)
        {
            return;
        }

        if (!IsThisPageCalledDirectly && !await SpidersService.IsSpiderClientAsync(HttpContext))
        {
            Logger.LogError(message: "{Request}", httpRequest.LogRequest(ResponseCode));
        }

        SetStatusCode();
    }

    private void SetStatusCode()
    {
        if (HttpContext is null || !ResponseCode.HasValue)
        {
            return;
        }

        HttpContext.Response.StatusCode = ResponseCode.Value == (int)HttpStatusCode.NotFound
            ? (int)HttpStatusCode.Gone
            : ResponseCode.Value;
    }
}
