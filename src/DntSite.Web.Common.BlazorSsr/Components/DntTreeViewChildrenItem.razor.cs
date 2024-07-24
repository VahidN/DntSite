namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom DntTreeView
/// </summary>
public partial class DntTreeViewChildrenItem<TRecord>
{
    /// <summary>
    ///     Defines the owner of this component.
    /// </summary>
    [CascadingParameter]
    public DntTreeView<TRecord>? OwnerTreeView { get; set; }

    internal DntTreeView<TRecord> SafeOwnerTreeView => OwnerTreeView ??
                                                       throw new InvalidOperationException(
                                                           "`DntTreeViewChildrenItem` should be placed inside of a `DntTreeView`.");

    /// <summary>
    ///     Nested parent item to display
    /// </summary>
    [Parameter]
    public TRecord? ParentItem { set; get; }

    private IEnumerable<TRecord>? Children => ParentItem is null || SafeOwnerTreeView.CompiledChildrenSelector is null
        ? null
        : SafeOwnerTreeView.CompiledChildrenSelector(ParentItem);
}
