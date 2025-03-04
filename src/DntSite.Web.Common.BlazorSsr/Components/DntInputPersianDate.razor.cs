using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntInputPersianDate
{
    /// <summary>
    ///     The InputText's margin bottom. Its default value is `3`.
    /// </summary>
    [Parameter]
    public int InputRowMarginBottom { get; set; } = 3;

    /// <summary>
    ///     The InputText's column width. Its default value is `9`.
    /// </summary>
    [Parameter]
    public int InputTextColumnWidth { get; set; } = 9;

    /// <summary>
    ///     The label's column width of the custom InputText. Its default value is `3`.
    /// </summary>
    [Parameter]
    public int LabelColumnWidth { get; set; } = 3;

    /// <summary>
    ///     The label name of the custom InputText
    /// </summary>
    [Parameter]
    public string? LabelName { get; set; }

    /// <summary>
    ///     Input field's icon from https://icons.getbootstrap.com/.
    ///     Its default value is `bi bi-calendar`.
    /// </summary>
    [Parameter]
    public string FieldIcon { set; get; } = DntBootstrapIcons.BiCalendar;

    [Parameter] public Expression<Func<int?>>? DayExpression { get; set; }

    [Parameter] [EditorRequired] public int? Day { get; set; }

    [Parameter] public EventCallback<int?> DayChanged { get; set; }

    [Parameter] public Expression<Func<int?>>? MonthExpression { get; set; }

    [Parameter] [EditorRequired] public int? Month { get; set; }

    [Parameter] public EventCallback<int?> MonthChanged { get; set; }

    [Parameter] public Expression<Func<int?>>? YearExpression { get; set; }

    [Parameter] [EditorRequired] public int? Year { get; set; }

    [Parameter] public EventCallback<int?> YearChanged { get; set; }

    /// <summary>
    ///     Its default value is 1300
    /// </summary>
    [Parameter]
    public int FromYear { get; set; } = 1300;

    /// <summary>
    ///     Its default value is 1400
    /// </summary>
    [Parameter]
    public int ToYear { get; set; } = 1400;

    /// <summary>
    ///     Its default value is `روز`.
    /// </summary>
    [Parameter]
    public string DayLabel { set; get; } = "روز";

    /// <summary>
    ///     Its default value is `ماه`.
    /// </summary>
    [Parameter]
    public string MonthLabel { set; get; } = "ماه";

    /// <summary>
    ///     Its default value is `سال`.
    /// </summary>
    [Parameter]
    public string YearLabel { set; get; } = "سال";

    [Parameter] public string? PersianDate { set; get; }

    [Parameter] public EventCallback<string?> PersianDateChanged { get; set; }

    [Parameter] public Expression<Func<string?>>? PersianDateExpression { get; set; }

    private string? PersianDateValue => Year is null || Month is null || Day is null
        ? null
        : string.Create(CultureInfo.InvariantCulture, $"{Year.Value}/{Month.Value:00}/{Day.Value:00}");

    [Parameter] public DateTime? Date { set; get; }

    [Parameter] public EventCallback<DateTime?> DateChanged { get; set; }

    [Parameter] public Expression<Func<DateTime?>>? DateExpression { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        InitDateNumbersIfDateIsSet();
        InitDateNumbersIfPersianDateIsSet();
        SetDateValueCallbacks();
    }

    private void InitDateNumbersIfPersianDateIsSet()
    {
        if (string.IsNullOrWhiteSpace(PersianDate))
        {
            return;
        }

        var result = PersianDate.ToPersianDateTime();

        if (result?.IsValidDateTime != true)
        {
            return;
        }

        SetDateNumberValues(new PersianDay(result.Year!.Value, result.Month!.Value, result.Day!.Value));
    }

    private void InitDateNumbersIfDateIsSet()
    {
        if (!Date.HasValue)
        {
            return;
        }

        var result = Date.Value.ToPersianYearMonthDay();
        SetDateNumberValues(result);
    }

    private void SetDateNumberValues(PersianDay result)
    {
        if (YearChanged.HasDelegate)
        {
            _ = YearChanged.InvokeAsync(result.Year);
        }

        if (MonthChanged.HasDelegate)
        {
            _ = MonthChanged.InvokeAsync(result.Month);
        }

        if (DayChanged.HasDelegate)
        {
            _ = DayChanged.InvokeAsync(result.Day);
        }
    }

    private void SetDateValueCallbacks()
    {
        var value = PersianDateValue;

        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        if (PersianDateChanged.HasDelegate)
        {
            _ = PersianDateChanged.InvokeAsync(value);
        }

        if (DateChanged.HasDelegate)
        {
            _ = DateChanged.InvokeAsync(value.ToGregorianDateTime());
        }
    }
}
