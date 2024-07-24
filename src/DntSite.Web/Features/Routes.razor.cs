using Microsoft.AspNetCore.Components.Web;

namespace DntSite.Web.Features;

public partial class Routes
{
    private ErrorBoundary? OurErrorBoundary { set; get; }

    // On each page navigation, reset any error state
    protected override void OnParametersSet() => OurErrorBoundary?.Recover();
}
