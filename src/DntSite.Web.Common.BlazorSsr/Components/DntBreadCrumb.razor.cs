namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom breadcrumb component
/// </summary>
public partial class DntBreadCrumb
{
    private BreadCrumb? _lastBreadCrumbItem;

    private IEnumerable<BreadCrumb>? BreadCrumbsToShow => BreadCrumbs
        ?.Where(breadCrumb => !string.IsNullOrWhiteSpace(breadCrumb.Title) && IsBreadCrumbVisible(breadCrumb))
        .OrderBy(breadCrumb => breadCrumb.Order)
        .DistinctBy(breadCrumb => breadCrumb.Url);

    [CascadingParameter] internal HttpContext? HttpContext { set; get; }

    /// <summary>
    ///     Additional user attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; } =
        new Dictionary<string, object>(StringComparer.Ordinal);

    /// <summary>
    ///     The list of BreadCrumbs to display.
    /// </summary>
    [Parameter]
    public IList<BreadCrumb>? BreadCrumbs { set; get; }

    /// <summary>
    ///     Its default value is `3`.
    /// </summary>
    [Parameter]
    public int Margin { set; get; } = 3;

    /// <summary>
    ///     Its default value is `❱`.
    /// </summary>
    [Parameter]
    public string BreadCrumbDivider { set; get; } = "❱";

    /// <summary>
    ///     Adds an `active` class to the last item, if it sets to true.
    /// </summary>
    [Parameter]
    public bool MakeLastItemActive { set; get; }

    /// <summary>
    ///     Method invoked when the component has received parameters from its parent
    /// </summary>
    protected override void OnParametersSet()
    {
        if (BreadCrumbs is not null && BreadCrumbs.Count > 0)
        {
            _lastBreadCrumbItem = BreadCrumbs[^1];
        }
    }

    private string GetActiveClass(BreadCrumb item)
    {
        if (_lastBreadCrumbItem is not null && _lastBreadCrumbItem == item && MakeLastItemActive)
        {
            return "active";
        }

        return item.IsActive ? "active" : "";
    }

    private bool IsBreadCrumbVisible(BreadCrumb? breadCrumb)
        => breadCrumb is not null && HttpContext.HasUserAccess(breadCrumb.AllowAnonymous, breadCrumb.AllowedClaims,
            breadCrumb.AllowedRoles);
}
