namespace DntSite.Web.Features.Common.Components;

public partial class Error
{
    [CascadingParameter] public HttpContext? HttpContext { get; set; }

    [Parameter] public int? ResponseCode { get; set; }

    [Inject] internal ILogger<Error> Logger { get; set; } = null!;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var httpRequest = HttpContext?.Request;

        if (httpRequest is null)
        {
            return;
        }

        Logger.LogError("{Request}", httpRequest.LogRequest(ResponseCode));
        SetStatusCode();
    }

    private void SetStatusCode()
    {
        if (HttpContext is null || !ResponseCode.HasValue)
        {
            return;
        }

        HttpContext.Response.StatusCode = ResponseCode.Value;
    }
}
