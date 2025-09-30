namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom DntInputTime component
/// </summary>
public partial class DntInputTime
{
    /// <summary>
    ///     Additional user attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    ///     The optional label name of the custom InputText
    /// </summary>
    [Parameter]
    public string? OptionalLabel { get; set; }

    /// <summary>
    ///     The label name of the custom InputText
    /// </summary>
    [Parameter]
    public string LabelName { get; set; } = default!;

    /// <summary>
    ///     Input field's icon from https://icons.getbootstrap.com/.
    /// </summary>
    [Parameter]
    public string FieldIcon { set; get; } = default!;

    /// <summary>
    ///     The InputText's margin bottom. Its default value is `3`.
    /// </summary>
    [Parameter]
    public int InputRowMarginBottom { get; set; } = 3;

    /// <summary>
    ///     The InputText's column width. Its default value is `10`.
    /// </summary>
    [Parameter]
    public int InputTextColumnWidth { get; set; } = 10;

    /// <summary>
    ///     The label's column width of the custom InputText. Its default value is `2`.
    /// </summary>
    [Parameter]
    public int LabelColumnWidth { get; set; } = 2;

    /// <summary>
    ///     How to display the minutes dropdown. Minute interval.
    /// </summary>
    [Parameter]
    public int MinutesSteps { get; set; } = 5;

    /// <summary>
    ///     Its default value is `Hour`.
    /// </summary>
    [Parameter]
    public string HourLabel { set; get; } = "Hour";

    /// <summary>
    ///     Its default value is `Minute`.
    /// </summary>
    [Parameter]
    public string MinuteLabel { set; get; } = "Minute";

    private string UniqueId { get; } = Guid.NewGuid().ToString(format: "N");

    [Parameter] public Expression<Func<int?>>? HourExpression { get; set; }

    [Parameter] public int? Hour { get; set; }

    [Parameter] public EventCallback<int?> HourChanged { get; set; }

    [Parameter] public Expression<Func<int?>>? MinuteExpression { get; set; }

    [Parameter] public int? Minute { get; set; }

    [Parameter] public EventCallback<int?> MinuteChanged { get; set; }
}
