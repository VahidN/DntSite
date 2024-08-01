namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntSimpleTableColumn<TRecord>
    where TRecord : class
{
    private Func<TRecord, object?>? _compiledExpression;
    private Expression? _lastCompiledExpression;

    /// <summary>
    ///     Defines the rest of the custom attributes for the current `td`.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    ///     Defines the owner of this component.
    /// </summary>
    [CascadingParameter]
    public DntSimpleTable<TRecord>? OwnerGrid { get; set; }

    private DntSimpleTable<TRecord> SafeOwnerGrid => OwnerGrid ??
                                                     throw new InvalidOperationException(
                                                         message: "It should be placed inside the `DntSimpleTable`.");

    /// <summary>
    ///     The display header.
    /// </summary>
    [Parameter]
    public string? HeaderTitle { get; set; }

    /// <summary>
    ///     Defines the custom content of this column's header.
    /// </summary>
    [Parameter]
    public RenderFragment? HeaderTitleTemplate { get; set; }

    /// <summary>
    ///     Defines the CSS class of the rendering header's cell (the `th` element) based on its title.
    /// </summary>
    [Parameter]
    public string? HeaderCellClass { get; set; }

    /// <summary>
    ///     Defines the CSS class of the rendering row's cell (the `td` element) based on its value.
    /// </summary>
    [Parameter]
    public string? FooterCellClass { get; set; }

    /// <summary>
    ///     Defines the CSS class of the rendering row's cell (the `td` element) based on its value.
    /// </summary>
    [Parameter]
    public Func<TRecord?, string>? RowCellClass { get; set; }

    /// <summary>
    ///     How to calculate the value of a cell and also its column's title?
    ///     It's optional and if it's not specified the provided template will be used.
    /// </summary>
    [Parameter]
    public Expression<Func<TRecord, object?>>? CellValueExpression { get; set; }

    /// <summary>
    ///     Defines the custom content of the footer of this column's cell.
    /// </summary>
    [Parameter]
    public RenderFragment<IEnumerable<TRecord>?>? FooterCellValueTemplate { get; set; }

    /// <summary>
    ///     Defines the custom content of this column's  value.
    /// </summary>
    [Parameter]
    public RenderFragment<TRecord>? CellValueTemplate { get; set; }

    /// <summary>
    ///     The optional cell's value format.
    /// </summary>
    [Parameter]
    public string? ValueFormat { get; set; }

    /// <summary>
    ///     It's default value is `true`. You can use it to apply a security trimming to this column.
    ///     Should it be visible to all, or to some users?
    /// </summary>
    [Parameter]
    public bool IsVisible { get; set; } = true;

    /// <summary>
    ///     Method invoked when the component is ready to start
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (IsVisible)
        {
            SafeOwnerGrid.AddColumn(this);
        }
    }

    /// <summary>
    ///     Method invoked when the component has received parameters from its parent
    /// </summary>
    protected override void OnParametersSet()
    {
        if (_lastCompiledExpression != CellValueExpression)
        {
            _compiledExpression = CellValueExpression?.Compile();
            _lastCompiledExpression = CellValueExpression;
        }
    }

    internal string? GetFormattedValue(TRecord? record)
    {
        if (record is null || _compiledExpression is null)
        {
            return null;
        }

        if (CellValueTemplate is not null)
        {
            return null;
        }

        var value = _compiledExpression(record);

        return string.IsNullOrWhiteSpace(ValueFormat)
            ? string.Format(CultureInfo.InvariantCulture, format: "{0}", value)
            : string.Format(CultureInfo.InvariantCulture, ValueFormat, value);
    }
}
