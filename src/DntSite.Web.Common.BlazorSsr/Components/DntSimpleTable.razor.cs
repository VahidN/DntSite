namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntSimpleTable<TRecord>
    where TRecord : class
{
    private List<DntSimpleTableColumn<TRecord>> DefinedColumns { get; } = new();

    private bool IsFooterDefined => DefinedColumns.Any(c => c.FooterCellValueTemplate is not null);

    /// <summary>
    ///     Defines the CSS class of the rendering header's footer (the `tfoot` element).
    /// </summary>
    [Parameter]
    public string? FooterRowClass { get; set; }

    /// <summary>
    ///     Defines the rest of the custom attributes.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    ///     Defines list if the table's records to display.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IList<TRecord>? Records { set; get; }

    /// <summary>
    ///     Defines a heading for a table. It helps users with screen readers to find a table and understand
    ///     what it’s about and decide if they want to read it.
    /// </summary>
    [Parameter]
    public RenderFragment? CaptionTemplate { get; set; }

    /// <summary>
    ///     Defines the CSS class of the rendering row (the `tr` element) based on the index of the row and it value.
    /// </summary>
    [Parameter]
    public Func<TRecord?, string>? RowClass { get; set; }

    /// <summary>
    ///     Defines the CSS class of the rendering table (the `table` element).
    /// </summary>
    [Parameter]
    public string? TableClass { get; set; }

    /// <summary>
    ///     Defines the CSS class of the rendering header's row (the `thead` element).
    /// </summary>
    [Parameter]
    public string? HeaderClass { get; set; }

    /// <summary>
    ///     When there is no record to display, this template will be displayed.
    /// </summary>
    [Parameter]
    public RenderFragment? DataSourceIsEmptyTemplate { get; set; }

    /// <summary>
    ///     The descendant components (defined in ChildContent) to receive the specified CascadingValue.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment? TableColumns { get; set; }

    /// <summary>
    ///     Defines the CSS class of the rendering table's enclosing div (the outer `div` element).
    /// </summary>
    [Parameter]
    public string? TableResponsiveClass { get; set; }

    internal void AddColumn(DntSimpleTableColumn<TRecord> property) => DefinedColumns.Add(property);
}
