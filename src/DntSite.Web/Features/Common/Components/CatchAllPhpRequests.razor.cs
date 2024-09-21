namespace DntSite.Web.Features.Common.Components;

public partial class CatchAllPhpRequests
{
    [CascadingParameter] public HttpContext HttpContext { get; set; } = null!;

    [Parameter] public string? Data { get; set; }

    [Parameter] public string? SubDomain { get; set; }

    [Parameter] public string? Folder { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        HttpContext.Response.StatusCode = (int)HttpStatusCode.Gone;
    }
}
