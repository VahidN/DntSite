namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom DntTreeView
/// </summary>
public partial class DntTreeView<TRecord>
{
    private Expression? _lastCompiledExpression;

    internal Func<TRecord, IEnumerable<TRecord>?>? CompiledChildrenSelector { private set; get; }

    /// <summary>
    ///     Additional user attributes. HTML attributes appended to the root ul node.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    ///     HTML attributes appended to the li nodes. HTML attributes appended to the children items.
    /// </summary>
    [Parameter]
    public IReadOnlyDictionary<string, object> ChildrenHtmlAttributes { get; set; } =
        new Dictionary<string, object>(StringComparer.Ordinal)
        {
            {
                "style", "list-style: none;"
            }
        };

    /// <summary>
    ///     The treeview's self-referencing items
    /// </summary>
    [Parameter]
    public IEnumerable<TRecord>? Items { set; get; }

    /// <summary>
    ///     The treeview item's template
    /// </summary>
    [Parameter]
    public RenderFragment<TRecord>? ItemTemplate { set; get; }

    /// <summary>
    ///     The content displayed if the list is empty
    /// </summary>
    [Parameter]
    public RenderFragment? EmptyContentTemplate { set; get; }

    /// <summary>
    ///     The property which returns the children items
    /// </summary>
    [Parameter]
    public Expression<Func<TRecord, IEnumerable<TRecord>?>>? ChildrenSelector { set; get; }

    /// <summary>
    ///     Method invoked when the component has received parameters from its parent
    /// </summary>
    protected override void OnParametersSet()
    {
        if (_lastCompiledExpression != ChildrenSelector)
        {
            CompiledChildrenSelector = ChildrenSelector?.Compile();
            _lastCompiledExpression = ChildrenSelector;
        }
    }
}
