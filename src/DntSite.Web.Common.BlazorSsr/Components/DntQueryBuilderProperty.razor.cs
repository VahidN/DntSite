namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     Defines a Table Column
/// </summary>
public partial class DntQueryBuilderProperty<TRecord>
    where TRecord : class
{
    /// <summary>
    ///     Defines the owner of this component.
    /// </summary>
    [CascadingParameter]
    public DntQueryBuilder<TRecord>? OwnerGrid { get; set; }

    private DntQueryBuilder<TRecord> SafeOwnerGrid => OwnerGrid ??
                                                      throw new InvalidOperationException(
                                                          message: "It should be placed inside the `DntQueryBuilder`.");

    /// <summary>
    ///     The display header.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    ///     How to calculate the value of a cell and also its column's title?
    ///     It's optional and if it's not specified the provided template will be used.
    /// </summary>
    [Parameter]
    public Expression<Func<TRecord, object?>>? ValueExpression { get; set; }

    /// <summary>
    ///     Defines the custom content of this column's query builder value.
    /// </summary>
    [Parameter]
    public RenderFragment<DntQueryBuilderSearchRule<TRecord>>? QueryBuilderValueTemplate { get; set; }

    /// <summary>
    ///     The Property index of the current cell
    /// </summary>
    public int PropertyIndex { private set; get; }

    /// <summary>
    ///     Get the property name from the expression.
    ///     For instance (customer => customer.Name) returns "Name".
    /// </summary>
    public string? PropertyPath { private set; get; }

    /// <summary>
    ///     The type of this property
    /// </summary>
    public string? PropertyType { private set; get; }

    /// <summary>
    ///     Method invoked when the component is ready to start
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        PropertyIndex = SafeOwnerGrid.AddProperty(this);
    }

    /// <summary>
    ///     Method invoked when the component has received parameters from its parent
    /// </summary>
    protected override void OnParametersSet()
    {
        PropertyPath = StronglyTyped.PropertyPath(ValueExpression, separator: "_");
        PropertyType = GetValueExpressionType();
    }

    private string? GetValueExpressionType()
    {
        var originalType = ValueExpression.GetObjectType();
        var underlyingType = originalType is null ? null : Nullable.GetUnderlyingType(originalType);

        return (underlyingType ?? originalType)?.ToString();
    }
}
