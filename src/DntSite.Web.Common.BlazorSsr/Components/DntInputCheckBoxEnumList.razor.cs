namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom InputCheckboxList component
/// </summary>
/// <typeparam name="TEnum">struct, Enum</typeparam>
public partial class DntInputCheckBoxEnumList<TEnum>
    where TEnum : struct, Enum
{
    private IReadOnlyDictionary<string, TEnum> _items = new Dictionary<string, TEnum>(StringComparer.Ordinal);

    /// <summary>
    ///     Additional user attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    ///     The InputCheckBoxList's margin bottom. Its default value is `3`.
    /// </summary>
    [Parameter]
    public int InputRowMarginBottom { get; set; } = 3;

    /// <summary>
    ///     The InputCheckBoxList's column width. Its default value is `10`.
    /// </summary>
    [Parameter]
    public int InputCheckBoxListColumnWidth { get; set; } = 10;

    /// <summary>
    ///     The label's column width of the custom InputCheckBoxList. Its default value is `2`.
    /// </summary>
    [Parameter]
    public int LabelColumnWidth { get; set; } = 2;

    /// <summary>
    ///     Sets the '@bind-SelectedValues' attribute to the provided string or object.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IList<TEnum>? SelectedValues { set; get; }

    /// <summary>
    ///     Specifies the field for which validation messages should be displayed.
    /// </summary>
    [Parameter]
    public Expression<Func<IList<TEnum>>> SelectedValuesExpression { get; set; } = default!;

    /// <summary>
    ///     The label name of the custom InputCheckBoxList
    /// </summary>
    [Parameter]
    public string LabelName { get; set; } = default!;

    /// <summary>
    ///     Should we display these checkboxes as `disabled readonly` elements?
    /// </summary>
    [Parameter]
    public bool IsReadOnly { get; set; }

    /// <summary>
    ///     Sets the '@oninput' attribute to the provided string or delegate value.
    /// </summary>
    [Parameter]
    public EventCallback<IList<TEnum>> SelectedValuesChanged { get; set; }

    private BlazorHtmlField<IList<TEnum>> SelectedValuesHtmlField => new(SelectedValuesExpression);

    /// <summary>
    ///     Method invoked when the component has received parameters from its parent.
    /// </summary>
    protected override void OnParametersSet() => SetItems();

    private void SetItems() => _items = ReflectionExtensions.GetEnumItems<TEnum>();

    private bool IsChecked(TEnum value) => SelectedValues?.Contains(value) == true;
}
