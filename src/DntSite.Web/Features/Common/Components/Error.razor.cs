using DntSite.Web.Features.Common.RoutingConstants;
using DntSite.Web.Features.Stats.Middlewares.Contracts;

namespace DntSite.Web.Features.Common.Components;

[DoNotLogReferrer]
public partial class Error
{
    [CascadingParameter] public HttpContext? HttpContext { get; set; }

    [Parameter] public int? ResponseCode { get; set; }

    [Inject] internal ILogger<Error> Logger { get; set; } = null!;

    [Inject] internal IUAParserService UaParserService { get; set; } = null!;

    [Inject] public NavigationManager NavigationManager { set; get; } = null!;

    private bool IsThisPageCalledDirectly => string.Equals(HttpContext?.GetCurrentUrl(), b: "/error/404",
        StringComparison.OrdinalIgnoreCase);

    protected override async Task OnInitializedAsync()
    {
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

        if (!IsThisPageCalledDirectly && !await UaParserService.IsSpiderClientAsync(HttpContext))
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
