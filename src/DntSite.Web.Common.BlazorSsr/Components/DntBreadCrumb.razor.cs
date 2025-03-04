namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom breadcrumb component
/// </summary>
public partial class DntBreadCrumb
{
    [CascadingParameter] internal HttpContext? HttpContext { set; get; }

    [Inject] internal NavigationManager NavigationManager { set; get; } = null!;

    /// <summary>
    ///     Additional user attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

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
    ///     Its default value is `fw-bold`.
    /// </summary>
    [Parameter]
    public string ActiveItemFontWeightClass { set; get; } = "fw-bold";

    private List<BreadCrumb>? GetBreadCrumbs()
    {
        if (BreadCrumbs is null)
        {
            return null;
        }

        var visibleItems = BreadCrumbs
            .Where(breadCrumb => !string.IsNullOrWhiteSpace(breadCrumb.Title) && IsBreadCrumbVisible(breadCrumb))
            .OrderBy(breadCrumb => breadCrumb.Order)
            .DistinctBy(breadCrumb => breadCrumb.Url)
            .ToList();

        SetFontWeightClass(visibleItems);
        SetActiveClass(visibleItems);

        return visibleItems;
    }

    private void SetActiveClass(List<BreadCrumb> visibleItems)
    {
        var lastBreadCrumbItem = BreadCrumbs?.Count > 0 ? BreadCrumbs[^1] : null;

        foreach (var item in visibleItems)
        {
            if (lastBreadCrumbItem is not null && lastBreadCrumbItem == item && MakeLastItemActive)
            {
                item.ActiveClass = "active";

                continue;
            }

            item.ActiveClass = item.IsActive ? "active" : "";
        }
    }

    private void SetFontWeightClass(List<BreadCrumb> visibleItems)
    {
        var currentRelativeUrl = "/".CombineUrl(NavigationManager.ToBaseRelativePath(NavigationManager.Uri),
            escapeRelativeUrl: false);

        if (currentRelativeUrl.IsEmpty())
        {
            return;
        }

        foreach (var item in visibleItems)
        {
            item.FontWeightClass = "";
        }

        var currentRelativeUrlSegments =
            currentRelativeUrl.Split(separator: '/', StringSplitOptions.RemoveEmptyEntries);

        Dictionary<BreadCrumb, int> matchedMap = [];

        foreach (var item in visibleItems.Where(item => !item.Url.IsEmpty()))
        {
            if (string.Equals(item.Url, currentRelativeUrl, StringComparison.OrdinalIgnoreCase))
            {
                item.FontWeightClass = ActiveItemFontWeightClass;

                return;
            }

            var itemUrlSegments = item.Url.Split(separator: '/', StringSplitOptions.RemoveEmptyEntries);

            var matchedSegmentsCount = itemUrlSegments.Count(itemUrlSegment
                => currentRelativeUrlSegments.Contains(itemUrlSegment, StringComparer.OrdinalIgnoreCase));

            matchedMap[item] = matchedSegmentsCount;
        }

        var itemWithMaxMatchedSegments = matchedMap.MaxBy(x => x.Value);

        if (itemWithMaxMatchedSegments.Value > 0)
        {
            itemWithMaxMatchedSegments.Key.FontWeightClass = ActiveItemFontWeightClass;
        }
    }

    private bool IsBreadCrumbVisible(BreadCrumb? breadCrumb)
        => breadCrumb is not null && HttpContext.HasUserAccess(breadCrumb.AllowAnonymous, breadCrumb.AllowedClaims,
            breadCrumb.AllowedRoles);
}
