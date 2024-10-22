namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom paginator
/// </summary>
public partial class DntSimplePaginator
{
    private bool _hasPagination;

    private int _pagerEnd;

    private int _pagerStart;

    private int _totalPages;

    [Parameter] [EditorRequired] public required string BasePath { set; get; }

    //NavigationManager.GetCurrentRelativePath().TrimEnd('/');

    [Inject] internal NavigationManager NavigationManager { set; get; } = null!;

    /// <summary>
    ///     Its default value is `3`.
    /// </summary>
    [Parameter]
    public int MarginTop { get; set; } = 3;

    [Parameter] public int? CurrentPage { set; get; }

    /// <summary>
    ///     Number of pages to display. Its default value is `10`.
    /// </summary>
    [Parameter]
    public int MaxPagerItems { get; set; } = 10;

    /// <summary>
    ///     Defines the CSS class of the rendering Pagination component (the `ul` element).
    ///     Its default value is `pagination shadow-sm`.
    /// </summary>
    [Parameter]
    public string PaginationClass { get; set; } = "pagination shadow-sm";

    /// <summary>
    ///     Defines the CSS class of the rendering Pagination component (the `li` elements).
    ///     Its default value is `page-item`.
    /// </summary>
    [Parameter]
    public string PaginationItemClass { get; set; } = "page-item";

    /// <summary>
    ///     Defines the CSS class of the rendering Pagination component (the `anchor` elements).
    ///     Its default value is `page-link`.
    /// </summary>
    [Parameter]
    public string PaginationPageLinkClass { get; set; } = "page-link";

    /// <summary>
    ///     When there is no record to display, this message will be displayed.
    /// </summary>
    [Parameter]
    public string? DataSourceIsEmptyMessage { get; set; } = "اطلاعاتی برای نمایش یافت نشد.";

    [Parameter] [EditorRequired] public int TotalItemCount { set; get; }

    [Parameter] [EditorRequired] public int ItemsPerPage { set; get; }

    /// <summary>
    ///     Its default value is `header`.
    /// </summary>
    [Parameter]
    public string PaginationScrollToId { get; set; } = "header";

    protected override void OnParametersSet()
    {
        ReDrawPagination();
        base.OnParametersSet();
    }

    private void ReDrawPagination()
    {
        if (TotalItemCount == 0)
        {
            return;
        }

        _totalPages = (int)Math.Ceiling(TotalItemCount / (double)ItemsPerPage);
        _hasPagination = _totalPages > 1;
        SetPagerBounds();
    }

    private void SetPagerBounds()
    {
        if (_totalPages <= MaxPagerItems)
        {
            _pagerStart = 1;
            _pagerEnd = _totalPages;
        }
        else
        {
            CurrentPage ??= 1;

            var maxPagesBeforeCurrentPage = (int)Math.Floor(MaxPagerItems / (decimal)2);
            var maxPagesAfterCurrentPage = (int)Math.Ceiling(MaxPagerItems / (decimal)2) - 1;

            if (CurrentPage.Value <= maxPagesBeforeCurrentPage)
            {
                _pagerStart = 1;
                _pagerEnd = MaxPagerItems;
            }
            else if (CurrentPage.Value + maxPagesAfterCurrentPage >= _totalPages)
            {
                _pagerStart = _totalPages - MaxPagerItems + 1;
                _pagerEnd = _totalPages;
            }
            else
            {
                _pagerStart = CurrentPage.Value - maxPagesBeforeCurrentPage;
                _pagerEnd = CurrentPage.Value + maxPagesAfterCurrentPage;
            }
        }
    }
}
