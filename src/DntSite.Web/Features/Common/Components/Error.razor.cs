using DntSite.Web.Features.Common.RoutingConstants;
using DntSite.Web.Features.Common.Utils.WebToolkit;

namespace DntSite.Web.Features.Common.Components;

public partial class Error
{
    [CascadingParameter] public HttpContext? HttpContext { get; set; }

    [Parameter] public int? ResponseCode { get; set; }

    [Inject] internal ILogger<Error> Logger { get; set; } = null!;

    [Inject] public NavigationManager NavigationManager { set; get; } = null!;

    private bool IsThisPageCalledDirectly => string.Equals(HttpContext?.GetCurrentUrl(), b: "/error/404",
        StringComparison.OrdinalIgnoreCase);

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var httpRequest = HttpContext?.Request;

        if (httpRequest is null)
        {
            return;
        }

        if (HttpContext.IsNoneAspNetCoreRequest())
        {
            NavigationManager.NavigateTo(CommonRoutingConstants.CatchAllPhpRequestsPath);

            return;
        }

        if (!IsThisPageCalledDirectly)
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
